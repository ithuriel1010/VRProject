using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.TrashThrownAway += DisplayScore;
    }

    private void OnDisable()
    {
        GameManager.TrashThrownAway -= DisplayScore;
    }

    private void DisplayScore()
    {
        Debug.Log("Score: " + GameManager.playerScore);
    }
}
