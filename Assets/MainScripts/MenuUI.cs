using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    bool audioflag;
   public Text audiotext;
    bool graphicsflag;
    public Text graphicstext;
    public GameObject menu;
    public GameObject loserscreen;
    bool menuflag;
    public GameObject[] particles = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        menuflag = false;
        audioflag = true;
        audiotext.text = "Sound ON";
          graphicstext.text="Graphics ON";
        menu.SetActive(false);
      
    }
    public void Pause() //paused by using timescale
    {
        menuflag = !menuflag;
        menu.SetActive(menuflag);
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void SoundC() //Sound muted by audio listener tweaking
    {
        audioflag = !audioflag;
        if (audioflag)
        {
            AudioListener.volume = 1f;
            audiotext.text = "Sound ON";

        }
        else
        {
            AudioListener.volume = 0f;
            audiotext.text = "Sound OFF";

        }
    }
    public void Graphics() 
    {
        graphicsflag = !graphicsflag;
        if (graphicsflag)
        {
            foreach(var item in particles)
            {
                item.SetActive(false);
                graphicstext.text = "Graphics OFF";

            }

        }
        else
        {

            foreach (var item in particles)
            {
                item.SetActive(true);
                graphicstext.text = "Graphics ON";

            }
        }
    }
    public void Reset()  //Reset reloads scene
    {
        SceneManager.LoadScene("Gameplay");
    }
    public void Home() //Takes back to home
    {
        SceneManager.LoadScene("Home");

    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
