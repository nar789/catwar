using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class DustController : MonoBehaviour
{
    // Start is called before the first frame update

    Sequence seq;
    AudioSource eatAudio;
    bool isCleaned = false;
    float createTime = 0f;

    NavMeshAgent agent;
    Vector3 target = Vector3.zero;

    bool lastNight;

    void Start()
    {
        createTime = Time.time;
        seq = DOTween.Sequence();
        seq.Append(transform.DOShakeScale(1, Random.Range(0.1f, 0.5f), 10, 90, true))
            .AppendInterval(Random.Range(0.1f, 1f))
            .Append(transform.DOScale(new Vector3(0.3f, 1, 0.3f), 0.2f))
            .SetLoops(-1, LoopType.Restart);
        seq.Play();

        eatAudio = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        var x = UnityEngine.Random.Range(-25, 25);
        var z = UnityEngine.Random.Range(-25, 25);
        target = new Vector3(x, 1f, z);

        lastNight = GameController.Instance.getIsNight();

    }

    // Update is called once per frame
    void Update()
    {
        //int limit = GameController.Instance.getAreaLimit();
    
        if(Vector3.Distance(target, transform.position) < 1)
        {
            var x = UnityEngine.Random.Range(-25, 25);
            var z = UnityEngine.Random.Range(-25, 25);
            target = new Vector3(x, 0, z);
        }
        moveToDest(target);

        if(lastNight != GameController.Instance.getIsNight())
        {
            lastNight = GameController.Instance.getIsNight();
            GameController.Instance.generateDust(transform.position);
        }
    }

    void moveToDest(Vector3 dest)
    {
        Vector3 targetPosition = dest;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Time.time - createTime <= 1)
        {
            return;
        }
        if(!isCleaned && other.CompareTag("Robo"))
        {
            isCleaned = true;
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            StopAllCoroutines();
            StartCoroutine(PlayAudioSequence());
        }
    }

    IEnumerator PlayAudioSequence()
    {
        eatAudio.Play();
        yield return new WaitUntil(() => !eatAudio.isPlaying);
        DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if(seq != null)
        {
            seq.Kill();
        }
        GameController.Instance.addSurpriseCat(1);
        GameController.Instance.updateCleanRate(true);
    }
}
