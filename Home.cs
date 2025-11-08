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
        
        if (idx == 0)
        {
            SceneManager.LoadScene("Intro");
        }
        else
        {

            GameController.Instance.playClickAudio();
            GameController.Instance.openPanel(5);
        }
       
    }

    public void onPauseBtn()
    {
        GameController.Instance.playClickAudio();
        GameController.Instance.pauseLastAudio();
        GameController.Instance.setIsPause(true);
        Time.timeScale = 0;
        
    }

    public void onPlayBtn()
    {
        GameController.Instance.playClickAudio();
        GameController.Instance.playLastAudio();
        GameController.Instance.setIsPause(false);
        Time.timeScale = 1;
    }
}
