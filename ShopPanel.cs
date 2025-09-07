using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buySpeaker()
    {
        Debug.Log("buySpeaker");
        GameController.Instance.buySpeakerByStore();
        GameController.Instance.closeAllPanel();
        GameController.Instance.showToast("냥이 퇴치 스피커가 획득되었습니다!", 1);
        
    }

    public void buyGold()
    {
        Debug.Log("buyGold");
        GameController.Instance.earnGold(30000);
        GameController.Instance.closeAllPanel();
        GameController.Instance.showToast("30,000골드가 증정되었습니다!", 1);
    }
}
