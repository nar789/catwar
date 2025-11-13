using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopPanel : MonoBehaviour
{

    public TMPro.TextMeshProUGUI diaText;

    public GameObject containerSkin;
    public GameObject containerWeapon;
    public GameObject shopItemPrefab;

    List<Vector2> list = new List<Vector2>();

    bool isFirst = true;

    public TMPro.TextMeshProUGUI[] filterText;
    Color32 textDefaultColor = new Color32(148, 131, 171, 255);
    Color32 pressTextColor = new Color32(212, 181, 255, 255);


    public GameObject buyItemPanel;
    public Image buyItemImage;
    public TMPro.TextMeshProUGUI buyItemName;
    public GameObject skinStat;
    public GameObject weaponStat;
    public TMPro.TextMeshProUGUI atk;
    public TMPro.TextMeshProUGUI def;
    public TMPro.TextMeshProUGUI dex1;
    public TMPro.TextMeshProUGUI dex2;
    public TMPro.TextMeshProUGUI buyItemDia;
    int opendBuyItemType = 0;
    int opendBuyItemIdx = 0;

    public StoragePanel storagePanel;

    public GameObject alreadyBuyText;
    public GameObject buyBtn;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void init()
    {
        for (int i = 0; i < 27; i++)
        {
            list.Add(new Vector2(1, i));
        }
        for (int i = 0; i < 27; i++)
        {
            list.Add(new Vector2(2, i));
        }
        generateItems();
    }

    public void open()
    {
        if(isFirst)
        {
            isFirst = false;
            init();
        }
        closeBuyItemPanel();
        updateDiaText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickBtn()
    {
        GameController.Instance.playClickAudio();
        Debug.Log("shop onClickBtn ");
        GameController.Instance.tryToPurchase();
    }

    public void onFailedBuying()
    {
        Debug.Log("shop onFailedBuying ");
        GameController.Instance.finishBuying();
    }

    public void buySpeaker()
    {
        Debug.Log("buySpeaker");
        GameController.Instance.buySpeakerByStore();
        GameController.Instance.closeAllPanel();
        GameController.Instance.showToast("냥이 퇴치 스피커가 획득되었습니다!", 1);
        GameController.Instance.finishBuying();
        
    }

    public void buyGold()
    {
        Debug.Log("buyGold");
        GameController.Instance.earnGold(30000);
        GameController.Instance.closeAllPanel();
        GameController.Instance.showToast("30,000골드가 증정되었습니다!", 1);
        GameController.Instance.finishBuying();
    }

    private void updateDiaText()
    {
        int dia = GameController.Instance.getDia();
        diaText.text = $"{dia:N0} DIA";
    }


    public void generateItems()
    {
        foreach (Vector2 vec in list)
        {
            GameObject item = Instantiate(shopItemPrefab, vec.x == 1 ? containerSkin.transform : containerWeapon.transform);
            ShopItem controller = item.GetComponent<ShopItem>();
            controller.setSprite((int)vec.x, (int)vec.y);
        }

    }

    public void setFilter(int filterType)
    {
        GameController.Instance.playClickAudio();
        GameObject[] containers = { containerSkin, containerWeapon };
        for (int i = 0; i < 2; i++)
        {
            filterText[i].color = textDefaultColor;
            containers[i].SetActive(false);
        }
        containers[filterType].SetActive(true);
        filterText[filterType].color = pressTextColor;
    }

    public void openBuyItemDialog(int type, int idx)
    {
        GameController.Instance.playClickAudio();
        opendBuyItemType = type;
        opendBuyItemIdx = idx;

        skinStat.SetActive(false);
        weaponStat.SetActive(false);
        if(type == 1)
        {
            buyItemImage.sprite = GameController.Instance.getSkinSprite(idx);
            skinStat.SetActive(true);
            int defValue = GameController.Instance.getDef(idx) * 10;
            def.text = $"{defValue:N0}";
            dex1.text = GameController.Instance.getDex1(idx) + "";

            buyItemDia.text = $"{GameController.Instance.getSkinDia(idx):N0}";
        } else if(type == 2)
        {
            buyItemImage.sprite = GameController.Instance.getWeaponSprite(idx);
            weaponStat.SetActive(true);
            int atkValue = GameController.Instance.getAtk(idx) * 100;
            atk.text = $"{atkValue:N0}";
            dex2.text = GameController.Instance.getDex2(idx) + "";

            buyItemDia.text = $"{GameController.Instance.getWeaponDia(idx):N0}";
        }

        buyItemName.text = (type == 1) ? GameController.Instance.getSkinName(idx) : GameController.Instance.getWeaponName(idx);

        if(GameController.Instance.existInMyAsset(type, idx))
        {
            alreadyBuyText.SetActive(true);
            buyBtn.SetActive(false);
        } else
        {
            alreadyBuyText.SetActive(false);
            buyBtn.SetActive(true);
        }


        buyItemPanel.SetActive(true);
    }

    public void closeBuyItemPanel()
    {
        opendBuyItemType = 0;
        opendBuyItemIdx = 0;
        buyItemPanel.SetActive(false);
    }


    public void buyItem()
    {
        Debug.Log("buyItem " + opendBuyItemType + " / " + opendBuyItemIdx);
        int diaAmount = 0;
        if(opendBuyItemType == 1)
        {
            diaAmount = GameController.Instance.getSkinDia(opendBuyItemIdx);
        } else if(opendBuyItemType == 2)
        {
            diaAmount = GameController.Instance.getWeaponDia(opendBuyItemIdx);
        }

        int myDia = GameController.Instance.getDia();
        if (myDia < diaAmount)
        {
            GameController.Instance.showToast("다이아가 모자랍니다.", 2);
            return;
        } else
        {
            myDia -= diaAmount;
            GameController.Instance.setDia(myDia);
            GameController.Instance.updateDiaText();
            updateDiaText();

            GameController.Instance.saveMyAsset(opendBuyItemType, opendBuyItemIdx);
            storagePanel.reload();
            string name = (opendBuyItemType == 1) ? GameController.Instance.getSkinName(opendBuyItemIdx) : GameController.Instance.getWeaponName(opendBuyItemIdx);
            GameController.Instance.showToast("[" + name + "] 구매 성공!", 1);
            GameController.Instance.playWinAudio();
        }

        closeBuyItemPanel();
    }


    public void buyDia100()
    {
        Debug.Log("buy dia 100");
        int myDia = GameController.Instance.getDia();
        myDia += 100;
        GameController.Instance.setDia(myDia);
        GameController.Instance.updateDiaText();
        updateDiaText();
        GameController.Instance.showToast("다이아 100개가 증정되었습니다!", 1);
        GameController.Instance.finishBuying();
        GameController.Instance.playWinAudio();
    }


    public void buyDia300()
    {
        Debug.Log("buy dia 300");
        int myDia = GameController.Instance.getDia();
        myDia += 300;
        GameController.Instance.setDia(myDia);
        GameController.Instance.updateDiaText();
        updateDiaText();
        GameController.Instance.showToast("다이아 300개가 증정되었습니다!", 1);
        GameController.Instance.finishBuying();
        GameController.Instance.playWinAudio();
    }


    public void buyDia500()
    {
        Debug.Log("buy dia 500");
        int myDia = GameController.Instance.getDia();
        myDia += 500;
        GameController.Instance.setDia(myDia);
        GameController.Instance.updateDiaText();
        updateDiaText();
        GameController.Instance.showToast("다이아 500개가 증정되었습니다!", 1);
        GameController.Instance.finishBuying();
        GameController.Instance.playWinAudio();
    }



    public void buyDia1000()
    {
        Debug.Log("buy dia 1000");
        int myDia = GameController.Instance.getDia();
        myDia += 1000;
        GameController.Instance.setDia(myDia);
        GameController.Instance.updateDiaText();
        updateDiaText();
        GameController.Instance.showToast("다이아 1000개가 증정되었습니다!", 1);
        GameController.Instance.finishBuying();
        GameController.Instance.playWinAudio();
    }

    public void buyStarterPack()
    {
        Debug.Log("buy starter pack");
        int myDia = GameController.Instance.getDia();
        myDia += 200;
        GameController.Instance.setDia(myDia);
        GameController.Instance.updateDiaText();
        updateDiaText();

        GameController.Instance.earnGold(10000);

        if (!GameController.Instance.existInMyAsset(1, 1))
        {
            GameController.Instance.saveMyAsset(1, 1);
            storagePanel.reload();
        }

        GameController.Instance.updateIsNoAd();

        GameController.Instance.showToast("스타터팩이 증정되었습니다!", 1);
        GameController.Instance.finishBuying();
        GameController.Instance.playWinAudio();
    }

}
