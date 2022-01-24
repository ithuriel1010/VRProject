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
    public static int playerScore = 0;
    // and other right hand buttons
    private bool _leftTriggerDown;
    public XRNode inputSource;
    private void Start()
    {
        eGameStatus = GameState.Playing;
    }

    private void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        ProcessInputDeviceButton(device, InputHelpers.Button.MenuButton, ref _leftTriggerDown,
            () => // On Button Down
            {
                Debug.Log("Left hand trigger down");
                // Your functionality
                
                if (ray.gameObject.activeSelf == false)
                {
                    ray.gameObject.SetActive(true);
                }
                else if (ray.gameObject.activeSelf == true)
                {
                    ray.gameObject.SetActive(false);
                }

            },
            () => // On Button Up
            {
                Debug.Log("Left hand trigger up");
            });
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
