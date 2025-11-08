using UnityEngine;
using UnityEngine.UI;

public class StageItem : MonoBehaviour
{


    public TMPro.TextMeshProUGUI stageText;
    int idx;
    Stage stageController;

    public GameObject lockState;
    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageController = GameObject.Find("Stage").GetComponent<Stage>();
        //lockState.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void init(int _idx, Sprite sprite, int stage)
    {
        idx = _idx;
        stageText.text = "스테이지" + (idx + 1);
        image.sprite = sprite;
        Debug.Log("stage " + stage + " / " + _idx) ;
        if (stage < _idx)
        {
            Debug.Log("lock");
            lockState.SetActive(true);
        } else
        {
            lockState.SetActive(false);
        }
    } 

    public void onClickItem()
    {
        stageController.onClickStage(idx);
    }

}
