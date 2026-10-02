using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

// ... : MonoBehaviour is a base class from which every Unity script derives. When you create a new C# script in Unity, it automatically inherits from MonoBehaviour, allowing it to be attached to GameObjects and participate in the Unity lifecycle (Start, Update, etc.).
public class M_Lander : MonoBehaviour
{

	/*
		Singelton Pattern
		 - Useful for when you only have one of something in a scene
		inside the {  } you can set a public/private get and set. Not specifying (like for the get) means
		its public. Specifying (Like for the set) means its private
	*/
	public static M_Lander Instance { get; private set; }


	// ---Instantiating Events---
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
	public event EventHandler OnLeftRightForce;
	// How to attach a parameter to signals
	public event EventHandler<int> OnCollectCoin;
	// here we are attached a class to a signal, which contains data
	public event EventHandler<OnLandingEventArgs> OnLanding;
	// Creating a class/object that extends EventArgs, so we can pass parameters through signals
	public class OnLandingEventArgs : EventArgs
	{
		public float score;
	}
	


    // ---GameObject Components---
    // private/public type name
    private Rigidbody2D landerRigidbody2D;
    private BoxCollider2D landerBoxCollider2D;
    private Transform landerTransform;


	// ---Seralized Variables---
	[Header("Fuel Stats")]
		[SerializeField] private float fuelAmount;
		[SerializeField] private float fuelAmountMax = 10f;
		[SerializeField] private float fuelConsumptionRate = 1f;

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
		// Set Instance
		Instance = this;
		// Get Components
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerBoxCollider2D = GetComponent<BoxCollider2D>();
        landerTransform = GetComponent<Transform>();
		// Set Fuel
		fuelAmount = fuelAmountMax;
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

		// Method Variables
		// Its bad practice to have 'magic numbers'
		float onlyLeftRight = 0.5f;

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

		// Use Fuel
		if(LeftPressed() || UpPressed() || RightPressed())
		{
			// If any directional key is pressed, use fuel
			fuelAmount -= fuelConsumptionRate * Time.deltaTime;
		}


        // New Input System
		// Up
        if (UpPressed())
        {
            // Debug.Log("Up");
			// Add directional Force
            landerRigidbody2D.AddForce(upForce * transform.up * Time.deltaTime);
			// ? - This just makes sure anything to the left is not null (the event exist)
            OnUpForce?.Invoke(this, EventArgs.Empty);
			if (LeftPressed() && UpPressed() && RightPressed())
			{
				return;
			}
        }
		// Left & NOT_UP & Right
		if (LeftPressed() && !UpPressed() && RightPressed())
		{
			// Debug.Log("Left and Right");
			landerRigidbody2D.AddForce(onlyLeftRight * upForce * transform.up * Time.deltaTime);
			OnLeftRightForce?.Invoke(this, EventArgs.Empty);
			return;
		}
        // Left
        if (LeftPressed())
        {
            // Debug.Log("Left");
			// Add directional Torque
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        // Right
        if (RightPressed())
        {
            // Debug.Log("Right");
            landerRigidbody2D.AddTorque(-(turnSpeed) * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

	// Directional Input Methods
	private bool LeftPressed()
	{
		return Keyboard.current.leftArrowKey.IsPressed() || Keyboard.current.aKey.IsPressed();
	}
	private bool UpPressed()
	{
		return Keyboard.current.upArrowKey.IsPressed() || Keyboard.current.wKey.IsPressed() || Keyboard.current.spaceKey.IsPressed();
	}
	private bool RightPressed()
	{
		return Keyboard.current.rightArrowKey.IsPressed() || Keyboard.current.dKey.IsPressed();
	}

    // Landing Detection
	private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Debug.Log("Lander Collided");
        // Debug.Log(collision2D.relativeVelocity.magnitude);

		// Method Variables
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
            // PrintLanding(0.00f, collision2D, "Crash : Not a Landing Pad :(");
            return;
        }

        // Check Speed
        if (crashSpeed > safeLandingVelocity)
        {
            // PrintLanding(0.00f, collision2D, "Crash : Landing was too Fast :(");
            return;
        }

        // Check Angle
        if (verticleOffset > safeLandingAngle)
        {
            // PrintLanding(0.00f, collision2D, "Crash : Landing Angle is not Safe :(");
            return;
        }

        // Successfull Landing!
        landingScore -= crashSpeed / safeLandingVelocity * maxSpeedEffectOnScore;
        landingScore -= verticleOffset / safeLandingAngle * maxAngleEffectOnScore;
        // this varialbe is assigned during the check surface check
        landingScore *= landingPadScript.getScoreMultiplyer();
        // PrintLanding(landingScore, collision2D, "Landed! : Landing was Safe :)");
		
		// Invoke OnLanding Event
		OnLanding?.Invoke(this, new OnLandingEventArgs {score = landingScore} );
        return;
    }


	// Built in Unity Method that is called when a trigger is entered
	private void OnTriggerEnter2D(Collider2D collider2d)
	{
		// If the trigger is fuelPickUp
		if(collider2d.gameObject.TryGetComponent(out M_FuelPickUp fuelPickUp))
		{
			RefilFuel(fuelPickUp);
		}
		// If the trigger is CoinPickUp
		if(collider2d.gameObject.TryGetComponent(out M_CoinPickUp coin))
		{
			CollectCoin(coin);
		}
	}

	private void CollectCoin(M_CoinPickUp coin)
	{
		OnCollectCoin?.Invoke(this, coin.getValue());
		coin.DestroySelf();
	}

	private void RefilFuel(M_FuelPickUp fuelPickUp)
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

	// ---Getters---
	public float GetSpeedX()
	{
		return landerRigidbody2D.linearVelocityX;
	}
	public float GetSpeedY()
	{
		return landerRigidbody2D.linearVelocityY;
	}
	public float GetFuelAmount()
	{
		return fuelAmount;
	}
	public float GetFuelAmountMax()
	{
		return fuelAmountMax;
	}


    private float VisualAngle()
    {
        if (landerTransform.eulerAngles.z > 180)
            return 360-landerTransform.eulerAngles.z;
        return landerTransform.eulerAngles.z;
    }

}
