using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Intro : MonoBehaviour
{
    public Transform startBtn;
    Sequence seq;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        startBtn.DOMoveY(244f, 1).OnComplete(() =>
       shakeAnimation(startBtn.gameObject)
       );

    }

    private void Update()
    {
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        #endif
    }


    public void onGameStart()
    {
        SceneManager.LoadScene("Stage");
    }

    private void shakeAnimation(GameObject obj)
    {if(seq != null)
        {
            seq.Kill();
        }
        
        seq = DOTween.Sequence();
        seq.Append(obj.transform.DOShakeScale(1, 0.1f, 10, 90, true))
            .AppendInterval(1f)
            .Append(transform.DOScale(Vector3.one, 0.2f))
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        if (seq != null)
        {
            seq.Kill();
        }
    }
}
