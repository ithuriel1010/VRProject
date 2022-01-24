//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR;

//public class RayToggle : MonoBehaviour
//{
//    private bool _leftTriggerDown;
//    private bool _leftGripDown;
//    // and other left hand buttons
 
//    private bool _rightTriggerDown;
//    private bool _rightGripDown;
//    // and other right hand buttons
//    public XRNode inputSource;


//    private void Update()
//    {
//        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
//        ProcessInputDeviceButton(device, InputHelpers.Button.MenuButton, ref _leftTriggerDown,
//            () => // On Button Down
//            {
//                Debug.Log("Left hand trigger down");
//                // Your functionality

//                if (gameObject.activeSelf == false)
//                {
//                    gameObject.SetActive(true);
//                }
//                else if (gameObject.activeSelf == true)
//                {
//                    gameObject.SetActive(false);
//                }

//            },
//            () => // On Button Up
//            {
//                Debug.Log("Left hand trigger up");
//            });
//    }
 
//private void ProcessInputDeviceButton(InputDevice inputDevice, InputHelpers.Button button, ref bool _wasPressedDownPreviousFrame, Action onButtonDown = null, Action onButtonUp = null, Action onButtonHeld = null)
//{
//    if (inputDevice.IsPressed(button, out bool isPressed) && isPressed)
//    {
//        if (!_wasPressedDownPreviousFrame) // // this is button down
//        {
//            onButtonDown?.Invoke();
//        }
 
//        _wasPressedDownPreviousFrame = true;
//        onButtonHeld?.Invoke();
//    }
//    else
//    {
//        if (_wasPressedDownPreviousFrame) // this is button up
//        {
//            onButtonUp?.Invoke();
//        }
 
//        _wasPressedDownPreviousFrame = false;
//    }
//}
//}


