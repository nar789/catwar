using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image mainImage;
    public TMPro.TextMeshProUGUI diaText;

    ShopPanel shopPanel;

    int itemType = 0;
    int itemIdx = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopPanel = GameObject.Find("ShopCanvas").GetComponent<ShopPanel>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void onBtnClick()
    {
        shopPanel.openBuyItemDialog(itemType, itemIdx);
    }


    public void setSprite(int type, int idx)
    {

        itemType = type;
        itemIdx = idx;
        if (type == 1) // skin
        {
            mainImage.sprite = GameController.Instance.getSkinSprite(idx);
            diaText.text = $"{GameController.Instance.getSkinDia(idx):N0}";
        }
        else if (type == 2)// weapon
        {
            mainImage.sprite = GameController.Instance.getWeaponSprite(idx);
            diaText.text = $"{GameController.Instance.getWeaponDia(idx):N0}";
        }
    }
}
