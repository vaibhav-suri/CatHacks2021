using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OpeningPageUI : MonoBehaviour
{
    public GameObject howto;
    bool howtoflag;
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    // Start is called before the first frame update
    public void Quit() //Quit APP
    {
        Application.Quit();
    }
    public void Help() //Prompt to show instructions
    {
        howtoflag = !howtoflag;
        howto.SetActive(howtoflag);
    }
    void Start()
    {

        howtoflag = false;
        howto.SetActive(howtoflag);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
