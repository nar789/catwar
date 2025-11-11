using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StoragePanel : MonoBehaviour
{

    public GameObject container;
    public GameObject containerSkin;
    public GameObject containerWeapon;
    public GameObject storageItemPrefab;

    int weaponIdx = 0;
    int skinIdx = 0;

    List<Vector2> list = new List<Vector2>();
    List<SotrageItemController> itemList = new List<SotrageItemController>();

    Image myRobo;
    Image myWeapon;

    public TMPro.TextMeshProUGUI[] filterText;
    Color32 textDefaultColor = new Color32(150, 171, 184, 255);
    Color32 pressTextColor = new Color32(181, 222, 255, 255);

    public TMPro.TextMeshProUGUI[] baseInfo;
    public TMPro.TextMeshProUGUI[] skinInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

     
    }

    void init()
    {
        if(myRobo == null)
        {
            myRobo = GameObject.Find("MyRoboImage").GetComponent<Image>();
            myWeapon = GameObject.Find("MyWeapon").GetComponent<Image>();
            /*
            for (int i = 0; i < 27; i++)
            {
                list.Add(new Vector2(1, i));
            }
            for (int i = 0; i < 27; i++)
            {
                list.Add(new Vector2(2, i));
            }*/

            reload();
            updateBaseInfo();
            updateSkinInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void updateSkinInfo()
    {
        string[] values =
            {
            GameController.Instance.getAtk(weaponIdx) + "",
            GameController.Instance.getDef(skinIdx) + "",
            GameController.Instance.getDex(skinIdx, weaponIdx)
        };

        skinInfo[0].text = $"{(int.Parse(values[0]) * 100):N0}";
        skinInfo[1].text = $"{(int.Parse(values[1]) * 10):N0}";
        skinInfo[2].text = values[2];
    }

    private void updateBaseInfo()
    {
        int lv1 = GameController.Instance.getProfile(0);
        var agent = GameObject.Find("Robo").GetComponent<NavMeshAgent>();
        int value1 = (int)agent.acceleration * 1000;
        baseInfo[0].text = $"{value1:N0} (Lv{lv1})";

        int lv2 = GameController.Instance.getProfile(1);
        int value2 = (int)(30 + lv2 * 0.33f * 1000);
        baseInfo[1].text = $"{value2:N0} (Lv{lv2})";

        int lv3 = GameController.Instance.getProfile(2);
        int value3 = (int)(((0.04f * lv3) / 4) * 100);
        baseInfo[2].text = $"{value3:N0}% (Lv{lv3})";
    }


    public void setFilter(int filterType)
    {
        GameController.Instance.playClickAudio();
        GameObject[] containers = {container, containerSkin, containerWeapon };
        for(int i=0;i<3;i++)
        {
            filterText[i].color = textDefaultColor;
            containers[i].SetActive(false);
        }
        containers[filterType].SetActive(true);
        filterText[filterType].color = pressTextColor;
    }

    

    public int getWeaponIdx()
    {
        return weaponIdx;
    }

    public int getSkinIdx()
    {
        return skinIdx;
    }

    public void setWeaponIdx(int i)
    {
        weaponIdx = i;
        updateSkinInfo();
    }

    public void setSkinIdx(int i)
    {
        skinIdx = i;
        updateSkinInfo();
    }

    public void open()
    {
        init();
        
        
        myRobo.sprite = GameController.Instance.getSkinSprite(GameController.Instance.getMySkinIdX());
        myWeapon.sprite = GameController.Instance.getWeaponSprite(GameController.Instance.getMyWeaponIdx());

        foreach (SotrageItemController item in itemList)
        {
            item.clearSelect();
        }

        updateBaseInfo();
        updateSkinInfo();
    }


    public void generateItems()
    {

        weaponIdx = GameController.Instance.getMyWeaponIdx();
        skinIdx = GameController.Instance.getMySkinIdX();
        
        foreach(Vector2 vec in list)
        {
            GameObject item = Instantiate(storageItemPrefab, container.transform);
            GameObject item2 = Instantiate(storageItemPrefab, vec.x == 1 ? containerSkin.transform : containerWeapon.transform);
            SotrageItemController controller = item.GetComponent<SotrageItemController>();
            SotrageItemController controller2 = item2.GetComponent<SotrageItemController>();
            controller.setSprite((int)vec.x, (int)vec.y);
            controller2.setSprite((int)vec.x, (int)vec.y);
            itemList.Add(controller);
            itemList.Add(controller2);
        }
        
    }
    
    public void reload()
    {
        list = GameController.Instance.getMyAssetList();
        RemoveAllChildren();
        generateItems();
    }

    public void RemoveAllChildren()
    {
        // Iterate through children in reverse order
        GameObject[] containers = { container, containerSkin, containerWeapon };
        for(int j=0;j<containers.Length;j++)
        {
            for (int i = containers[j].transform.childCount - 1; i >= 0; i--)
            {
                // Get the child Transform
                Transform child = containers[j].transform.GetChild(i);

                // Destroy the child's GameObject
                Destroy(child.gameObject);
            }
        }
        itemList.Clear();
        
    }


    public void save()
    {
        GameController.Instance.setMySkinId(skinIdx);
        GameController.Instance.setMyWeaponIdx(weaponIdx);
        GameController.Instance.closeAllPanel();
    }
    
}
