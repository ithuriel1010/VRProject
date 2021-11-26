using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{    
    public float speed = 1;
    public float gravity = 0.005f;
    public XRNode inputSource;
    public XRNode inputSource2;

    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;

    private float fallingSpeed;
    private XRRig rig;
    private Vector2 inputAxis;
    private Vector2 inputAxis2;

    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        InputDevice device2 = InputDevices.GetDeviceAtXRNode(inputSource2);
        device2.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis2);
    }

    private void FixedUpdate()
    {
        //CapsuleFolowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);

        float y = inputAxis2.y;
        
        if(inputAxis2.y == 0)
        {
            y = gravity;
        }

        Vector3 direction = headYaw * new Vector3(inputAxis.x, y, inputAxis.y);

        character.Move(direction*Time.fixedDeltaTime*speed);

        //gravity
        bool isGrounded = CheckIfGrounded();
        if (isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;
        
        //character.Move(Vector3.up * (gravity- inputAxis2.y) * Time.fixedDeltaTime);
    }

    //void CapsuleFolowHeadset()
    //{
    //    character.height = rig.cameraInRigSpaceHeight + additionalHeight;
    //    Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
    //    capsuleCenter = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    //}

    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }
}
