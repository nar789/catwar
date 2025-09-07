using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;


    public GameObject[] levelDustGroup;
    public GameObject[] levelObjectsGroup;
    public GameObject ringGroup;
    List<Transform> dust = new List<Transform>();

    public GameObject dustPrefab;
    public GameObject ringPrefab;
    public GameObject ringCanvas;


    int[] dustCnt = { 1, 6, 10, 20, 50, 100, 200, 500, 700, 1 };
    int[] ringCnt = { 3, 1, 2, 4, 10, 20, 40, 100, 140, 1 };
    int[] dustArea = { 10, 10, 15, 20, 25, 30, 35, 40, 45, 50 };

    public GameObject chairPrefab;
    public GameObject dogPrefab;
    public GameObject sharkPrefab;
    public GameObject[] catPrefab;

    public Transform charger;
    public GameObject chargeLed;

    int currentDustIdx = 0;
    GameObject robo;
    CharController charController;
    NavMeshAgent agent;
    bool isManual = true;
    bool isCharger = false;

    float battery = 100f;
    int gold = 0;

    int stage = 9;
    float time = 0;

    public TMPro.TextMeshProUGUI cleanRate;
    public TMPro.TextMeshProUGUI batteryText;
    public TMPro.TextMeshProUGUI stageText;
    public TMPro.TextMeshProUGUI goldText;
    public TMPro.TextMeshProUGUI timeText;

    public GameObject[] panels;
    ScorePanel scorePanel;
    StatPanel statPanel;

    int[] profile = { 1, 1, 1};
    bool buySpeaker = false;
    public GameObject speakerBtn;
    public GameObject speakerObj;
    public GameObject speakerItem;

    public GameObject toastBack;
    public GameObject[] toast;
    public TMPro.TextMeshProUGUI[] toastText;
    Sequence toastSeq;
    Sequence winSeq;
    Sequence goldSeq;
    Sequence cleanRateSeq;

    public AudioSource[] audio;

    public Slider batterySlider;


    int surpriseCat = 0;




    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else if(Instance !=this)
        {
            Destroy(gameObject);
        }       
    }


    void Start()
    {

        stage = DataHolder.selStage;
        Debug.Log("stage " + stage);

        profile[0] = PlayerPrefs.GetInt("speed", 1);
        profile[1] = PlayerPrefs.GetInt("charge", 1);
        profile[2] = PlayerPrefs.GetInt("battery", 1);
        gold = PlayerPrefs.GetInt("gold", 0);
        buySpeaker = PlayerPrefs.GetInt("buySpeaker", 0) == 1;
        updateSkillBtn();


        robo = GameObject.Find("Robo");
        charController = robo.GetComponent<CharController>();
        agent = robo.GetComponent<NavMeshAgent>();
        scorePanel = panels[3].GetComponent<ScorePanel>();
        statPanel = panels[0].GetComponent<StatPanel>();
        updateDust();
        updateCleanRate(false);
        updateStage();
        updateGoldText();
        levelUpAgentSpeed();

        if (stage == 0)
        {
            showToast("귀여운 포자들을 \n 모조리 청소해보세요", 1);
        }

        audio[UnityEngine.Random.Range(0, audio.Length)].Play();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        updateTimeText();
    }

    public void onCleanBtn()
    {
        Debug.Log("on clean btn.");
        agent.ResetPath();
        isManual = false;
        isCharger = false;
        showToast("자동청소", 1);
    }


    public Transform getNextDust()
    {
        if (dust.Count == 0)
        {
            return null;
        }
        while (dust[currentDustIdx] == null && currentDustIdx + 1 < dust.Count)
        {
            currentDustIdx += 1;
            agent.velocity = Vector3.zero;
        }
        return dust[currentDustIdx];
    }

    public void onManualBtn()
    {
        agent.ResetPath();
        isManual = true;
        isCharger = false;
        showToast("수동청소", 1);
    }

    public bool getIsManual()
    {
        return isManual;
    }

    public Transform getCharger()
    {
        return charger;
    }

    public void onChargeBtn()
    {
        isCharger = true;
        showToast("충전기로 돌아가유", 0);
    }

    public bool getIsCharger()
    {
        float dist = Vector3.Distance(robo.transform.position, charger.position);
        if (dist < 2f)
        {
            chargeLed.SetActive(true);
            updateCharging();
        } else
        {
            chargeLed.SetActive(false);
        }
        return isCharger;
    }

    public void updateCharging()
    {
        //isCharger = false;

        //charging...

        Debug.Log("is charging...");
        if (battery < 100f)
        {   
            float weight = profile[1];
            battery += Time.deltaTime * weight;
            if (battery > 100f)
            {
                battery = 100f;
            }
            batteryText.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
        }
    }


    public void updateCleanRate(bool force)
    {
        if(cleanRate == null)
        {
            return;
        }

        int removed = 0;
        for (int i = 0; i < dust.Count; i++)
        {
            if (dust[i] == null)
            {
                removed += 1;
            }
        }
        if (force)
        {
            removed += 1;
        }

        float rate = (float)removed / (float)dust.Count * 100f;
        string r = Mathf.Floor(rate).ToString();
        if (rate % 1 != 0)
        {
            r = rate.ToString("f2");
        }
        if (dust.Count == 0)
        {
            r = "0";
        }
        cleanRate.text = r + "%";


        if (rate == 100)
        {
            win();
        }

        if(force)
        {
            shakeAnimation(cleanRate.gameObject, cleanRateSeq);
        }

    }


    public void win()
    {
        Debug.Log("game win");
        showToast("스테이지 " + (stage + 1) + " 클리어", 0);
        int maxStage = 10;
        if (stage + 1 == maxStage)
        {
            //ending.
            Debug.Log("ending!");
            battery = 100f;
            isCharger = false;
            isManual = true;
            agent.ResetPath();
            agent.gameObject.transform.position = new Vector3(0, 0.5f, 0);
            cleanDustAndRings();
            stage = 0;
            time = 0;
            updateStage();
            updateDust();
            updateCleanRate(false);
            openPanel(4);
        } else
        {
            //tests
            scorePanel.setTime(time, dustCnt[stage]);
            scorePanel.setCat(surpriseCat, stage);
            openPanel(3);
        }


    }

    public void goToHome()
    {
        DOTween.Clear();
        Time.timeScale = 1;
        if (toastSeq != null)
        {
            toastSeq.Kill();
            toastSeq = null;
        }
        if(winSeq != null)
        {
            winSeq.Kill();
            winSeq = null;
        }
        if(goldSeq != null)
        {
            goldSeq.Kill();
            goldSeq = null;
        }
        if(cleanRateSeq != null)
        {
            cleanRateSeq.Kill();
            cleanRateSeq = null;
        }
    }

    public void gameOver()
    {
        Debug.Log("game over");
        showToast("게임오버 \n 청소기 사망", 2);
        time = 0;
        updateTimeText();
        battery = 100f;
        batteryText.text = battery.ToString("f2") + "%";
        batterySlider.value = battery;
        isCharger = false;
        isManual = true;
        agent.ResetPath();
        agent.gameObject.transform.position = new Vector3(0, 0.5f, 0);
        cleanDustAndRings();
        updateStage();
        updateDust();
        updateCleanRate(false);

        //gold = 0;

    }


    public void useBattery()
    {
        float weight = 2.2f - (0.2f * profile[2]);
        battery -= Time.deltaTime * weight;
        if((battery <= 15f && battery > 14.4f) || (battery <= 5f && battery > 4.4f))
        {
            showToast("배터리가 없습니다. \n 충전기로 돌아가세요.", 2); 
        }

        if (battery <= 0)
        {
            battery = 0;
            batteryText.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
            gameOver();
            return;
        } else
        {
            batteryText.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
        }

    }

    private void updateStage()
    {
        stageText.text = "" + (stage + 1);
    }

    private void cleanDustAndRings()
    {
        for (int i = 0; i < levelDustGroup[stage].transform.childCount; i++)
        {
            DestroyImmediate(levelDustGroup[stage].transform.GetChild(i).gameObject);
        }

        for(int i=0;i<ringGroup.transform.childCount;i++)
        {
            DestroyImmediate(ringGroup.transform.GetChild(i).gameObject);
        }
    }

    public int getAreaLimit()
    {
        return stage < dustArea.Length ? dustArea[stage] : 50;
    }

    private void updateDust()
    {
        currentDustIdx = 0;
        if(stage - 1 >= 0)
        {
            levelObjectsGroup[stage - 1].SetActive(false);
        }
        levelObjectsGroup[stage].SetActive(true);
        levelDustGroup[stage].SetActive(true);
        dust.Clear();

        for (int i = 0; i < dustCnt[stage]; i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = UnityEngine.Random.Range(-limit, limit);
            var z = UnityEngine.Random.Range(-limit, limit);
            var pos = new Vector3(x, 0.5f, z);
            Instantiate(dustPrefab, pos, Quaternion.Euler(new Vector3(-60f, 0, 0)), levelDustGroup[stage].transform);
        }

        for (int i = 0; i < ringCnt[stage]; i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = UnityEngine.Random.Range(-limit, limit);
            var z = UnityEngine.Random.Range(-limit, limit);
            var pos = new Vector3(x, 0.6f, z);
            Instantiate(ringPrefab, pos, Quaternion.Euler(new Vector3(-20f, -45f, 0)), ringGroup.transform);
        }

        for(int i=0;i<stage + 1;i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = UnityEngine.Random.Range(-limit, limit);
            var z = UnityEngine.Random.Range(-limit, limit);
            var pos = new Vector3(x, 2f, z);
            Instantiate(chairPrefab, pos, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-90, 90), 0)), levelObjectsGroup[stage].transform);
        }

        for (int i = 0; i < stage + 1; i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = UnityEngine.Random.Range(-limit, limit);
            var z = UnityEngine.Random.Range(-limit, limit);
            var pos = new Vector3(x, 1f, z);
            Instantiate(dogPrefab, pos, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-90, 90), 0)), levelObjectsGroup[stage].transform);
        }

        for (int i = 0; i < stage + 1; i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = 0;
            do
            {
                x = UnityEngine.Random.Range(-limit, limit);
            } while (x < 5 && x > -5);
            var z = 0;
            do
            {
                z = UnityEngine.Random.Range(-limit, limit);
            } while (z < 5 && z > -5);
            var pos = new Vector3(x, 0, z);
            int catIdx = UnityEngine.Random.Range(0, catPrefab.Length);
            Instantiate(catPrefab[catIdx], pos, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-90, 90), 0)), levelObjectsGroup[stage].transform);
        }


        for (int i = 0; i < stage + 1; i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = UnityEngine.Random.Range(-limit, limit);
            var z = UnityEngine.Random.Range(-limit, limit);
            var pos = new Vector3(x, 2.5f, z);
            Instantiate(sharkPrefab, pos, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-90, 90), 0)), levelObjectsGroup[stage].transform);
        }



        for (int i = 0; i < levelDustGroup[stage].transform.childCount; i++)
        {
            dust.Add(levelDustGroup[stage].transform.GetChild(i));
        }
        var comparison = new Comparison<Transform>(CompareIntMethod);
        dust.Sort(comparison);
    }

    private static int CompareIntMethod(Transform a, Transform b)
    {
        float aa = Vector3.Distance(a.position, Vector3.zero);
        float bb = Vector3.Distance(b.position, Vector3.zero);
        return aa.CompareTo(bb);
    }

    public void nextStage()
    {
        Debug.Log("next stage.");
        closeAllPanel();
        isCharger = false;
        isManual = true;
        agent.ResetPath();
        agent.gameObject.transform.position = new Vector3(0, 0.5f, 0);
        cleanDustAndRings();
        stage += 1;
        time = 0;
        updateStage();
        updateDust();
        updateCleanRate(false);
        showToast("스테이지 " + (stage + 1) + " 청소시작", 0);
        if(PlayerPrefs.GetInt("stage", 0) < stage)
        {
            PlayerPrefs.SetInt("stage", stage);
        }

        for(int i = 0; i < audio.Length; i++)
        {
            audio[i].Stop();
        }
        audio[UnityEngine.Random.Range(0, audio.Length)].Play();

    }

    public void closeAllPanel()
    {
        Time.timeScale = 1f;
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null && panels[i].transform.childCount > 1)
            {
                panels[i].transform.GetChild(1).DOLocalMoveY(-500f, 0.1f);
                panels[i].SetActive(false);
            }
        }

        if (winSeq != null)
        {
            winSeq.Kill();
        }

    }

    public void openPanel(int idx)
    {
        closeAllPanel();
        Time.timeScale = 0f;
        panels[idx].SetActive(true);
        if(idx == 2)
        {
            speakerItem.SetActive(!buySpeaker);
        }

        if(idx == 0)
        {
            statPanel.updateGoldText();
            statPanel.updateProfile();
            statPanel.updatePrice();
        }
        panels[idx].transform.GetChild(1).DOLocalMoveY(0, 1).SetUpdate(true);        

        if(idx >= 3)
        {
            var win = panels[idx].transform.GetChild(1).GetChild(0);
            if(winSeq != null)
            {
                winSeq.Kill();
            }
            winSeq = DOTween.Sequence();
            winSeq.Append(
                win.DOScale(1f, 0.2f))                
                .Append(win.DOScale(1.5f, 0.2f))
                .SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                
        }

        if(idx == 4)
        {
            var comment = panels[idx].transform.GetChild(1).GetChild(1).gameObject.GetComponent<CanvasGroup>();
            comment.alpha = 0;
            comment.DOFade(1, 5).SetUpdate(true);
        }
    }

    private void updateGoldText()
    {
        goldText.text = $"{gold:N0}G";
    }

    private void shakeAnimation(GameObject obj, Sequence seq)
    {
        if(seq != null)
        {
            seq.Kill();
        }
        obj.transform.DOScale(Vector3.one, 0.1f);
        seq = DOTween.Sequence();
        seq.Append(obj.transform.DOShakeScale(1, 0.1f, 10, 90, true))
            .AppendInterval(0.5f)
            .Append(transform.DOScale(Vector3.one, 0.2f));
    }


    private void updateTimeText()
    {
        string sec = (time % 60).ToString("f2");
        if(sec.Length <= 4)
        {
            sec = "0" + sec;
        }
        string mm = Mathf.FloorToInt((time / 60) % 60).ToString("D2");
        string hh = Mathf.FloorToInt(time / 60 / 60).ToString("D2");
        timeText.text = hh + ":" + mm + ":" + sec;
    }

    public int getGold()
    {
        return gold;
    }

    public void useGold(int use)
    {
        gold -= use;
        PlayerPrefs.SetInt("gold", gold);
        updateGoldText();
    }

    public void earnGold(int earn)
    {
        gold += earn;
        PlayerPrefs.SetInt("gold", gold);
        updateGoldText();
        shakeAnimation(goldText.gameObject, goldSeq);
    }

    public int getProfile(int idx)
    {
        return profile[idx];
    }

    public bool levelUpProfile(int idx)
    {
        if (profile[idx] + 1 <= 10)
        {
            profile[idx] += 1;
            if(idx ==0 )
            {
                PlayerPrefs.SetInt("speed", profile[0]);
            } else if(idx == 1)
            {
                PlayerPrefs.SetInt("charge", profile[1]);
            } else if(idx == 2)
            {
                PlayerPrefs.SetInt("battery", profile[2]);
            }
            return true;

        } else
        {
            return false;
        }
    }

    public void levelUpAgentSpeed()
    {
        agent.speed = profile[0] * 5;
        agent.acceleration = profile[0] * 5;
        Debug.Log("agent speed " + agent.speed + " / acc " + agent.acceleration);
    }

    public void showToast(string text, int idx)
    {
        if(toastBack == null)
        {
            return;
        }
        toastBack.SetActive(true);

        var cg = toastBack.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        for(int i=0;i<toast.Length;i++)
        {
            toast[i].SetActive(false);
        }
        toast[idx].SetActive(true);
        toastText[idx].text = text;

        if(toastSeq != null)
        {
            toastSeq.Kill();
        }

        toastSeq = DOTween.Sequence();
        toastSeq.Append(cg.DOFade(1, 1f).SetUpdate(true))
            .AppendInterval(2f)
            .Append(cg.DOFade(0, 1f).SetUpdate(true))
            .OnComplete(() =>
            {
                toastBack.gameObject.SetActive(false);
            }).SetUpdate(true);

    }

    public void onUpdateCameraZoom(int idx)
    {
        int sz = 15;
        if (idx == 0)
        {
            sz = 10;
        }
        else if (idx == 2)
        {
            if (Camera.main.orthographicSize == 30)
            {
                sz = 40;
            }
            else
            {
                sz = 30;
            }
        }
        Camera.main.orthographicSize = sz;
    }

    private void OnDestroy()
    {
        goToHome();
    }

    public GameObject getRingCanvas()
    {
        return ringCanvas;
    }

    public void onSpeakerBtn()
    {
        charController.speaker();
    }

    public void addSurpriseCat(int cnt)
    {
        surpriseCat += cnt;
    }

    public bool getIsBuySpeaker()
    {
        return buySpeaker;
    }

    public void buySpeakerByStore()
    {
        buySpeaker = true;
        PlayerPrefs.SetInt("buySpeaker", 1);
        updateSkillBtn();
    }

    void updateSkillBtn()
    {
        speakerBtn.SetActive(buySpeaker);
        speakerObj.SetActive(buySpeaker);
    }

    public float getTime()
    {
        return time;
    }
}
