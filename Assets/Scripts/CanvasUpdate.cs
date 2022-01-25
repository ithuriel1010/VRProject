using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUpdate : MonoBehaviour
{
    public TextMesh TrashNumber;
    public TextMesh ResultTrash;
    public TextMesh ResultFish;

    public TextMesh ScoreText;
    public TextMesh Time;
    public TextMesh Points;

    void Start()
    {
        //Points.text = "Twój postęp:";

    }

    void Update()
    {
        
    }

    public void OkIntro()
    {
        GameManager.instance.StartGame();
    }

    public void StartGameButton()
    {
        GameManager.instance.StartSmallGame();
    }
    public void UpdateTrashNumber(int trashNumber)
    {
        TrashNumber.text = trashNumber.ToString();
    }

    public void ShowEndResults(int trashScore, int fishScore, int trashGoal, int fishGoal)
    {
        ResultTrash.text = trashScore + "/" + trashGoal;
        ResultFish.text = fishScore + "/" + fishGoal;
    }

    public void UpdateScore(int trashScore, int fishScore, int trashGoal, int fishGoal)
    {
        //Points.text = "Twoje punkty:";
        string score = "Śmieci: " + trashScore + "/" + trashGoal + "\nRyby: " + fishScore + "/" + fishGoal;
        ScoreText.text = score;
    }

    public void UpdateTime(float time, float endTime)
    {
        int roundedTime = (int)time;

        int gameEndTimeInSeconds = (int)endTime;

        int secondsLeft = gameEndTimeInSeconds - roundedTime;
        int minutesLeft = secondsLeft / 60;
        int partialSeconds = secondsLeft % 60;

        string timeString = "Pozostały czas \n" + minutesLeft + ":" + partialSeconds;

        Time.text = timeString;

    }
}
