using UnityEngine;

public class M_LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThruster;
    [SerializeField] private ParticleSystem middleThruster;
    [SerializeField] private ParticleSystem rightThruster;

	// Script References
	private M_Lander landerScript;


	private void Awake()
	{
		// Get the Lander Script
		landerScript = GetComponent<M_Lander>();

		// Add a function Listener to the Landers OnUpForce Event
		landerScript.OnBeforeForce += Lander_OnBeforeForce;
		landerScript.OnUpForce += Lander_OnUpForce;
		landerScript.OnLeftForce += Lander_OnLeftForce;
		landerScript.OnRightForce += Lander_OnRightForce;
	}


	private void Lander_OnBeforeForce(object sender, System.EventArgs e)
	{
		SetParticleSystem(leftThruster, false);
		SetParticleSystem(rightThruster, false);
		SetParticleSystem(middleThruster, false);
	}
	private void Lander_OnUpForce(object sender, System.EventArgs e)
	{
		SetParticleSystem(leftThruster, true);
		SetParticleSystem(rightThruster, true);
		SetParticleSystem(middleThruster, true);
	}
	private void Lander_OnLeftForce(object sender, System.EventArgs e)
	{
		SetParticleSystem(rightThruster, true);
		SetParticleSystem(leftThruster, false);
	}
	private void Lander_OnRightForce(object sender, System.EventArgs e)
	{
		SetParticleSystem(leftThruster, true);
		SetParticleSystem(rightThruster, false);
	}

	// Turn On Off a Thruster
	private void SetParticleSystem(ParticleSystem particleSystem, bool state)
	{
		// For some reason this has to be split into two lines...
		ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
		emissionModule.enabled = state;
	}

}
