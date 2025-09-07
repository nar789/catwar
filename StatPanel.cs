using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPanel : MonoBehaviour
{


    public TMPro.TextMeshProUGUI gold;
    public TMPro.TextMeshProUGUI speed;
    public TMPro.TextMeshProUGUI charge;
    public TMPro.TextMeshProUGUI battery;

    public TMPro.TextMeshProUGUI[] priceText;

    int[] price = { 0, 5000, 10000, 20000, 30000, 45000, 70000, 75000, 80000, 90000, 0};


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void updateGoldText()
    {
        gold.text = $"{GameController.Instance.getGold():N0} G";
    }

    public void updateProfile()
    {
        speed.text = "Lv." + GameController.Instance.getProfile(0);
        charge.text = "Lv." + GameController.Instance.getProfile(1);
        battery.text = "Lv." + GameController.Instance.getProfile(2);
    }

    public void updatePrice()
    {
        for(int i=0;i<3;i++)
        {
            int level = GameController.Instance.getProfile(i);
            int pri = price[level];
            priceText[i].text = $"{pri:N0}G";
        }
    }


    public void speedLevelUp()
    {
        int level = GameController.Instance.getProfile(0);
        if (GameController.Instance.getGold() < price[level])
        {
            GameController.Instance.showToast("돈이 모자랍니다.", 2);
            return;
        }
        if (GameController.Instance.levelUpProfile(0))
        {
            int lv = GameController.Instance.getProfile(0);
            speed.text = "Lv." + (lv + 1);
            GameController.Instance.useGold(price[level]);
            updateGoldText();
            GameController.Instance.levelUpAgentSpeed();
            updatePrice();
        } else
        {
            GameController.Instance.showToast("최대 레벨입니다\n 포자들을 쓸어보세요", 0);
        }
    }

    public void chargeLevelUp()
    {
        int level = GameController.Instance.getProfile(1);
        if (GameController.Instance.getGold() < price[level])
        {
            GameController.Instance.showToast("돈이 모자랍니다.", 2);
            return;
        }
        if (GameController.Instance.levelUpProfile(1))
        {
            int lv = GameController.Instance.getProfile(1);
            charge.text = "Lv." + (lv + 1);
            GameController.Instance.useGold(price[level]);
            updateGoldText();
            updatePrice();
        }
        else
        {
            GameController.Instance.showToast("최대 레벨입니다\n 포자들을 쓸어보세요", 0);
        }
    }

    public void batteryLevelUp()
    {
        int level = GameController.Instance.getProfile(2);
        Debug.Log("gold " + GameController.Instance.getGold() + " / " + price[level]);
        if (GameController.Instance.getGold() < price[level])
        {
            GameController.Instance.showToast("돈이 모자랍니다.", 2);
            return;
        }
        if (GameController.Instance.levelUpProfile(2))
        {
            int lv = GameController.Instance.getProfile(2);
            battery.text = "Lv." + (lv + 1);
            GameController.Instance.useGold(price[level]);
            updateGoldText();
            updatePrice();
        }
        else
        {
            GameController.Instance.showToast("최대 레벨입니다\n 포자들을 쓸어보세요", 0);
        }
    }

}
