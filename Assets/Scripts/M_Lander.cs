using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

// ... : MonoBehaviour is a base class from which every Unity script derives. When you create a new C# script in Unity, it automatically inherits from MonoBehaviour, allowing it to be attached to GameObjects and participate in the Unity lifecycle (Start, Update, etc.).
public class M_Lander : MonoBehaviour
{

	// ---Instantiating Events---
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;


    // ---GameObject Components---
    // private/public type name
    private Rigidbody2D landerRigidbody2D;
    private BoxCollider2D landerBoxCollider2D;
    private Transform landerTransform;

	[SerializeField] private float fuelAmount = 10f;
	[SerializeField] private float fuelConsumption = 1f;

    [Header("Player Controls")]
		[SerializeField] private float upForce = 700f;
		[SerializeField] private float turnSpeed = 100f;

    [Header("Landing Parameters")]
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
    // the start method should be used to get commponent references to external game objects (components on gameobject that the script isnt direclty attached to)
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



		// Unless an input is pressed, turn off thruster visuals
		OnBeforeForce?.Invoke(this, EventArgs.Empty);

		// If were out of fuel, then leave this function : skips all the input code
		if(fuelAmount<=0)
		{
			Debug.Log("Out of Fuel :(");
			return;
		}

		// If any directional key is pressed, then set to true : When its true, fuel will be consumed
		bool isMoving = false;
		Debug.Log($"Fuel: {fuelAmount}");


        // New Input System
        // Up
        if (Keyboard.current.upArrowKey.IsPressed() || Keyboard.current.wKey.IsPressed())
        {
            Debug.Log("Up");
			// Lander is Moving
			isMoving = true;
			// Add directional Force
            landerRigidbody2D.AddForce(upForce * transform.up * Time.deltaTime);
			// ? - This just makes sure anything to the left is not null (the event exist)
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        // Left
        if (Keyboard.current.leftArrowKey.IsPressed() || Keyboard.current.aKey.IsPressed())
        {
            Debug.Log("Left");
			isMoving = true;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        // Right
        if (Keyboard.current.rightArrowKey.IsPressed() || Keyboard.current.dKey.IsPressed())
        {
            Debug.Log("Right");
			isMoving = true;
            landerRigidbody2D.AddTorque(-(turnSpeed) * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }

		// Use Fuel
		if(isMoving)
		{
			fuelAmount -= fuelConsumption * Time.deltaTime;
		}
    }


    // Landing Detection
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Debug.Log("Lander Collided");
        // Debug.Log(collision2D.relativeVelocity.magnitude);

        float landingScore = 100;
        float crashSpeed = collision2D.relativeVelocity.magnitude;
        float currentAngle = landerTransform.eulerAngles.z;
        float verticleOffset = Math.Abs(Math.Abs(currentAngle-180)-180);
        int maxSpeedEffectOnScore = 50;
        int maxAngleEffectOnScore = 50;

        // Check Surface
		/*
			Out Parameter Keyword
				Turns a non-bool method into a bool method, while still returning the intended value of the function
				In this case we dont need to use 'landingPadScript', but as seen in the comment, the variable name is 
				directly in the method call, and then can be used there after
		*/
        if (!collision2D.gameObject.TryGetComponent(out M_IdentifyLandingPad landingPadScript))
        {
			// landingPadScript.getScoreMultiplyer();
            PrintLanding(0.00f, collision2D, "Crash : Not a Landing Pad :(");
            return;
        }

        // Check Speed
        if (crashSpeed > safeLandingVelocity)
        {
            PrintLanding(0.00f, collision2D, "Crash : Landing was too Fast :(");
            return;
        }

        // Check Angle
        if (verticleOffset > safeLandingAngle)
        {
            PrintLanding(0.00f, collision2D, "Crash : Landing Angle is not Safe :(");
            return;
        }

        // Successfull Landing!
        landingScore -= crashSpeed / safeLandingVelocity * maxSpeedEffectOnScore;
        landingScore -= verticleOffset / safeLandingAngle * maxAngleEffectOnScore;
        // this varialbe is assigned during the check surface check
        landingScore *= landingPadScript.getScoreMultiplyer();
        PrintLanding(landingScore, collision2D, "Landed! : Landing was Safe :)");
        return;
    }


	// Built in Unity Method that is called when a trigger is entered
	private void OnTriggerEnter2D(Collider2D collider2d)
	{
		// If the trigger is fuelPickUp
		if(collider2d.gameObject.TryGetComponent(out M_FuelPickUp fuelPickUp))
		{
			OnFulePickUp(fuelPickUp);
		}	
	}

	private void OnFulePickUp(M_FuelPickUp fuelPickUp)
	{
		fuelAmount += fuelPickUp.getRefuelAmount();
		fuelPickUp.DestroySelf();
	}


	private void PrintLanding(float score, Collision2D collision2D, String result)
    {
        // Printing with Multiple Debug.Logs
        Debug.Log(result);
        Debug.Log($"    Score: {score.ToString("#.##")}");
        Debug.Log($"    Speed: {collision2D.relativeVelocity.magnitude.ToString("F2")}");
        Debug.Log($"    Angle: {VisualAngle().ToString("#.##")}");
        Debug.Log($"    Has Landing Pad Identifyer Class: {collision2D.gameObject.TryGetComponent(out M_IdentifyLandingPad landingPad)}");

        // Printing only 1 Debug.Log
        // Debug.Log(
        //     crashSpeed.ToString("#.##") + result + @"
        //     Speed: " + crashSpeed.ToString("F1")+ @"
        //     Angle: " + VisualAngle().ToString("F1")
        // );
    }


    private float VisualAngle()
    {
        if (landerTransform.eulerAngles.z > 180)
            return 360-landerTransform.eulerAngles.z;
        return landerTransform.eulerAngles.z;
    }

}
