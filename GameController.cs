using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    int[] ringCnt = { 
        3, 6, 9,
        10, 10, 20,
        40, 100, 140,
        40, 100, 140,
        40, 100, 140,
        40, 100, 140,
        40, 100, 140,
        40, 100, 140,
        40, 100, 140,
        40, 100, 140};

    int[] dustArea = { 
        15, 20, 25,
        15, 20, 25,
        15, 20, 25,
        15, 20, 25,
        15, 20, 25,
        15, 20, 25,
        15, 20, 25,
        15, 20, 25,
        15, 20, 25,
        15, 20, 25
    };

    int[] catCnt = { 
        5, 7, 10,
        5, 7, 10,
        5, 7, 10, 
        5, 7, 10,
        7, 10, 20,
        7, 10, 20,
        7, 10, 20,
        7, 10, 20,
        7, 10, 20,
        7, 10, 20
    };

    int[] catMultiply =
    {
        2, 3, 3,
        3, 3, 3,
        3, 3, 3,
        3, 3, 3,
        5, 5, 5,
        5, 5, 5,
        5, 5, 5,
        5, 5, 5,
        5, 5, 5,
        5, 5, 5
    };
    int currentCatMultiply = 5;

    int aliveCatCnt = 0;

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

    int stage = 0;
    float time = 30;
    float totalTime = 0;
    int stageGold = 0;

    public TMPro.TextMeshProUGUI cleanRate;
    public TMPro.TextMeshProUGUI cleanRate2;
    public TMPro.TextMeshProUGUI batteryText;
    public TMPro.TextMeshProUGUI batteryText2;
    public TMPro.TextMeshProUGUI stageText;
    public TMPro.TextMeshProUGUI goldText;
    public TMPro.TextMeshProUGUI timeText;

    public GameObject[] panels;
    ScorePanel scorePanel;
    StatPanel statPanel;
    StoragePanel storagePanel;
    ShopPanel shopPanel;
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
    Sequence cleanRateNotiferSeq;

    public AudioSource[] audio;

    public Slider batterySlider;
    public Slider batterySlider2;


    int surpriseCat = 0;


    public AudioSource clickAudio;
    public AudioSource winAudio;
    public AudioSource hitAudio;
    public AudioSource hitRobotAudio;

    public GameObject[] blockBuyingBtn;


    int lastAudioIdx = -1;

    bool isPause = false;

    //packs start
    bool isPowerCharger = false;
    //packs end

    CameraShake cameraShake;
    public CanvasGroup darkFade;
    bool isGameOver = false;
    bool isNight = false;
    public Light light;


    public Sprite[] weaponSprites;
    public Sprite[] skinSprites;

    int skinIdx = 0;
    int weaponIdx = 0;


    int[] atk = { 10,11,13,14,16,   17, 19, 20, 22, 23,  25,26,28,29,31,  32,34,35,37,38,  40, 41,43,44, 46,  47,49};
    int[] def = {300,322,344,366,388,  411,433,455,477,499,  522,544,566,588,610,  633,655,677,699,721,  744,766,788,810,832,  855,877 };
    int[] dex1 = { 60,64,68,72,76,  80,84,88,92,96,  100,104,108,112,116,  120,124,128,132,136,  140,144,148,152,156,  160,170};
    int[] dex2 = { 180,181,181,182,183,  184,184,185,186,187,  188,188,189,190,191,   192,192,193,194,195,   196,196,197,198,199,  200,200};
    int[] enemyAtk = { 2, 3,4,2,3,  4,2,3,4,2,    3,4,2,3,3,   4,3,4,4,4,  4,4,4,4,4,  5,5,5};

    int[] skinDia = { 0, 50, 100, 150, 200,   250, 300, 350, 400, 450,    500, 550, 600, 650, 700,    750, 800, 850, 900, 950,   1000, 1050, 1100, 1150, 1200,   1250, 1300};
    int[] weaponDia = { 0, 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900, 950, 1000, 1050, 1100, 1150, 1200, 1250, 1300 };


    string[] skinName = { "기본파랭이", "야구공", "곰도리", "도도한꿀벌", "견고한꿀벌", "체리케이크", "냥냥이", "치즈케이크", "태엽이", "공사장",
    "만두한판", "안드로이드", "바둑이", "도넛츠", "덕덕이", "코가손", "활활이", "갸꾸리", "버거왕", "호기심많은키위",
    "LP판", "팝코니", "에일리언", "라이징스타", "딸기딸기", "용감한유니콘", "초월한나무"};

    string[] weaponName = {"기본뽕뿅이", "꿀펀치", "붕어빵", "단검", "돌도끼", "톱날", "양손도끼", "날카로운낫", "장검", "아이스바",
    "거대톱날", "폭탄", "축복받은검", "마법지팡이", "할로윈펌킨", "할로윈낫", "할로윈지팡이", "할로윈봉", "마력단검", "전설의장검",
    "전설의도끼", "전설의망치", "전설의단검", "용신검", "용신도끼", "용신망치", "용신봉"};



    int dia = 0;
    public TMPro.TextMeshProUGUI diaText;

    public CanvasGroup cleanRateNotifier;


    public class AssetData
    {
        public List<Vector2> assetList;
    }

    List<Vector2> myAssetList = new List<Vector2>();


    public AdmobManager admobManager;
    int isNoAd = 0;


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
        //stage = 30;
        Debug.Log("stage " + stage);

        
        levelObjectsGroup[0].SetActive(false);


        //test        
        /*
        PlayerPrefs.SetInt("speed", 1);
        PlayerPrefs.SetInt("charge", 1);
        PlayerPrefs.SetInt("battery", 1);
        PlayerPrefs.SetInt("gold", 500);
        PlayerPrefs.SetInt("dia", 0);
        PlayerPrefs.DeleteKey("myAsset");
        PlayerPrefs.SetInt("skin", 0);
        PlayerPrefs.SetInt("weapon", 0);*/

        //test2
        /*
        PlayerPrefs.SetInt("gold", 1000);
        PlayerPrefs.SetInt("dia", 12000);
        PlayerPrefs.SetInt("speed", 30);
        PlayerPrefs.SetInt("charge", 30);
        PlayerPrefs.SetInt("battery", 30);
        PlayerPrefs.SetInt("isNoAd", 0);
        */

        isNoAd = PlayerPrefs.GetInt("isNoAd", 0);
        profile[0] = PlayerPrefs.GetInt("speed", 7);
        profile[1] = PlayerPrefs.GetInt("charge", 7);
        profile[2] = PlayerPrefs.GetInt("battery", 7);
        gold = PlayerPrefs.GetInt("gold", 100);
        buySpeaker = PlayerPrefs.GetInt("buySpeaker", 0) == 1;
        updateSkillBtn();

        skinIdx = PlayerPrefs.GetInt("skin", 0);
        weaponIdx = PlayerPrefs.GetInt("weapon", 0);

        myAssetList.Add(new Vector2(1, 0));
        myAssetList.Add(new Vector2(2, 0));

        
        if(PlayerPrefs.HasKey("myAsset"))
        {
            string json = PlayerPrefs.GetString("myAsset");
            AssetData data = JsonUtility.FromJson<AssetData>(json);
            myAssetList = data.assetList;
        }

        dia = PlayerPrefs.GetInt("dia", 0);


        robo = GameObject.Find("Robo");
        charController = robo.GetComponent<CharController>();
        agent = robo.GetComponent<NavMeshAgent>();
        scorePanel = panels[3].GetComponent<ScorePanel>();
        statPanel = panels[0].GetComponent<StatPanel>();
        storagePanel = panels[1].GetComponent<StoragePanel>();
        shopPanel = panels[2].GetComponent<ShopPanel>();
        updateDust();
        updateCleanRate(false);
        updateStage();
        updateGoldText();
        updateDiaText();
        levelUpAgentSpeed();

        if (stage == 0)
        {
            showToast("스탯을 먼저 올려보세요.", 1);
        }

        lastAudioIdx = UnityEngine.Random.Range(0, audio.Length);
        audio[lastAudioIdx].Play();

        cameraShake = Camera.main.GetComponent<CameraShake>();
        darkFade.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        totalTime += Time.deltaTime;
        if(time < 1)
        {
            time = 30;
            changeDayNight();
        }
        updateTimeText();


        #if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if(panels[0].activeSelf || panels[1].activeSelf || panels[2].activeSelf || panels[5].activeSelf)
            {
                closeAllPanel();
                return;
            }

            if (panels[3].activeSelf || panels[4].activeSelf)
            {
                return;
            }

            openPanel(5);
        }
        #endif
    
    }

    public void onCleanBtn()
    {
        clickAudio.Play();
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
        clickAudio.Play();
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
        clickAudio.Play();
        isManual = true;
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
            isCharger = false;
            agent.ResetPath();
        } else
        {
            chargeLed.SetActive(false);
        }
        return isCharger;
    }

    public bool isCharging()
    {
        return Vector3.Distance(robo.transform.position, charger.position) < 2f;
    }

    public void updateCharging()
    {
        //isCharger = false;

        //charging...

        Debug.Log("is charging...");
        if (battery < 100f)
        {
            float weight = 30 + profile[1] * 0.33f;

            battery += Time.deltaTime * weight;
            if (battery > 10f && Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
            }
            if (battery > 100f)
            {
                battery = 100f;
            }
            batteryText.text = battery.ToString("f2") + "%";
            batteryText2.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
            batterySlider2.value = battery;
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

        //float rate = (float)removed / (float)dust.Count * 100f;
        float rate = ((float)removed / (float)dust.Count) * 100f;
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
        cleanRate2.text = r + "%";

        Debug.Log("rate " + rate + " / alive cat " + aliveCatCnt); 
        if (aliveCatCnt == 0 && currentCatMultiply == 0 && rate == 100 && time != 0 && catCnt[stage] <= dust.Count) //time 0 means game over 
        {
            if(cleanRateNotifier.gameObject.activeSelf)
            {
                cleanRateNotifier.alpha = 0;
                cleanRateNotifier.gameObject.SetActive(false);
            }
            win();
        } else if(force)
        {
            updateCleanRateNotifier();
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
        winAudio.Play();
        int maxStage = 30;
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
            stageGold = 0;
            stage = 0;
            time = 0;
            totalTime = 0;
            updateStage();
            updateDust();
            updateCleanRate(false);
            openPanel(4);
        } else
        {
            scorePanel.setTime(totalTime, stageGold);
            scorePanel.setCat(surpriseCat);
            openPanel(3);
        }


    }

    public void goToHome()
    {
        SceneManager.LoadScene("Stage");
    }

    private void closeAllKillSeq()
    {
        if (clickAudio != null)
        {
            clickAudio.Play();
        }
        DOTween.Clear();
        Time.timeScale = 1;
        if (toastSeq != null)
        {
            toastSeq.Kill();
            toastSeq = null;
        }
        if (winSeq != null)
        {
            winSeq.Kill();
            winSeq = null;
        }
        if (goldSeq != null)
        {
            goldSeq.Kill();
            goldSeq = null;
        }
        if (cleanRateSeq != null)
        {
            cleanRateSeq.Kill();
            cleanRateSeq = null;
        }
        if(cleanRateNotiferSeq != null)
        {
            cleanRateNotiferSeq.Kill();
            cleanRateNotiferSeq = null;
        }
    }

    public void gameOver()
    {
        Debug.Log("game over");
        isGameOver = true;
        shakeCamera();
        showToast("게임오버 \n 청소기 사망", 2);
        time = 0;
        totalTime = 0;
        stageGold = 0;
        updateTimeText();
        battery = 25f;
        batteryText.text = battery.ToString("f2") + "%";
        batteryText2.text = battery.ToString("f2") + "%";
        batterySlider.value = battery;
        batterySlider2.value = battery;
        isCharger = false;
        isManual = true;
        agent.ResetPath();
        agent.gameObject.transform.position = new Vector3(0, 0.5f, 0);
        cleanDustAndRings();
        updateStage();
        updateDust();
        updateCleanRate(false);
        fadeOut();
        //gold = 0;

    }

    public void useBatteryByAttack(float power)
    {
        battery -= power;
        if ((battery <= 15f && battery > 14.4f) || (battery <= 5f && battery > 4.4f))
        {
            showToast("배터리가 없습니다. \n 충전기로 돌아가세요.", 2);

        }

        if (battery <= 10f && Time.timeScale >= 1f)
        {
            Time.timeScale = 0.8f;
        }
        else if (battery > 10f && Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        if (battery <= 0)
        {
            battery = 0;
            batteryText.text = battery.ToString("f2") + "%";
            batteryText2.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
            batterySlider2.value = battery;
            Time.timeScale = 1f;
            gameOver();
            return;
        }
        else
        {
            batteryText.text = battery.ToString("f2") + "%";
            batteryText2.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
            batterySlider2.value = battery;
        }
    }

    private void blackOutAndGameOver()
    {

    }


    public void useBattery()
    {
        float weight = 5.3f - (0.04f * profile[2]);
        battery -= Time.deltaTime * weight;
        if((battery <= 15f && battery > 14.4f) || (battery <= 5f && battery > 4.4f))
        {
            showToast("배터리가 없습니다. \n 충전기로 돌아가세요.", 2); 
          
        }

        if (battery <= 10f && Time.timeScale >= 1f)
        {
            Time.timeScale = 0.8f;
        }
        else if (battery > 10f && Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        if (battery <= 0)
        {
            battery = 0;
            batteryText.text = battery.ToString("f2") + "%";
            batteryText2.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
            batterySlider2.value = battery;
            Time.timeScale = 1f;
            gameOver();
            return;
        } else
        {
            batteryText.text = battery.ToString("f2") + "%";
            batteryText2.text = battery.ToString("f2") + "%";
            batterySlider.value = battery;
            batterySlider2.value = battery;
        }

    }

    private void updateStage()
    {
        stageText.text = "" + (stage + 1);
    }




    private void cleanDustAndRings()
    {
        Debug.Log("levelDustGroup[stage].transform.childCount " + levelDustGroup[stage / 3].transform.childCount);
        for (int i = 0; i < levelDustGroup[stage / 3].transform.childCount; i++)
        {
            Destroy(levelDustGroup[stage / 3].transform.GetChild(i).gameObject);
        }

        for(int i=0;i<ringGroup.transform.childCount;i++)
        {
            Destroy(ringGroup.transform.GetChild(i).gameObject);
        }
    }


    public int getAreaLimit()
    {
        return stage < dustArea.Length ? dustArea[stage] : 50;
    }

    public void generateDust(Vector3 enemyPos)
    {
        float limit = 3f;
        float x = UnityEngine.Random.Range(enemyPos.x - limit, enemyPos.x + limit);
        float z = UnityEngine.Random.Range(enemyPos.z - limit, enemyPos.z + limit);
        var pos = new Vector3(x, 0, z);
        GameObject dustObj = Instantiate(dustPrefab, pos, Quaternion.Euler(Vector3.zero), levelDustGroup[stage / 3].transform);
        dust.Add(dustObj.transform);

        updateCleanRate(false);

        //var comparison = new Comparison<Transform>(CompareIntMethod);
        //dust.Sort(comparison);
    }

    private void appendCat()
    {
        Debug.Log("cat cnt " + catCnt[stage]);
        int[] staridx = { 0, 3, 6, 9, 12, 14, 16, 19, 22, 25 };

        for (int i = 0; i < catCnt[stage]; i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = 0;
            do
            {
                x = UnityEngine.Random.Range(-limit, limit);
            } while (x < 9 && x > -9);
            var z = 0;
            do
            {
                z = UnityEngine.Random.Range(-limit, limit);
            } while (z < 9 && z > -9);
            var pos = new Vector3(x, 0, z);
            //int catIdx = UnityEngine.Random.Range(0, catPrefab.Length);
            int cnt = stage / 3 == 4 || stage / 3 == 5 ? 2 : 3;
            int start = staridx[stage / 3];
            int end = start + cnt;
            int catIdx = UnityEngine.Random.Range(start, end);
            Debug.Log(start + "~" + end + " / cat idx " + catIdx);
            var obj = Instantiate(catPrefab[catIdx], pos, Quaternion.Euler(new Vector3(0, 0, 0)), levelDustGroup[stage / 3].transform);
            var enemy = obj.GetComponent<EnemyScript>();
            enemy.setEnemyIdx(catIdx);
        }
        aliveCatCnt = catCnt[stage];
    }


    private void updateDust()
    {
        surpriseCat = 0;
        currentDustIdx = 0;
        if(stage - 1 >= 0)
        {
            levelDustGroup[(stage - 1) / 3].SetActive(false);
            levelObjectsGroup[(stage - 1)/3].SetActive(false);
        }
        levelObjectsGroup[stage / 3].SetActive(true);
        levelDustGroup[stage / 3].SetActive(true);
        dust.Clear();

        /*

        for (int i = 0; i < dustCnt[stage]; i++)
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
            var pos = new Vector3(x, 1f, z);
            Instantiate(dustPrefab, pos, Quaternion.Euler(Vector3.zero), levelDustGroup[stage].transform);
        }*/

        for (int i = 0; i < ringCnt[stage]; i++)
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
            var pos = new Vector3(x, 0.6f, z);
            Instantiate(ringPrefab, pos, Quaternion.Euler(new Vector3(-20f, -45f, 0)), ringGroup.transform);
        }

        /*
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
        */

        appendCat();
        currentCatMultiply = catMultiply[stage];
        

        /*
        for (int i = 0; i < stage + 1; i++)
        {
            int limit = stage < dustArea.Length ? dustArea[stage] : 50;
            var x = UnityEngine.Random.Range(-limit, limit);
            var z = UnityEngine.Random.Range(-limit, limit);
            var pos = new Vector3(x, 2.5f, z);
            Instantiate(sharkPrefab, pos, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-90, 90), 0)), levelObjectsGroup[stage].transform);
        }*/



        /*
        for (int i = 0; i < levelDustGroup[stage].transform.childCount; i++)
        {
            dust.Add(levelDustGroup[stage].transform.GetChild(i));
        }
        var comparison = new Comparison<Transform>(CompareIntMethod);
        dust.Sort(comparison);*/
    }

    private static int CompareIntMethod(Transform a, Transform b)
    {
        float aa = Vector3.Distance(a.position, Vector3.zero);
        float bb = Vector3.Distance(b.position, Vector3.zero);
        return aa.CompareTo(bb);
    }

    public void nextStage()
    {

        clickAudio.Play();
        Debug.Log("next stage.");
        closeAllPanel();
        isCharger = false;
        isManual = true;
        agent.ResetPath();
        agent.gameObject.transform.position = new Vector3(0, 0.5f, 0);
        cleanDustAndRings();
        stage += 1;
        time = 0;
        totalTime = 0;
        stageGold = 0;
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
        lastAudioIdx = UnityEngine.Random.Range(0, audio.Length);
        audio[lastAudioIdx].Play();


        if(isNoAd <= 0)
        {
            Debug.Log("show ad front");
            admobManager.ShowFrontAd();
        }

    }

    public void closeAllPanel()
    {
        clickAudio.Play();

        if (!isPause)
        {
            Time.timeScale = 1f;
            playLastAudio();
        }

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
        clickAudio.Play();
        closeAllPanel();
        Time.timeScale = 0f;
        panels[idx].SetActive(true);
        if(idx == 2)
        {
            //speakerItem.SetActive(!buySpeaker);
            shopPanel.open();
        }

    
        if (idx == 0)
        {
            statPanel.open();
        }

        if (idx ==  3 || idx == 4)
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

        if(idx == 1)
        {
            storagePanel.open();
        }
        
        panels[idx].transform.GetChild(1).DOLocalMoveY(0, 1).SetUpdate(true);
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
        PlayerPrefs.Save();
        updateGoldText();
    }

    public void updateStageGold(int earn)
    {
        stageGold += earn;
    }

    public void earnGold(int earn)
    {
        gold += earn;
        PlayerPrefs.SetInt("gold", gold);
        PlayerPrefs.Save();
        updateGoldText();
        panels[0].GetComponent<StatPanel>().updateGoldText();
        shakeAnimation(goldText.gameObject, goldSeq);
    }



    public int getProfile(int idx)
    {
        return profile[idx];
    }

    public bool levelUpProfile(int idx)
    {
        if (profile[idx] + 1 <= 99)
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
            PlayerPrefs.Save();
            return true;

        } else
        {
            return false;
        }
    }

    public void levelUpAgentSpeed()
    {
        float alpha = profile[0] > 1 ? 7 : 5;
        agent.speed = profile[0] * 0.66f + alpha;
        agent.acceleration = profile[0] * 0.66f + alpha;
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
        clickAudio.Play();
        int sz = 30;
        if (idx == 0)
        {
            if (Camera.main.orthographicSize == 15)
            {
                sz = 10;
            }
            else
            {
                sz = 15;
            }
        }
        else if (idx == 2)
        {
            if (Camera.main.orthographicSize == 40)
            {
                sz = 50;
            }
            else
            {
                sz = 40;
            }
        }
        Camera.main.orthographicSize = sz;
    }

    private void OnDestroy()
    {
        closeAllKillSeq();
    }

    public GameObject getRingCanvas()
    {
        return ringCanvas;
    }

    public void onSpeakerBtn()
    {
        clickAudio.Play();
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
        //speakerBtn.SetActive(buySpeaker);
        //speakerObj.SetActive(buySpeaker);
    }

    public float getTime()
    {
        return time;
    }

    public void playClickAudio()
    {
        clickAudio.Play();
    }

    public void tryToPurchase()
    {
        foreach(GameObject obj in blockBuyingBtn)
        {
            obj.SetActive(false);
        }
    }
    public void finishBuying()
    {
        foreach (GameObject obj in blockBuyingBtn)
        {
            obj.SetActive(true);
        }
    }

    public void playLastAudio()
    {
        if(!audio[lastAudioIdx].isPlaying)
        {
            audio[lastAudioIdx].Play();
        }
    }

    public void pauseLastAudio()
    {
        if(audio[lastAudioIdx].isPlaying)
        {
            audio[lastAudioIdx].Pause();
        }
    }

    public void setIsPause(bool status)
    {
        isPause = status;
    }

    public void shakeCamera()
    {
        cameraShake.TriggerShake();
    }

    public void fadeOut()
    {
        darkFade.gameObject.SetActive(true);
        darkFade.alpha = 1;
        darkFade.DOFade(0, 3).OnComplete(() =>
        {
            darkFade.alpha = 0;
            darkFade.gameObject.SetActive(false);
            isGameOver = false;
        });
    }

    public bool getIsGameOver()
    {
        return isGameOver;
    }

    public bool getIsNight()
    {
        return isNight;
    }

    void changeDayNight()
    {
        if(isNight)
        {
            isNight = false;
            //light.colorTemperature = 5536;
            light.color = Color.white;
        } else
        {
            isNight = true;
            //light.colorTemperature = 20000;
            light.color = new Color32(150, 150, 240, 255);
        }
    }

    public void reduceAliveCatCnt()
    {
        aliveCatCnt -= 1;
        if(aliveCatCnt < 0)
        {
            aliveCatCnt = 0;
        }
        if(aliveCatCnt == 0 && currentCatMultiply != 0)
        {
            currentCatMultiply -= 1;
            if(currentCatMultiply > 0)
            {
                showToast((catMultiply[stage] - currentCatMultiply + 1) + " of " + catMultiply[stage] + " 라운드", 0);
                appendCat();
            }
        }
        Debug.Log("aliveCatCnt " + aliveCatCnt + " / " + currentCatMultiply);
    }

    public Sprite getWeaponSprite(int idx)
    {
        return weaponSprites[idx];
    }

    public Sprite getSkinSprite(int idx)
    {
        return skinSprites[idx];
    }

    public int getMySkinIdX()
    {
        return skinIdx;
    }

    public void setMySkinId(int _idx)
    {
        skinIdx = _idx;
        PlayerPrefs.SetInt("skin", skinIdx);
    }

    public int getMyWeaponIdx()
    {
        return weaponIdx;
    }

    public void setMyWeaponIdx(int _idx)
    {
        weaponIdx = _idx;
        PlayerPrefs.SetInt("weapon", weaponIdx);
    }

    public int getAtk(int _idx)
    {
        return atk[_idx];
    }

    public int getDef(int _idx)
    {
        return def[_idx];
    }
    
    public string getDex(int _idx1, int _idx2)
    {
        return "" + dex1[_idx1] + "/" + dex2[_idx2];
    }

    public int getAtk()
    {
        if(weaponIdx >= atk.Length)
        {
            return atk[0];
        }
        return atk[weaponIdx];
    }

    public int getDef()
    {
        if(skinIdx >= def.Length)
        {
            return def[0];
        }
        return def[skinIdx];
    }

    public int getDex1(int _idx)
    {
        return dex1[_idx];
    }

    public int getDex2(int _idx)
    {
        return dex2[_idx];
    }

    public float getDex()
    {
        int a = 1;
        int b = 10;
        if(skinIdx >= dex1.Length)
        {
            a = dex1[0];
        } else
        {
            a = dex1[skinIdx];
        }
        if(weaponIdx >= dex2.Length)
        {
            b = dex2[0];
        } else
        {
            b = dex2[weaponIdx];
        }
        float ret = (float)a / (float)b;
        return ret;
    } 

    public int getEnemyAtk(int idx)
    {
        if(idx >= enemyAtk.Length)
        {
            return enemyAtk[0];
        }
        return enemyAtk[idx];
    }

    public void setDia(int _dia)
    {
        dia = _dia;
        PlayerPrefs.SetInt("dia", dia);
    }

    public int getDia()
    {
        return dia;
    }

    public void updateDiaText()
    {
        diaText.text = $"{dia:N0} DIA";
    }

    public int getSkinDia(int _idx)
    {
        return skinDia[_idx];
    }

    public int getWeaponDia(int _idx)
    {
        return weaponDia[_idx];
    }

    public void saveMyAsset(int type, int idx)
    {
        myAssetList.Add(new Vector2(type, idx));
        AssetData data = new AssetData();
        data.assetList = myAssetList;
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("myAsset", json);
    }

    public bool existInMyAsset(int type, int idx)
    {
        foreach(var item in myAssetList)
        {
            if(item.x == type && item.y == idx)
            {
                return true;
            }
        }
        return false;
    }

    public List<Vector2> getMyAssetList()
    {
        return myAssetList;
    }

    public void updateCleanRateNotifier()
    {
        if(cleanRateNotiferSeq != null)
        {
            cleanRateNotiferSeq.Kill();
        }
        cleanRateNotifier.gameObject.SetActive(true);
        cleanRateNotifier.alpha = 0;
        cleanRateNotiferSeq = DOTween.Sequence();
        cleanRateNotiferSeq.Append(cleanRateNotifier.DOFade(0.6f, 1))
            .Append(cleanRateNotifier.DOFade(0, 0.5f)).OnComplete(() => {
                cleanRateNotifier.gameObject.SetActive(false);
            });
    }

    public string getSkinName(int _idx)
    {
        return skinName[_idx];
    }

    public string getWeaponName(int _idx)
    {
        return weaponName[_idx];
    }

    public void playHitAudio()
    {
        hitAudio.Play();
    }
    public void playHitRobotAudio()
    {
        hitRobotAudio.Play();
    }

    public void playWinAudio()
    {
        winAudio.Play();
    }

    public void updateIsNoAd()
    {
        isNoAd = 1;
        PlayerPrefs.SetInt("isNoAd", 1);
    }

    public void playRewardAd()
    {
        string lastReward = PlayerPrefs.GetString("lastReward", "");
        DateTime today = DateTime.Today;
        if(lastReward != "")
        {
            DateTime lastRewardDate = DateTime.Parse(lastReward);
            Debug.Log("last reward date " + lastRewardDate);
            if(lastRewardDate == today)
            {
                showToast("하루에 한번 보상이 가능합니다.", 0);
                return;
            } 
        }
        admobManager.ShowRewardAd(completed => {
            int myDia = getDia();
            myDia += 100;
            setDia(myDia);
            updateDiaText();

            PlayerPrefs.SetString("lastReward", today.ToString());
            showToast("다이아 100개가 증정되었습니다!", 1);
            playWinAudio();
        });
    }

}
