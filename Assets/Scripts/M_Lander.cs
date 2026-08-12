using System;
using UnityEngine;
using UnityEngine.InputSystem;

// ... : MonoBehaviour is a base class from which every Unity script derives. When you create a new C# script in Unity, it automatically inherits from MonoBehaviour, allowing it to be attached to GameObjects and participate in the Unity lifecycle (Start, Update, etc.).
public class M_Lander : MonoBehaviour
{

    // ---Class Variables---
    // private/public type name
    private Rigidbody2D landerRigidbody2D;
    private BoxCollider2D landerBoxCollider2D;
    private Transform landerTransform;
    [SerializeField] private float upForce = 700f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private float safeLandingVelocity = 4f;
    [SerializeField, Range(0, 180)] private float safeLandingAngle = 10f;

    // awake is the first thing called 
    // the awake method should be used  to get references on local game objects (game objects that this script is attached to)
    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerBoxCollider2D = GetComponent<BoxCollider2D>();
        landerTransform = GetComponent<Transform>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created / is called after awake
    // the start method should be used to get commponent references to externam game objects (components on gameobject that the script isnt direclty attached to)
    private void Start()
    {
        // Debug.Log("Start");
    }


    // Update is called once per frame
    private void Update()
    {
        // Debug.Log("Update");
        // Debug.Log(transform.eulerAngles);
    }


    // This is a special Update() function that is called at a fixed interval, and is where all physics code should live
    private void FixedUpdate()
    {
        // ---Detecting Key Board Input---
        /*
        By defualt the code is set to use the New Input System. But there is also a Legacy Input Manager that 
        can be used if enabeled in project settings.
        */

        // Legacy Input Manager
        // if (Input.GetKey(KeyCode.UpArrow))
        // {
        //     Debug.Log("Up");
        // }

        // New Input System
        // Up
        if (Keyboard.current.upArrowKey.IsPressed() || Keyboard.current.wKey.IsPressed())
        {
            // Debug.Log("Up");
            landerRigidbody2D.AddForce(upForce * transform.up * Time.deltaTime);
        }
        // Left
        if (Keyboard.current.leftArrowKey.IsPressed() || Keyboard.current.aKey.IsPressed())
        {
            // Debug.Log("Left");
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
        // Right
        if (Keyboard.current.rightArrowKey.IsPressed() || Keyboard.current.dKey.IsPressed())
        {
            // Debug.Log("Right");
            landerRigidbody2D.AddTorque(-(turnSpeed) * Time.deltaTime);
        }
    }


    // Landing Detection
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Debug.Log("Lander Collided");
        // Debug.Log(collision2D.relativeVelocity.magnitude);

        // Message
        String result = " : You Crashed :(";
        // Speed
        float crashSpeed = collision2D.relativeVelocity.magnitude;

        
        if (crashSpeed < safeLandingVelocity && isSafeLandingAngle())
        {
            result = " : Safe Landing :)";
        }
      
        Debug.Log(crashSpeed.ToString("#.##") + result);
        Debug.Log("Speed: "+crashSpeed.ToString("F1"));
        Debug.Log("Angle: "+visualAngle().ToString("F1"));

    }

    private bool isSafeLandingAngle()
    {
        float currentAngle = landerTransform.eulerAngles.z;
        if (currentAngle <= safeLandingAngle || 360-currentAngle < safeLandingAngle)
            return true;
        return false;
    }

    private float visualAngle()
    {
        if (landerTransform.eulerAngles.z > 180)
            return 360-landerTransform.eulerAngles.z;
        return landerTransform.eulerAngles.z;
    }

}
