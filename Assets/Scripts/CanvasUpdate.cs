using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    public TextMesh ScoreText;
    public TextMesh Time;
    public TextMesh Points;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        //Intro.text = "Hello, nasza gra jest \n zajebista";
        Points.text = "Twój postęp:";
        //ScoreText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int trashScore, int fishScore, int trashGoal, int fishGoal)
    {
        //Points.text = "Twoje punkty:";
        string score = "Śmieci: " + trashScore + "/" + trashGoal + "\nRyby: " + fishScore + "/" + fishGoal;
        ScoreText.text = score;
    }

    public void UpdateTime(float time)
    {
        int roundedTime = (int)time;

        int gameEndTimeInSeconds = (int)GameManager.instance.gameEndTime;

        int secondsLeft = gameEndTimeInSeconds - roundedTime;
        int minutesLeft = secondsLeft / 60;
        int partialSeconds = secondsLeft % 60;

        string timeString = "Pozostały czas \n" + minutesLeft + ":" + partialSeconds;

        Time.text = timeString;
         
    }

}
