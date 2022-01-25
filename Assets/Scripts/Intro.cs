using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public TextMesh TrashNumber;
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
    public void UpdateTrashNumber(int trashNumber)
    {
        TrashNumber.text = trashNumber.ToString();
    }
}
