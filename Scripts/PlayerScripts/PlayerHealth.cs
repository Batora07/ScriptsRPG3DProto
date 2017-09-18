using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public float health = 100f;
	public GameObject deadFX;

	public void TakeDamage(float damageAmount)
	{
		health -= damageAmount;
		print("Urgh I got hurt !");

		if (health <= 0)
		{
			//KILL THE PLAYER
			Instantiate(deadFX, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

}// class
