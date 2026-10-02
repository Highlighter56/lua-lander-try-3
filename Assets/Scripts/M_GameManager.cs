using System;
using UnityEngine;

public class M_GameManager : MonoBehaviour
{

	// Singleton Pattern
	public static M_GameManager Instance { get ; private set; }

	// ---Attributes---
	private int score;
	private float time;


	// ---Awake---
	private void Awake()
	{
		Instance = this;
	}

	// ---Start---
	private void Start()
	{
		/*
			Because there is only one lander in this game, we can access it through its Instance
				- (the Instance is somethign created in the Lander itself using hte principle of the singelton pattern)
		*/
		// This is the process of adding a method to an event (an event in nuity is the same thing as a signal in godot)
		M_Lander.Instance.OnCollectCoin += AddScore;
		M_Lander.Instance.OnLanding += AddScore;
	}

	// ---Update---
	private void Update()
	{
		time += Time.deltaTime;
	}


		// ---Getters
	public int GetScore()
	{
		return score;
	}
	public float GetTime()
	{
		return time;
	}


	// ---General Methods---
	public void AddScore(object sender, int amount)
	{
		score += amount;
		Debug.Log(score);
	}
	public void AddScore(object sender, M_Lander.OnLandingEventArgs e)
	{
		score += (int) e.score;
		Debug.Log(score);
	}

}
