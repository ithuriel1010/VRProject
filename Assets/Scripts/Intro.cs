using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
}
