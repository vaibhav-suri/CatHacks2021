using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class ScoreCard : MonoBehaviour
{
    public Color[] colors = new Color[4];
    public GameObject[] players = new GameObject[4];
    public Text[] colorassignedtext = new Text[4];
    public GameObject[] colorassigned = new GameObject[4];
    public bool pauseflag;
    public GameObject winnerscreen;
    public GameObject loserscreen;

    //  Dictionary<string, int> scoretime;
    //   // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<4;i++) //scorecard text
        {
            colorassignedtext[i].text = "-"; 
          //      colorassigned[i].GetComponent<Image>().color=Color.white;
        }

    }
   
    void Playercatcher()//finds players in the level
    {
        players[0] = GameObject.Find("MainP");
        players[1] = GameObject.Find("Ai1");
        players[2] = GameObject.Find("Ai2");
        players[3] = GameObject.Find("Ai3");

    }
    void scorecatcher() //updates scores to players in the UI
    {
        Playercatcher();
        for (int i = 0; i < 4; i++)
        {
            if (players[i]!=null)
            {
                colorassignedtext[i].text = players[i].name + ": " + players[i].GetComponent<bl_ControllerExample>().followcount.ToString();
            }
            else
            {
                colorassignedtext[i].text = "DEAD";

            }
        }
    }
    void WinOrLose()  //Checks Enemy and Player Scores
    {
        if (!GameObject.Find("MainP").activeInHierarchy)
        {
            //lost 
            loserscreen.SetActive(true);
            Time.timeScale = 0;

        }
        if (GameObject.Find("Ai1") == null && GameObject.Find("Ai2") == null && GameObject.Find("Ai3") == null)
        {
            winnerscreen.SetActive(true);
            Time.timeScale = 0;

        }
    }
    // Update is called once per frame
    void Update()
    {
        scorecatcher();
        WinOrLose();

    }
}
