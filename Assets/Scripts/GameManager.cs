using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Playing,
        GameOver
    }

    public static GameState eGameStatus;
    public delegate void TrashHandler();
    public static event TrashHandler TrashThrownAway;

    public static int playerScore = 0;

    private void Start()
    {
        eGameStatus = GameState.Playing;
    }
    public static void TrashDisposed()
    {
        if(eGameStatus == GameState.Playing)
        {
            playerScore += 1;
            TrashThrownAway();
        }
        else
        {
            Debug.Log("Not in play mode");
        }
    }
}
