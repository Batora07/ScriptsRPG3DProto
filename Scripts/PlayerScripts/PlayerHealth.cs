using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public float health = 100f;
	public float maxHealth = 100f;
	public float deathAnimSeconds = 2;
	public bool isDead = false;
	public float dissolveFXTime = 3f;
	public Dissolve[] dissolveFX;
	public GameObject deadFX;

	public delegate void SendDeathEvent();
	public static event SendDeathEvent deathEvent;

	private CharacterMovement playerMovement;
	private AudioClip deathSound;
	private string deathMusic = "DeathMusic";

	private void Awake()
	{
		playerMovement = GetComponent<CharacterMovement>();
	}

	public void Start()
	{
		PlayerStatus.instance.PlayerHealth = this;
	}

	public void TakeDamage(float damageAmount)
	{
		if (health > 0 && !isDead)
		{
			SoundManager.instance.RandomizeSfx(playerMovement.SfxAudio, playerMovement.BodyHitSounds);
			SoundManager.instance.RandomizeSfx(playerMovement.VoiceAudio, playerMovement.HitSounds);
			health -= damageAmount;
			NotifyUI();
		}
		else
		{
			health = 0;
			isDead = true;
			SendDeath();
			NotifyUI();
			//KILL THE PLAYER
			//Instantiate(deadFX, transform.position, Quaternion.identity);
			StartCoroutine(WaitForDeathAnim());	
		}
	}

	public void HealingHealth(float healAmount)
	{
		if(health + healAmount > maxHealth)
		{
			health = maxHealth;
		}
		else
		{
			health += healAmount;
		}
	}

	private void DeathAnim()
	{
		Animator anim = gameObject.GetComponent<CharacterMovement>().Anim;
		MovementMotor motor = gameObject.GetComponent<CharacterMovement>().Motor;
		motor.Stop();
		anim.SetInteger("State", 5);
	}

	private IEnumerator WaitForDeathAnim()
	{
		DeathAnim();
		SoundManager.instance.RandomizeSfx(playerMovement.VoiceAudio, playerMovement.DeathSounds);
		yield return new WaitForSeconds(deathAnimSeconds);
		foreach(Dissolve dissolve in dissolveFX)
		{
			dissolve.enabled = true;
		}
		yield return new WaitForSeconds(dissolveFXTime);
		DisplayDeathScreen();
		Destroy(gameObject);
	}

	public void NotifyUI()
	{
		// notify UI
		UnitFrameManager.instance.SetHealthText();
		UnitFrameManager.instance.FillHealthGauge();

		EntityStatus entityStatus = GetComponent<EntityStatus>();
		entityStatus.entityInfos.health = health;
		entityStatus.NotifyUI();
	}

	public void SendDeath()
	{
		if(deathEvent != null)
		{
			deathEvent();
		}
	}

	public void DisplayDeathScreen()
	{
		int nbSounds = SoundManager.instance.listAudioTypes.Length;
		int nbFullSounds = SoundManager.instance.soundObject.sounds.Length;

		for(int i = 0; i < nbFullSounds; ++i)
		{
			if(SoundManager.instance.soundObject.sounds[i].scene == SceneName.Common)
			{
				int nbCommonSounds = SoundManager.instance.soundObject.sounds[i].sounds.Length;

				for (int j = 0; j < nbCommonSounds; ++j)
				{
					if(SoundManager.instance.soundObject.sounds[i].sounds[j].name == deathMusic)
					{
						deathSound = SoundManager.instance.soundObject.sounds[i].sounds[j].trackFile;
					}
				}
			}
		}

		for(int i = 0; i < nbSounds; ++i)
		{
			if(SoundManager.instance.listAudioTypes[i].audioType == AudioType.Music)
			{
				if(deathSound != null)
				{
					SoundManager.instance.listAudioTypes[i].clipToPlay = deathSound;
				}
			}
		}

		PauseMenuUI.instance.EnablePauseMenu();
	}

}// class
