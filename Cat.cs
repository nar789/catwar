using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cat : MonoBehaviour
{

    Animator animator;
    List<int> hashList = new List<int>();
    NavMeshAgent agent;
    float patrolTime = 10;
    float restTime = 10;
    float curTime = 10;

    enum CMD
    {
        PATROL,
        MOVING,
        REST,
        RESTING,
        ATTACK,
        HIT,
    }
    CMD cmd = CMD.PATROL;
    Vector3 target;


    AudioSource catAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();

        hashList.Add(Animator.StringToHash("isWalk"));
        hashList.Add(Animator.StringToHash("isWow"));
        hashList.Add(Animator.StringToHash("isHit"));
        hashList.Add(Animator.StringToHash("isAttack"));
        hashList.Add(Animator.StringToHash("isAgro"));
        hashList.Add(Animator.StringToHash("isStretch"));
        hashList.Add(Animator.StringToHash("isClean"));

        agent = GetComponent<NavMeshAgent>();
        catAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cmd == CMD.PATROL)
        {
            patrol();
            cmd = CMD.MOVING;
        } else if(cmd == CMD.MOVING)
        {
            bool isMoving = agent.remainingDistance > 0.1f;
            if (isMoving)
            {
                curTime -= Time.deltaTime;
                if(curTime <= 0)
                {
                    agent.ResetPath();
                    cmd = CMD.REST;
                    curTime = restTime;
                }
            } else
            {
                agent.ResetPath();
                cmd = CMD.REST;
                curTime = restTime;
            }
        } else if(cmd == CMD.REST)
        {
            rest();
            cmd = CMD.RESTING;
            
        } else if(cmd == CMD.RESTING)
        {
            curTime -= Time.deltaTime;
            if (curTime <= 0)
            {
                cmd = CMD.PATROL;
                curTime = patrolTime;
            }
        }

        //Debug.Log("cmd " + cmd + " / " + curTime);
    }

    void patrol()
    {
        int limit = GameController.Instance.getAreaLimit();
        var x = UnityEngine.Random.Range(-limit, limit);
        var z = UnityEngine.Random.Range(-limit, limit);
        target = new Vector3(x, 0.5f, z);
        moveToDest(target);
        walk();
    }

    void moveToDest(Vector3 dest)
    {
        Vector3 targetPosition = dest;
        transform.LookAt(targetPosition);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }


    void walk()
    {
        clearAnimation();
        animator.SetBool(hashList[0], true);
    }


    void wow()
    {
        clearAnimation();
        animator.SetBool(hashList[1], true);
    }

    void hit()
    {
        clearAnimation();
        animator.SetBool(hashList[2], true);
    }


    void attack()
    {
        clearAnimation();
        animator.SetBool(hashList[3], true);
    }


    void rest()
    {
        clearAnimation();
        int idx = Random.Range(4, 7);
        animator.SetBool(hashList[idx], true);
    }

    void clearAnimation()
    {
        foreach(int item in hashList)
        {
            animator.SetBool(item, false);
        }
    }

    public void attackRobo(Vector3 pos, CharController robo)
    {
        cmd = CMD.ATTACK;
        agent.ResetPath();
        clearAnimation();
        transform.LookAt(pos);
        catAudio.Play();
        attack();
        robo.hit(2);
        transform.LookAt(pos);
        StartCoroutine(patroAfterWait(2));
    }

    IEnumerator patroAfterWait(float sec)
    {
        yield return new WaitForSeconds(sec);
        clearAnimation();
        cmd = CMD.PATROL;
    }

    public void hitBySpeaker()
    {
        cmd = CMD.HIT;
        StopAllCoroutines();
        agent.ResetPath();
        hit();
        StartCoroutine(patroAfterWait(5));
    }

    public bool isHitCmd()
    {
        return cmd == CMD.HIT;
    }
}
