using System;
using UnityEngine;

public class M_GameManager : MonoBehaviour
{

	// ---Attributes---
	private int score;


	// ---Start---
	private void Start()
	{
		/*
			Because there is only one lander in this game, we can access it through its Instance
				- (the Instance is somethign created in the Lander itself)
		*/
		M_Lander.Instance.OnCollectCoin += AddScore;
		M_Lander.Instance.OnLanding += AddScore;
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
