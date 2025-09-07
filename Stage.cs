using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    public Image[] img;

    int stage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    Sequence seq;
    void Start()
    {
        stage = PlayerPrefs.GetInt("stage", 0);
        DataHolder.selStage = stage;
        shakeAnimation(img[stage].gameObject);
        for (int i=0;i<=stage;i++)
        {
            img[i].color = new Color32(255, 255, 255, 255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickStage(int idx)
    {
        if(idx <= stage)
        {
            DataHolder.selStage = idx;
            shakeAnimation(img[idx].gameObject);
        }
    }

    private void shakeAnimation(GameObject obj)
    {
        if(seq != null)
        {
            seq.Kill();
        }
        seq = DOTween.Sequence();
        seq.Append(obj.transform.DOShakeScale(1, 0.1f, 10, 90, true))
            .AppendInterval(1f)
            .Append(transform.DOScale(Vector3.one, 0.2f));
    }

    public void onGameStart()
    {
        SceneManager.LoadScene("Main");
    }

    private void OnDestroy()
    {
        if (seq != null)
        {
            seq.Kill();
        }
    }
}
