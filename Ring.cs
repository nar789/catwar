using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ring : MonoBehaviour
{
    GameObject ringImg;
    GameObject ringCanvas;
    GameObject fixedRingImg;

    AudioSource goldAudio;
    bool isEat = false;
    float createTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        createTime = Time.time;
        ringCanvas = GameController.Instance.getRingCanvas();
        ringImg = ringCanvas.transform.GetChild(0).gameObject;
        fixedRingImg = ringCanvas.transform.GetChild(1).gameObject;

        goldAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 50f * Time.deltaTime, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(Time.time - createTime <= 1)
        {
            return;
        }
        if (!isEat && other.CompareTag("Robo"))
        {
            //goldAudio.Play();
            isEat = true;
            StopAllCoroutines();
            StartCoroutine(PlayAudioSequence());
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            ringImg.transform.position = pos;
            transform.position = new Vector3(transform.position.x, -2f, transform.position.z);
            ringImg.SetActive(true);
            ringImg.transform.DOMove(fixedRingImg.transform.position, 0.5f).OnComplete(() =>
            {
                ringImg.SetActive(false);
                GameController.Instance.earnGold(50);
                GameController.Instance.updateStageGold(50);
            });
        }
    }


    IEnumerator PlayAudioSequence()
    {
        goldAudio.Play();
        yield return new WaitUntil(() => !goldAudio.isPlaying && !ringImg.activeSelf);
        //Destroy(gameObject);
    }


}
