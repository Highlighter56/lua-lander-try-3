using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M_StatsUI : MonoBehaviour
{
    // ---Game Object References---
	[Header("UI Elements")]
		[SerializeField] private TextMeshProUGUI statsTextMesh;
		[SerializeField] private GameObject leftArrow;
		[SerializeField] private GameObject rightArrow;
		[SerializeField] private GameObject upArrow;
		[SerializeField] private GameObject downArrow;
		[SerializeField] private float arrowRestBuffer = 0.5f;
		[SerializeField] private Image fuelImage;


	// ---Update---
	private void Update()
	{
		// ---Method Variables---
		float landerXSpeed = M_Lander.Instance.GetSpeedX();
		float landerYSpeed = M_Lander.Instance.GetSpeedY();


		// ---Update Stats---
		// -Fuel Bar-
		// fuelImage.fillAmount = M_Lander.Instance.GetFuelAmount() / M_Lander.Instance.GetFuelAmountMax();

		// -Text-
		statsTextMesh.text = 
			M_GameManager.Instance.GetScore() + "\n" +
			Mathf.Round(M_GameManager.Instance.GetTime()) + "\n" +
			Mathf.Abs(Mathf.Round(M_Lander.Instance.GetSpeedX() * 10f)) + "\n" +
			Mathf.Abs(Mathf.Round(M_Lander.Instance.GetSpeedY() * 10f)) + "\n" +
			M_Lander.Instance.GetFuelAmount().ToString("F2")
		;


		// ---Update Speed Arrows---

		// Clear Arrows before setting hte correct active ones
		ClearArrows();

		// -Horizontal-
		// Left
		if (landerXSpeed < -arrowRestBuffer)
		{
			leftArrow.SetActive(true);
		// Right
		} else if (landerXSpeed > arrowRestBuffer)
		{
			rightArrow.SetActive(true);
		} 


		// -Vertical-
		// Up
		if (landerYSpeed > arrowRestBuffer)
		{
			upArrow.SetActive(true);
		// Down
		} else if (landerYSpeed < -arrowRestBuffer)
		{
			downArrow.SetActive(true);
		} 

	}

	// ---General Methods---
	private void ClearArrows()
	{
		leftArrow.SetActive(false);
		rightArrow.SetActive(false);
		upArrow.SetActive(false);
		downArrow.SetActive(false);
		// Debug.Log("Maybe?");
	}

}
