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

	private EnemyAISystem enemyAI;

	public EntityStatus entityStatus;

	private void Awake()
	{
		entityStatus = gameObject.GetComponent<EntityStatus>();
		enemyAI = gameObject.GetComponent<EnemyAISystem>();
		entityStatus.SetupEntityHealth(health);
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
		}
	}

}// class
