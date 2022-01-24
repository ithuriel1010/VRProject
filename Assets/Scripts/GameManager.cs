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
        GameOver
    }

    public static GameState eGameStatus;
    public delegate void TrashHandler();
    public static event TrashHandler TrashThrownAway;

    public XRRayInteractor ray;
    public Canvas menu;
    public static int playerScore = 0;
    private bool _menuButtonDown;
    public XRNode inputSource;
    public Camera cam;

    private void Start()
    {
        eGameStatus = GameState.Playing;
        ray.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
    }

    private void Update()
    {
        MenuButtonControll();
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

    private void MenuButtonControll()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        ProcessInputDeviceButton(device, InputHelpers.Button.MenuButton, ref _menuButtonDown,
            () => // On Button Down
            {
                Debug.Log("Menu button down");

                if (ray.gameObject.activeSelf == false && menu.gameObject.activeSelf==false)
                {
                    ray.gameObject.SetActive(true);

                    Vector3 director = cam.transform.forward;
                    Quaternion inverseRot = Quaternion.LookRotation(director);
                    transform.rotation = inverseRot;
                    Vector3 newPos = cam.transform.position + (director * 1);

                    menu.gameObject.transform.position = newPos;
                    menu.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);

                    menu.gameObject.SetActive(true);

                }
                else if (ray.gameObject.activeSelf == true && menu.gameObject.activeSelf == true)
                {
                    ray.gameObject.SetActive(false);
                    menu.gameObject.SetActive(false);
                }

            },
            () => // On Button Up
            {
                Debug.Log("Menu button up");
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
}
