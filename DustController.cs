using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DustController : MonoBehaviour
{
    // Start is called before the first frame update

    Sequence seq;

    void Start()
    {
        seq = DOTween.Sequence();
        seq.Append(transform.DOShakeScale(1, Random.Range(10, 50), 10, 90, true))
            .AppendInterval(Random.Range(0.1f, 1f))
            .Append(transform.DOScale(new Vector3(250, 250,250), 0.2f))
            .SetLoops(-1, LoopType.Restart);
        seq.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Robo"))
        {
            Destroy(gameObject);       
        }
    }

    private void OnDestroy()
    {
        if(seq != null)
        {
            seq.Kill();
        }
        GameController.Instance.updateCleanRate(true);
    }
}
