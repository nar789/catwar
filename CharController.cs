using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;

public class CharController : MonoBehaviour
{

    public FloatingJoystick joystick;
    NavMeshAgent agent;

    bool isHit = false;

    CatCatcher catCatcher;
    
    SpriteRenderer renderer;

    public Image batteryFill;
    public Image batteryFill2;
    public TMPro.TextMeshProUGUI batteryText;
    public TMPro.TextMeshProUGUI batteryText2;
    Color32 originBatteryColor;
    Color32 attackedBatteryColor;
    Color32 attackedBatteryTextColor;
    Color32 attackedRoboColor;

    public TMPro.TextMeshProUGUI hitText;

    public Sprite[] sprites;
    public int spriteIdx = 0;

    public GameObject[] weapons;
    public int weaponIdx = 0;
    int lastWeaponIdx = 0;

    public WeaponRotater rotater;

    float rotateLastTime = 0f;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        catCatcher = GetComponent<CatCatcher>();
        renderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

        originBatteryColor = new Color32(49, 149, 255, 255);
        attackedBatteryColor = new Color32(176, 0, 77, 255);
        attackedBatteryTextColor = new Color32(255, 64, 140, 255);
        attackedRoboColor = new Color32(255, 155, 155, 255);

        updateBatteryView(false);

        hitText.text = "";
    }

    private void updateBatteryView(bool isAttack)
    {
        if(isAttack)
        {
            batteryFill.color = attackedBatteryColor;
            batteryText.color = attackedBatteryTextColor;
            batteryFill2.color = attackedBatteryColor;
            batteryText2.color = attackedBatteryTextColor;
        } else
        {
            batteryFill.color = originBatteryColor;
            batteryText.color = Color.white;
            batteryFill2.color = originBatteryColor;
            batteryText2.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateSprite();
        updateWeapon();
    }



    private void updateWeapon()
    {
        int idx = GameController.Instance.getMyWeaponIdx();
        if (lastWeaponIdx == idx)
        {
            return;
        }
        weapons[lastWeaponIdx].SetActive(false);
        weapons[idx].SetActive(true);
        lastWeaponIdx = idx;
    }


    private void updateSprite()
    {
        int idx = GameController.Instance.getMySkinIdX();
        Sprite sprite = GameController.Instance.getSkinSprite(idx);
        if (renderer.sprite != sprite)
        {
            renderer.sprite = sprite;
        }
    }

    private void FixedUpdate()
    {
        /*
        if(isHit)
        {
            transform.Rotate(0, 50f * Time.deltaTime, 0);
            return;
        }*/
        if(rotateLastTime != 0 && Time.time - rotateLastTime > 1f)
        {
            stopWeaponRotate();
            rotateLastTime = 0;
        }



        if(isMoving())
        {
            //Debug.Log("moving!!");
            GameController.Instance.useBattery();
        }

          if (!GameController.Instance.getIsManual() &&
             (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            GameController.Instance.onManualBtn();
            return;
        }


        if (GameController.Instance.getIsCharger())
        {
            MoveToCharger();
        }
        else if (GameController.Instance.getIsManual())
        {
            float alpha = GameController.Instance.getProfile(0) > 1 ? 6 : 3;
            float speed = GameController.Instance.getProfile(0) * 0.33f + alpha;
            Vector3 moveDirection = new Vector3(-joystick.Horizontal, 0, -joystick.Vertical).normalized;
            //transform.LookAt(transform.position + moveDirection);
            MoveAgent(moveDirection, speed);
        } else
        {
            MoveToDest();
        }
    }

    void MoveToCharger()
    {
        Vector3 targetPosition = GameController.Instance.getCharger().position;
        Debug.Log("getIsCharger " + targetPosition);
        //transform.LookAt(targetPosition);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 2.0f, NavMesh.AllAreas))
        {
            Debug.Log("hit " + hit.position);
            agent.SetDestination(hit.position);
        }
    }


    void MoveToDest()
    {
        if(GameController.Instance.getNextDust() == null)
        {
            agent.ResetPath();
            return;
        }

        Vector3 targetPosition = GameController.Instance.getNextDust().position;
        //transform.LookAt(targetPosition);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }


    void MoveAgent(Vector3 moveDirection, float moveSpeed)
    {
        if (moveDirection.magnitude > 0.1f) // Add a small deadzone
        {
            //Option 1: Using Move (simple movement)
            agent.Move(moveDirection * moveSpeed * Time.deltaTime);

            //Option 2: Using SetDestination (pathfinding)
            //Vector3 targetPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
            //NavMeshHit hit;
            //if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            //{
            //    agent.SetDestination(hit.position);
            //}
        }
        else
        {
            // Optionally stop the agent if no input
            //agent.SetDestination(transform.position); // Or set velocity to zero.
            agent.velocity = Vector3.zero;
        }

    }

    private bool isMoving()
    {
        if (GameController.Instance.getIsManual() &&
            (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            return true;
        } else if(GameController.Instance.getIsCharger() ||
            !GameController.Instance.getIsManual())
        {
            return agent.velocity.magnitude > 0.1f;
        }
        else
        {
            return false;
        }
    }

    public bool getIsHit()
    {
        return isHit;
    }

    public void hit(int enemyAtk)
    {
        GameController.Instance.playHitRobotAudio();
        isHit = true;
        //GameController.Instance.showToast("현재 위치 파악중입니다.", 2);
        float def = ((float)(1000 - GameController.Instance.getDef()) / (float)1000f);
        float power = enemyAtk * def;
        if(GameController.Instance.getIsNight())
        {
            power = enemyAtk * 2 * def;
        }
        hitText.text = "-" + power;
        hitText.transform.DOShakePosition(0.5f).OnComplete(() => {
            hitText.text = "";
            hitText.transform.localPosition = Vector3.zero;
        });

        GameController.Instance.useBatteryByAttack(power);
        StartCoroutine(malfunctionAndWait());
    }

    IEnumerator malfunctionAndWait()
    {
        batteryFill.color = 
        renderer.material.color = attackedRoboColor;
        batteryFill2.color =
        renderer.material.color = attackedRoboColor;
        updateBatteryView(true);
        yield return new WaitForSeconds(1);
        renderer.material.color = Color.white;
        updateBatteryView(false);
        isHit = false;
    }


    public void speaker()
    {
        StopAllCoroutines();
        GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
        isHit = false;
        catCatcher.speaker();
    }

    public void startWeaponRotate()
    {
        rotateLastTime = Time.time;
        rotater.startRotate();
    }

    public void stopWeaponRotate()
    {
        rotater.stopRotate();
    }

}
