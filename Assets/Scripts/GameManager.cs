using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Playing,
        Before,
        GameOver,
        Paused
    }

    public static GameState eGameStatus;
    public delegate void TrashHandler();
    public static event TrashHandler TrashThrownAway;

    public XRRayInteractor ray;
    public Canvas menu;
    public Canvas intro;
    public Canvas beforeGameMenu;
    public Canvas endGameMenu;
    public Transform prefab;

    public static int trashScore = 0;
    private bool _menuButtonDown;
    public XRNode inputSource;
    public Camera cam;

    public static GameManager instance = null;

    private float gameTime = 0.0f;
    private float gameEndTime = 10.0f;

    private int trashNumber;

    private void Awake()
    {
        if (instance == null)
            instance = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        eGameStatus = GameState.Intro;
        //ray.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        endGameMenu.gameObject.SetActive(false);
        beforeGameMenu.gameObject.SetActive(false);


        //OpenMenuFirst();
    }

    private void Update()
    {
        if(eGameStatus== GameState.Playing)
        {
            gameTime += Time.deltaTime;

            if(menu.gameObject.activeSelf==true)
            {
                ShowTimeInMenu(gameTime);
            }

            if(gameTime>= gameEndTime)
            {
                EndGame();
            }
        }
        MenuButtonControll();

    }
    public static void TrashDisposed()
    {
        if(eGameStatus == GameState.Playing)
        {
            trashScore += 1;
            TrashThrownAway();
            
        }
        else
        {
            Debug.Log("Not in play mode");
        }
    }

    public float GetTime()
    {
        return gameTime;
    }

    public void StartGame()
    {
        RestartPosition();
        gameTime = 0;
        trashScore = 0;

        intro.gameObject.SetActive(false);
        endGameMenu.gameObject.SetActive(false);
        
        //ray.gameObject.SetActive(false);
        eGameStatus = GameState.Before;
        trashNumber = GetRandomNumber(3,10);
        beforeGameMenu.gameObject.SetActive(true);
        ShowStartGameData(trashNumber);

    }

    public void StartSmallGame()
    {
        RandomTrash(trashNumber);

        beforeGameMenu.gameObject.SetActive(false);
        ray.gameObject.SetActive(false);        
        eGameStatus = GameState.Playing;
    }

    private int GetRandomNumber(int min, int max)
    {
        System.Random r = new System.Random();

        return r.Next(min, max);
    }

    private void RestartPosition()
    {
        var movement = FindObjectOfType<ContinuousMovement>();
        movement.ResetPosition();
    }

    public void EndGame()
    {
        //intro.gameObject.SetActive(false);
        //ray.gameObject.SetActive(false);

        if(menu.gameObject.activeSelf==true)
        {
            CloseMenu();
        }

        ShowEndGameMenu();

        var clones = GameObject.FindGameObjectsWithTag("TrashAndFish");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }

    private void MenuButtonControll()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        ProcessInputDeviceButton(device, InputHelpers.Button.MenuButton, ref _menuButtonDown,
            () => // On Button Down
            {
                //Debug.Log("Menu button down");

                if (ray.gameObject.activeSelf == false && menu.gameObject.activeSelf==false)
                {
                    OpenMenu();
                    ShowPointsInMenu();
                    ShowTimeInMenu(gameTime);

                }
                else if (ray.gameObject.activeSelf == true && menu.gameObject.activeSelf == true)
                {
                    CloseMenu();
                }

            },
            () => // On Button Up
            {
                //Debug.Log("Menu button up");
            });
    }


    private void ProcessInputDeviceButton(InputDevice inputDevice, InputHelpers.Button button, ref bool _wasPressedDownPreviousFrame, Action onButtonDown = null, Action onButtonUp = null, Action onButtonHeld = null)
    {
        if (inputDevice.IsPressed(button, out bool isPressed) && isPressed)
        {
            if (!_wasPressedDownPreviousFrame) // // this is button down
            {
                onButtonDown?.Invoke();
            }

            _wasPressedDownPreviousFrame = true;
            onButtonHeld?.Invoke();
        }
        else
        {
            if (_wasPressedDownPreviousFrame) // this is button up
            {
                onButtonUp?.Invoke();
            }

            _wasPressedDownPreviousFrame = false;
        }
    }

    private void CloseMenu()
    {
        ray.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        eGameStatus = GameState.Playing;
    }

    private void OpenMenu()
    {
        ray.gameObject.SetActive(true);

        Vector3 director = cam.transform.forward;
        Quaternion inverseRot = Quaternion.LookRotation(director);
        transform.rotation = inverseRot;
        Vector3 newPos = cam.transform.position + (director * 1);

        menu.gameObject.transform.position = newPos;
        menu.gameObject.transform.rotation = inverseRot;

        //menu.gameObject.transform.rotation = Quaternion.Euler(menu.gameObject.transform.rotation.x, menu.gameObject.transform.rotation.y, menu.gameObject.transform.rotation.z);


        //menu.gameObject.transform.rotation = Quaternion.Euler(inverseRot.x, inverseRot.y, inverseRot.z);


        menu.gameObject.SetActive(true);
        eGameStatus = GameState.Paused;

    }
    private void ShowPointsInMenu()
    {
        var canvasUpdate = FindObjectOfType<CanvasUpdate>();
        canvasUpdate.UpdateScore(trashScore, 0, trashNumber, 0);
    }

    private void ShowTimeInMenu(float time)
    {
        var canvasUpdate = FindObjectOfType<CanvasUpdate>();
        canvasUpdate.UpdateTime(time, gameEndTime);
    }

    private void ShowStartGameData(int trashNumber)
    {
        var canvasBegin = FindObjectOfType<Intro>();
        canvasBegin.UpdateTrashNumber(trashNumber);
    }

    private void RandomTrash(int trashNumber)
    {
        System.Random r = new System.Random();

        for (int i = 0; i < trashNumber; i++)
        {
            int x = r.Next(298, 305);
            int y = r.Next(45, 50);
            int z = r.Next(95, 100);

            Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);

            //Debug.Log(x + " " + y + " " + z);
        }
    }

    private void ShowEndGameMenu()
    {
        ray.gameObject.SetActive(true);

        Vector3 director = cam.transform.forward;
        Quaternion inverseRot = Quaternion.LookRotation(director);
        transform.rotation = inverseRot;
        Vector3 newPos = cam.transform.position + (director * 1);

        endGameMenu.gameObject.transform.position = newPos;
        endGameMenu.gameObject.transform.rotation = inverseRot;

        //menu.gameObject.transform.rotation = Quaternion.Euler(menu.gameObject.transform.rotation.x, menu.gameObject.transform.rotation.y, menu.gameObject.transform.rotation.z);


        //menu.gameObject.transform.rotation = Quaternion.Euler(inverseRot.x, inverseRot.y, inverseRot.z);


        endGameMenu.gameObject.SetActive(true);

        var canvasEnd = FindObjectOfType<Intro>();
        canvasEnd.ShowEndResults(trashScore, 0, trashNumber, 0);

        eGameStatus = GameState.GameOver;

    }

}
