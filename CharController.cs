using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CharController : MonoBehaviour
{

    public FloatingJoystick joystick;
    NavMeshAgent agent;

    bool isHit = false;

    CatCatcher catCatcher;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        catCatcher = GetComponent<CatCatcher>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(isHit)
        {
            transform.Rotate(0, 50f * Time.deltaTime, 0);
            return;
        }


        if(isMoving())
        {
            //Debug.Log("moving!!");
            GameController.Instance.useBattery();
        }


        if(GameController.Instance.getIsCharger())
        {
            MoveToCharger();
        }
        else if (GameController.Instance.getIsManual())
        {
            float speed = 7;
            Vector3 moveDirection = new Vector3(-joystick.Horizontal, 0, -joystick.Vertical).normalized;
            transform.LookAt(transform.position + moveDirection);
            MoveAgent(moveDirection, speed);
        } else
        {
            MoveToDest();
        }
    }

    void MoveToCharger()
    {
        Vector3 targetPosition = GameController.Instance.getCharger().position;
        transform.LookAt(targetPosition);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
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
        transform.LookAt(targetPosition);
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

    public void hit()
    {
        isHit = true;
        GameController.Instance.showToast("현재 위치 파악중입니다.", 2);
        StartCoroutine(malfunctionAndWait());
    }

    IEnumerator malfunctionAndWait()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.SetColor("_BaseColor", Color.green);
        yield return new WaitForSeconds(10);
        renderer.material.SetColor("_BaseColor", Color.white);
        isHit = false;
    }


    public void speaker()
    {
        StopAllCoroutines();
        GetComponent<Renderer>().material.SetColor("_BaseColor", Color.white);
        isHit = false;
        catCatcher.speaker();
    }

}
