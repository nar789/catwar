using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatPanel : MonoBehaviour
{


    public TMPro.TextMeshProUGUI gold;
    public TMPro.TextMeshProUGUI speed;
    public TMPro.TextMeshProUGUI charge;
    public TMPro.TextMeshProUGUI battery;

    public TMPro.TextMeshProUGUI[] priceText;

    int[] price = { 50, 100, 150, 200, 300, 450, 700, 750, 800, 900, 0};

    public AudioSource levelUpAudio;


    NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void open()
    {
        if(agent == null)
        {
            agent = GameObject.Find("Robo").GetComponent<NavMeshAgent>();
            updateGoldText();
            updateProfile();
            updatePrice();
        }
    }


    public void updateGoldText()
    {
        gold.text = $"{GameController.Instance.getGold():N0} G";
    }

    public void updateProfile()
    {

        int lv1 = GameController.Instance.getProfile(0);
        int value1 = (int)agent.acceleration * 1000;

        int lv2 = GameController.Instance.getProfile(1);
        int value2 = (int)(30 + lv2 * 0.33f * 1000);

        int lv3 = GameController.Instance.getProfile(2);
        int value3 = (int)(((0.04f * lv3) / 4) * 100);

        speed.text = $"{value1:N0} (Lv{lv1})";
        charge.text = $"{value2:N0} (Lv{lv2})";
        battery.text = $"{value3:N0}% (Lv{lv3})";
    }

    public void updatePrice()
    {
        for(int i=0;i<3;i++)
        {
            int level = GameController.Instance.getProfile(i);
            int pri = price[level / 10];
            priceText[i].text = $"{pri:N0}G";
        }
    }


    public void speedLevelUp()
    {
        levelUpAudio.Play();
        int level = GameController.Instance.getProfile(0);
        int gold = price[level / 10];
        if (GameController.Instance.getGold() < gold)
        {
            GameController.Instance.showToast("돈이 모자랍니다.", 2);
            return;
        }

        

        if (GameController.Instance.levelUpProfile(0))
        {
            GameController.Instance.useGold(gold);
            updateGoldText();
            int lv1 = GameController.Instance.getProfile(0);
            int value1 = (int)agent.acceleration * 1000;
            speed.text = $"{value1:N0} (Lv{lv1})";
            GameController.Instance.levelUpAgentSpeed();
            updatePrice();
        } else
        {
            GameController.Instance.showToast("최대 레벨입니다\n 먼지들을 쓸어보세요", 0);
        }
    }

    public void chargeLevelUp()
    {
        levelUpAudio.Play();

        int level = GameController.Instance.getProfile(1);
        int gold = price[level / 10];
        if (GameController.Instance.getGold() < gold)
        {
            GameController.Instance.showToast("돈이 모자랍니다.", 2);
            return;
        }

        

        if (GameController.Instance.levelUpProfile(1))
        {
            GameController.Instance.useGold(gold);
            updateGoldText();
            int lv2 = GameController.Instance.getProfile(1);
            int value2 = (int)(30 + lv2 * 0.33f * 1000);
            charge.text = $"{value2:N0} (Lv{lv2})";
            updatePrice();
        }
        else
        {
            GameController.Instance.showToast("최대 레벨입니다\n 먼지들을 쓸어보세요", 0);
        }
    }

    public void batteryLevelUp()
    {
        levelUpAudio.Play();


        int level = GameController.Instance.getProfile(2);
        int gold = price[level / 10];
        if (GameController.Instance.getGold() < gold)
        {
            GameController.Instance.showToast("돈이 모자랍니다.", 2);
            return;
        }

        

        if (GameController.Instance.levelUpProfile(2))
        {
            GameController.Instance.useGold(gold);
            updateGoldText();
            int lv3 = GameController.Instance.getProfile(2);
            int value3 = (int)(((0.04f * lv3) / 4) * 100);
            battery.text = $"{value3:N0}% (Lv{lv3})";
            updatePrice();
        }
        else
        {
            GameController.Instance.showToast("최대 레벨입니다\n 먼지들을 쓸어보세요", 0);
        }
    }

}
