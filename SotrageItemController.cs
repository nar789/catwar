using UnityEngine;
using UnityEngine.UI;

public class SotrageItemController : MonoBehaviour
{
    public GameObject selected;
    public Image mainImage;

    public StoragePanel storagePanel;

    int itemType = 0;
    int itemIdx = 0;

    Image myRobo;
    Image myWeapon;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storagePanel = GameObject.Find("StorageCanvas").GetComponent<StoragePanel>();
        myRobo = GameObject.Find("MyRoboImage").GetComponent<Image>();
        myWeapon = GameObject.Find("MyWeapon").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected.activeSelf)
        {
            if (itemType == 1)
            {
                if (storagePanel.getSkinIdx() != itemIdx)
                {
                    selected.SetActive(false);
                }
            } else if (itemType == 2)
            {
                if(storagePanel.getWeaponIdx() != itemIdx)
                {
                    selected.SetActive(false);
                }
            }
            
        }         
    }

    public void onBtnClick()
    {         
        selected.SetActive(!selected.activeSelf);
        if (selected.activeSelf)
        {
            if (itemType == 1)
            {
                myRobo.sprite = mainImage.sprite;
                storagePanel.setSkinIdx(itemIdx);
            }
            else if (itemType == 2)
            {
                myWeapon.sprite = mainImage.sprite;
                storagePanel.setWeaponIdx(itemIdx);
            }
        } else
        {
            clearMySprites();
        }

    }


    public void clearSelect()
    {
        selected.SetActive(false);
    }


    public void clearMySprites()
    {
        if (itemType == 1)
        {
            myRobo.sprite = GameController.Instance.getSkinSprite(GameController.Instance.getMySkinIdX());
        }
        else if (itemType == 2)
        {
            myWeapon.sprite = GameController.Instance.getWeaponSprite(GameController.Instance.getMyWeaponIdx());
        }
    }

    public void setSprite(int type, int idx)
    {

        itemType = type;
        itemIdx = idx;
        if (type == 1) // skin
        {
            mainImage.sprite = GameController.Instance.getSkinSprite(idx);
        } else if(type == 2)// weapon
        {
            mainImage.sprite = GameController.Instance.getWeaponSprite(idx);
        }
    }
}
