using UnityEngine;
using UnityEngine.InputSystem;

// ... : MonoBehaviour is a base class from which every Unity script derives. When you create a new C# script in Unity, it automatically inherits from MonoBehaviour, allowing it to be attached to GameObjects and participate in the Unity lifecycle (Start, Update, etc.).
public class M_Lander : MonoBehaviour
{

    // ---Class Variables---
    // private/public type name
    private Rigidbody2D landerRigidbody2D;
    private BoxCollider2D landerBoxCollider2D;
    [SerializeField] private float upForce = 700f;
    [SerializeField] private float turnSpeed = 100f;

    // awake is the first thing called 
    // the awake method should be used  to get references on local game objects (game objects that this script is attached to)
    private void Awake()
    {
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerBoxCollider2D = GetComponent<BoxCollider2D>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created / is called after awake
    // the start method should be used to get commponent references to externam game objects (components on gameobject that the script isnt direclty attached to)
    private void Start()
    {
        // Debug.Log("Start");
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


    // Update is called once per frame
    private void Update()
    {
        
    }
}
