using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	public LayerMask enemyLayer;
	public float damage = 1f;
	public float radius = 0.3f;

	private EnemyHealth enemyHealth;
	private bool collided;

	public delegate void DamageDealtToEnemyEvent();
	public static event DamageDealtToEnemyEvent damageDealtToEnemy;

	// Update is called once per frame
	void Update () {
		CheckForDamage();
	}

	void CheckForDamage()
	{
		Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);

		foreach(Collider h in hits)
		{
			enemyHealth = h.GetComponent<EnemyHealth>();
			if (enemyHealth)
			{
				collided = true;
			}
		}

		if (collided)
		{
			DamageDealtToEnemy();
			collided = false;
			enemyHealth.TakeDamage(damage);
			// we only want to do damage to an enemy once
			gameObject.SetActive(false);
		}
	}

	public void DamageDealtToEnemy()
	{
		if(damageDealtToEnemy != null)
		{
			damageDealtToEnemy();
		}
	}
} // class
