using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ring : MonoBehaviour
{
    GameObject ringImg;
    GameObject ringCanvas;
    GameObject fixedRingImg;
    

    // Start is called before the first frame update
    void Start()
    {
        ringCanvas = GameController.Instance.getRingCanvas();
        ringImg = ringCanvas.transform.GetChild(0).gameObject;
        fixedRingImg = ringCanvas.transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 50f * Time.deltaTime, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robo"))
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            ringImg.transform.position = pos;
            gameObject.SetActive(false);
            ringImg.SetActive(true);
            ringImg.transform.DOMove(fixedRingImg.transform.position, 0.5f).OnComplete(() =>
            {
                ringImg.SetActive(false);
                GameController.Instance.earnGold(50);
                Destroy(gameObject);
            });
        }
    }

}
