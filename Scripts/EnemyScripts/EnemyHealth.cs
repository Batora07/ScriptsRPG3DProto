using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public float health = 100f;
	public float dissolveFXTime = 3f;
	public float deathAnimTime = 4f;
	public GameObject deadFX;
	public Dissolve[] dissolveFX;
	public Animator anim;
	public bool isDead = false;
<<<<<<< HEAD
=======

	private EnemyAISystem enemyAI;

	public EntityStatus entityStatus;

	private void Awake()
	{
		entityStatus = gameObject.GetComponent<EntityStatus>();
		enemyAI = gameObject.GetComponent<EnemyAISystem>();
		entityStatus.SetupEntityHealth(health);
	}
>>>>>>> 29ae8ac92b7742914c4c477d588ef8ce27939288

	private EnemyAISystem enemyAI;

	public EntityStatus entityStatus;

	public delegate void KilledEntity(EntityType enemyType);
	public static event KilledEntity entityKilled;

	private void Awake()
	{
		entityStatus = gameObject.GetComponent<EntityStatus>();
		enemyAI = gameObject.GetComponent<EnemyAISystem>();
		entityStatus.SetupEntityHealth(health);
		entityStatus.SetupEntityType(enemyAI.entityType);
	}
	
	public void TakeDamage(float damageAmount)
	{
		if (health - damageAmount < 1)
		{
			isDead = true;
			UpdateHealtEntityStatus(0);
			// DESTROY THE ENEMY
			//  State to idle then
			SetToIdleState();
<<<<<<< HEAD

			EntityKilled(entityStatus.entityInfos.entityType);
=======
>>>>>>> 29ae8ac92b7742914c4c477d588ef8ce27939288
			DisableVFX();
			anim.SetBool(EnemyAIState.DeathState.ToString(), true);
			// make the death anim
			StartCoroutine(WaitForEndDeathAnimation());
		}
		else
		{
			SoundManager.instance.RandomizeSfx(enemyAI.SfxAudio, enemyAI.BodyHitSounds);
			SoundManager.instance.RandomizeSfx(enemyAI.VoiceAudio, enemyAI.HitSounds);
			health -= damageAmount;
			UpdateHealtEntityStatus(health);
		}
	}

	void SetToIdleState()
	{
		anim.SetBool(EnemyAIState.Attack1.ToString(), false);
		anim.SetBool(EnemyAIState.Attack2.ToString(), false);
		anim.SetBool(EnemyAIState.Run.ToString(), false);
	}
<<<<<<< HEAD

	IEnumerator WaitForEndDeathAnimation()
	{
		SoundManager.instance.RandomizeSfx(enemyAI.VoiceAudio, enemyAI.DeathSounds);
		yield return new WaitForSeconds(deathAnimTime);
		//  Enable the dissolve effect
		StartCoroutine(WaitForEndFX());
	}

	IEnumerator WaitForEndFX()
	{
		foreach(Dissolve dissolve in dissolveFX)
		{
			dissolve.enabled = true;
		}
		
		yield return new WaitForSeconds(dissolveFXTime);
		// Instantiate the loot at this position then destroy the current G.O
		Destroy(gameObject);
	}

	public void UpdateHealtEntityStatus(float health)
	{
		entityStatus.entityInfos.health = health;
		entityStatus.NotifyUI();
	}


	public void DisableVFX()
	{
		if(GetComponent<EnemyAISystem>().leftAttackPoint.GetComponent<EnableVFX>() != null)
		{
			GetComponent<EnemyAISystem>().leftAttackPoint.GetComponent<EnableVFX>().DisableVFX();
		}
		if(GetComponent<EnemyAISystem>().rightAttackPoint.GetComponent<EnableVFX>() != null)
		{
			GetComponent<EnemyAISystem>().rightAttackPoint.GetComponent<EnableVFX>().DisableVFX();
=======

	IEnumerator WaitForEndDeathAnimation()
	{
		SoundManager.instance.RandomizeSfx(enemyAI.VoiceAudio, enemyAI.DeathSounds);
		yield return new WaitForSeconds(deathAnimTime);
		//  Enable the dissolve effect
		StartCoroutine(WaitForEndFX());
	}

	IEnumerator WaitForEndFX()
	{
		foreach(Dissolve dissolve in dissolveFX)
		{
			dissolve.enabled = true;
>>>>>>> 29ae8ac92b7742914c4c477d588ef8ce27939288
		}
		
		yield return new WaitForSeconds(dissolveFXTime);
		// Instantiate the loot at this position then destroy the current G.O
		Destroy(gameObject);
	}

	public void UpdateHealtEntityStatus(float health)
	{
		entityStatus.entityInfos.health = health;
		entityStatus.NotifyUI();
	}


<<<<<<< HEAD
	private void EntityKilled(EntityType entityType)
	{
		if(entityKilled != null)
		{
			entityKilled(entityType);
			StatsManager.enemyDefeated++;
			//Debug.Log(entityType + " killed");
		}
	}
=======
	public void DisableVFX()
	{
		if(GetComponent<EnemyAISystem>().leftAttackPoint.GetComponent<EnableVFX>() != null)
		{
			GetComponent<EnemyAISystem>().leftAttackPoint.GetComponent<EnableVFX>().DisableVFX();
		}
		if(GetComponent<EnemyAISystem>().rightAttackPoint.GetComponent<EnableVFX>() != null)
		{
			GetComponent<EnemyAISystem>().rightAttackPoint.GetComponent<EnableVFX>().DisableVFX();
		}
	}

>>>>>>> 29ae8ac92b7742914c4c477d588ef8ce27939288
}// class
