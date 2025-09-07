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
    



    public void setTime(float time, int cnt)
    {
        bonus = 0;
        int timeBonus = 0;
   
        string sec = (time % 60).ToString("f2");
        if (sec.Length <= 4)
        {
            sec = "0" + sec;
        }
        string mm = Mathf.FloorToInt((time / 60) % 60).ToString("D2");
        string hh = Mathf.FloorToInt(time / 60 / 60).ToString("D2");
        timeText.text = hh + ":" + mm + ":" + sec;


        if(5 * cnt >= time)
        {
            timeBonus += 500;
        } else if((5 * cnt) / 2 >= time)
        {
            timeBonus += 1000;
        } else if((5 * cnt) / 3>= time)
        {
            timeBonus += 2000;
        }

        bonus += timeBonus;

        timeBonusText.text = $"{timeBonus:N0}G";
        totalBonusText.text = $"{bonus:N0}G";
        //GameController.Instance.earnGold(bonus);
    }


    public void setCat(int cat, int stage)
    {
        if(cat > (stage + 1))
        {
            cat = stage + 1;
        }
        int catBonus = cat * 20;
        catBonusCntText.text = $"{cat:N0}";
        catBonusText.text = $"{catBonus:N0}G";
        bonus += catBonus;
        totalBonusText.text = $"{bonus:N0}G";
        GameController.Instance.earnGold(bonus);
    }

    



}
