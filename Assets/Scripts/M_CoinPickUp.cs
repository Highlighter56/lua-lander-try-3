using UnityEditor.Search;
using UnityEngine;

public class M_CoinPickUp : MonoBehaviour
{
   
	// Arrtibutes
	[SerializeField] private int value = 10;


	public int getValue()
	{
		return value;
	}

	public void DestroySelf()
	{
		Destroy(gameObject);
	}
}
