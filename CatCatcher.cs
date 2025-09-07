using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCatcher : MonoBehaviour
{


    public float radius = 0f;
    public LayerMask layer;

    public Collider[] colliders;

    public ParticleSystem[] fx;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void speaker()
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layer);

        if (colliders.Length > 0)
        {
            foreach (Collider col in colliders)
            {
                Cat cat = col.GetComponent<Cat>();
                cat.hitBySpeaker();
            }
            GameController.Instance.addSurpriseCat(colliders.Length);
            GameController.Instance.showToast("스피커가 송출되었습니다", 1);
        } else
        {
            GameController.Instance.showToast("주변에 냥이가 없습니다.\n더 가까이 다가가세요.", 2);
        }

        startFx();
    }

    void startFx()
    {
        StopAllCoroutines();
        for(int i=0;i<fx.Length;i++)
        {
            fx[i].gameObject.SetActive(true);
            fx[i].Stop();
            fx[i].Clear();
            fx[i].Play();
        }

        StartCoroutine(clearFx());
    }

    IEnumerator clearFx()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < fx.Length; i++)
        {
            fx[i].Stop();
            fx[i].Clear();
            fx[i].gameObject.SetActive(false);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
