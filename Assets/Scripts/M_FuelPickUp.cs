using UnityEngine;

public class M_FuelPickUp : MonoBehaviour
{
    

	[SerializeField] private float refuelAmount = 10f;


	public float getRefuelAmount()
	{
		return refuelAmount;
	}

	public void DestroySelf()
	{
		Destroy(gameObject);
	}

}
