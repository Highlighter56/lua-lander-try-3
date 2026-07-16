using UnityEngine;
using UnityEngine.InputSystem;

// ... : MonoBehaviour is a base class from which every Unity script derives. When you create a new C# script in Unity, it automatically inherits from MonoBehaviour, allowing it to be attached to GameObjects and participate in the Unity lifecycle (Start, Update, etc.).
public class M_Lander : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Debug.Log("Start");
    }

    // Update is called once per frame
    private void Update()
    {
        // Debug.Log("Update");

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
        if (Keyboard.current.upArrowKey.IsPressed() || Keyboard.current.wKey.IsPressed())
        {
            Debug.Log("Up");
        }
        if (Keyboard.current.leftArrowKey.IsPressed() || Keyboard.current.aKey.IsPressed())
        {
            Debug.Log("Left");
        }
        if (Keyboard.current.rightArrowKey.IsPressed() || Keyboard.current.dKey.IsPressed())
        {
            Debug.Log("Right");
        }
        

    }


}
