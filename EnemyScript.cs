using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyScript : MonoBehaviour
{


    NavMeshAgent agent;
    Transform robo;
    CharController charController;

    bool isAttack = false;

    Animator animator;
    int isAttackHash = 0;


    public Slider slider;
    int hp = 100;

    public enum CMD
    {
        PATROL,
        ATTACK,
    }

    CMD cmd = CMD.PATROL;

    Vector3 patrolTarget = Vector3.zero;

    Color32 attackedColor;

    SpriteRenderer renderer;
    public TMPro.TextMeshProUGUI hitText;

    public GameObject eyeGroup;


    Vector3 leftScale;
    Vector3 rightScale;

    int enemyIdx = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        robo = GameObject.Find("Robo").transform;
        charController = robo.gameObject.GetComponent<CharController>();

        animator = transform.GetChild(0).GetComponent<Animator>();
        isAttackHash = Animator.StringToHash("isAttack");

        slider.value = 100;
        updatePatrolTarget();

        attackedColor = new Color32(255, 155, 155, 255);

        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        hitText.text = "";

        leftScale = transform.GetChild(0).localScale;
        rightScale = new Vector3(-leftScale.x, leftScale.y, leftScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (transform.position.x - robo.position.x > 0)
        {
            transform.GetChild(0).localScale = rightScale;
        } else
        {
            transform.GetChild(0).localScale = leftScale;
        }

        
        if(isAttack)
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath();
            if(eyeGroup != null)
            {
                eyeGroup.SetActive(false);
            }
            
            return;
        }

        if (eyeGroup != null)
        {
            eyeGroup.SetActive(GameController.Instance.getIsNight());
        }

        if(cmd == CMD.ATTACK)
        {
            charController.startWeaponRotate();

            if (Vector3.Distance(transform.position, robo.position) > 4f)
            {
                MoveToDest(robo.position);
            }
            else
            {    
                agent.velocity = Vector3.zero;
                agent.ResetPath();
            }
        } else if(cmd == CMD.PATROL)
        {
            if (Vector3.Distance(transform.position, patrolTarget) > 2f)
            {
                MoveToDest(patrolTarget);
            }
            else
            {
                agent.velocity = Vector3.zero;
                agent.ResetPath();
                updatePatrolTarget();
            }
        }
        
    
    }

    public void setCmd(CMD _cmd)
    {
        cmd = _cmd;
    }

    private void updatePatrolTarget()
    {
        int limit = GameController.Instance.getAreaLimit();
        var x = UnityEngine.Random.Range(-limit, limit);
        var z = UnityEngine.Random.Range(-limit, limit);
        patrolTarget = new Vector3(x, 1f, z);
    }


    void MoveToDest(Vector3 target)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GameController.Instance.isCharging() || GameController.Instance.getIsGameOver())
        {
            return;
        }

        if(other.CompareTag("Weapon"))
        {
            if(isAttack)
            {
                return;
            }
            isAttack = true;

            hitHp();

            StartCoroutine(hitAndAttackEffect());
        }
    }


    IEnumerator hitAndAttackEffect()
    {
        //Debug.Log("attack");
        renderer.color = attackedColor;
        yield return new WaitForSeconds(1f);
        renderer.color = Color.white;

        //Random.Range(0, 100) > X
        // X = (int) (회피율1 / 회피율2 * 100)
        int X = (int)(GameController.Instance.getDex() * 100);
        if(Random.Range(0, 100) > X && cmd == CMD.ATTACK)
        {
            animator.SetBool(isAttackHash, true);
            charController.hit(getEnemyAtk());
            yield return new WaitForSeconds(1f);
            animator.SetBool(isAttackHash, false);
        }

        isAttack = false;
    }

    private void hitHp()
    {
        GameController.Instance.playHitAudio();
        int roboPower = GameController.Instance.getAtk();
        hitText.text = roboPower + "";
        hitText.transform.DOShakePosition(0.5f).OnComplete(() => {
            hitText.text = "";
        });

        hp -= roboPower; 
        if(hp < 0)
        {
            hp = 0; 
        }
        slider.value = hp;
        if(hp == 0)
        {
            die();
        }
    }

    private void die()
    {
        Debug.Log("die!!");
        StopAllCoroutines();
        Destroy(this.gameObject);
        generateDust();
        GameController.Instance.reduceAliveCatCnt();
    }

    private void generateDust()
    {
        GameController.Instance.generateDust(transform.position);
    }

    public void setEnemyIdx(int _idx)
    {
        enemyIdx = _idx;
    }

    private int getEnemyAtk()
    {
        return GameController.Instance.getEnemyAtk(enemyIdx);
    }


}
