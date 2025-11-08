using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{    
    public TMPro.TextMeshProUGUI timeText;
    public TMPro.TextMeshProUGUI timeBonusText;
    public TMPro.TextMeshProUGUI catBonusText;
    public TMPro.TextMeshProUGUI catBonusCntText;
    public TMPro.TextMeshProUGUI totalBonusText;


    int bonus = 0;
    



    public void setTime(float time, int gold)
    {
        bonus = gold;
        
        string sec = (time % 60).ToString("f2");
        if (sec.Length <= 4)
        {
            sec = "0" + sec;
        }
        string mm = Mathf.FloorToInt((time / 60) % 60).ToString("D2");
        string hh = Mathf.FloorToInt(time / 60 / 60).ToString("D2");
        timeText.text = hh + ":" + mm + ":" + sec;

        timeBonusText.text = $"{bonus:N0}G";
        totalBonusText.text = $"{bonus:N0}G";
    }


    public void setCat(int cat)
    {
        int catBonus = cat * 20;
        catBonusCntText.text = $"{cat:N0}";
        catBonusText.text = $"{catBonus:N0}G";
        bonus += catBonus;
        totalBonusText.text = $"{bonus:N0}G";
        GameController.Instance.earnGold(bonus);
    }


}
