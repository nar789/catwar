using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToUp(int idx)
    {
        if(idx == 0)
        {
            SceneManager.LoadScene("Intro");
        }
        else
        {
            GameController.Instance.goToHome();
            SceneManager.LoadScene("Stage");
        }
       
    }
}
