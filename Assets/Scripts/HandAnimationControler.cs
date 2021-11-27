using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandAnimationControler : MonoBehaviour
{
    public InputDeviceCharacteristics controlerType;
    public InputDevice thisController;

    private Animator animatorController;
    private bool isControlerDetected = false;

    void Start()
    {

        Initialise();
        animatorController = GetComponent<Animator>();
    }

    void Initialise()
    {
        List<InputDevice> controlerDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controlerType, controlerDevices);

        if (controlerDevices.Count != 0)
        {
            thisController = controlerDevices[0];
            isControlerDetected = true;
        }
    }

    void Update()
    {
        if (!isControlerDetected)
            Initialise();
        else
        {

            if (thisController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.01f)
            {
                animatorController.SetFloat("Trigger", triggerValue);
            }

            //if (thisController.TryGetFeatureValue(CommonUsages.grip, out float gripValue) && gripValue > 0.1f)
            //{
            //    animatorController.SetFloat("Grip", gripValue);
            //}
        }

    }
}
