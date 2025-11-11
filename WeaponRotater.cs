using UnityEngine;

public class WeaponRotater : MonoBehaviour
{

    
    bool isStart = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speed = isStart ? 500 : 50;
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
        
    }

    public void startRotate()
    {
        isStart = true;
    }

    public void stopRotate()
    {
        isStart = false;
    }


}
