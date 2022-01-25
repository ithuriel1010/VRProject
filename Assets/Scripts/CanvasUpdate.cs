using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdate : MonoBehaviour
{
    public TextMesh ScoreText;
    public TextMesh Intro;
    public TextMesh Points;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        //Intro.text = "Hello, nasza gra jest \n zajebista";
        Points.text = "Twoje punkty:";
        //ScoreText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        //Points.text = "Twoje punkty:";
        ScoreText.text = score.ToString();
    }

    public void StartGame()
    {
        Intro.text = "";
        Destroy(button);
        GameManager.instance.StartGame();        
    }

}
