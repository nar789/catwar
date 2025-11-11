using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Stage : MonoBehaviour
{
    public Image[] img;

    public Sprite[] itemBacks;

    int stage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    Sequence seq;

    public GameObject startPanel;
    public TMPro.TextMeshProUGUI startPanelText;

    public GameObject scrollViewContainer;
    public GameObject stageItemPrefab;

    public TMPro.TextMeshProUGUI roomNameText;

    bool isFirst = true;


    private string[] roomName = {"거실", "화장실", "작은방",
    "게임방", "우주선", "외계행성",
    "할로윈", "마법학교", "크리스마스","로청이공장"};


    void Start()
    {
        stage = PlayerPrefs.GetInt("stage", 0);
        
        //test
        //stage = 29;

        DataHolder.selStage = stage;
        roomNameText.text = roomName[stage / 3];
    }

    private void generateStageItems()
    {
        for(int i=0;i<30;i++)
        {
            GameObject obj = Instantiate(stageItemPrefab, scrollViewContainer.transform);
            var stageItem = obj.GetComponent<StageItem>();
            stageItem.init(i, getBackSprite(i), stage);
        }
        
    }


    // Update is called once per frame
    void Update()
    {

        if(isFirst)
        {
            isFirst = false;
            generateStageItems();
        }
        
        
        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(startPanel.activeSelf)
            {
                closeStartPanel();
                return;
            }
            SceneManager.LoadScene("Intro");
        }
        #endif
    }

    public void onClickStage(int idx)
    {
        if (idx <= stage)
        {
            DataHolder.selStage = idx;
            roomNameText.text = roomName[idx / 3];
            openStartPanel();
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

    public void openStartPanel()
    {
        startPanelText.text = "스테이지" + (DataHolder.selStage + 1);
        startPanel.SetActive(true);   
        startPanel.transform.GetChild(1).DOLocalMoveY(0, 1);
    }

    public void closeStartPanel()
    {
        startPanel.SetActive(false);
        startPanel.transform.GetChild(1).DOLocalMoveY(-500, 0.1f);
    }

    public Sprite getBackSprite(int _idx)
    {
        return itemBacks[_idx / 3];
    }
}
