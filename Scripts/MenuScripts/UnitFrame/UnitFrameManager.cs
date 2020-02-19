using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFrameManager : LivingEntityInfos
{
	public static UnitFrameManager instance;

	public Text userNameText;
	public Text characterTypeText;
	public Text lvlText;
	public Text healthText;
	public Text manaText;
	public Transform healthGauge;
	public Transform manaGauge;

	private PlayerStatus playerStatus;

	public void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	public void Start()
	{
		playerStatus = PlayerStatus.instance;
		CompleteSetInfosUnitFrame();
	}

	public void OnEnable()
	{
		CompleteSetInfosUnitFrame();
	}

	public void CompleteSetInfosUnitFrame()
	{
		SetUsername();
		SetLvl();
		SetCharacterType();
		FillHealthGauge();
		FillManaGauge();
		SetHealthText();
		SetManaText();
	}

	public void SetUsername()
	{
		if(playerStatus == null)
			return;

		if(string.IsNullOrEmpty(playerStatus.characterName))
			return;

		userNameText.text = playerStatus.characterName;
	}

	public void SetLvl()
	{
		if(playerStatus == null)
			return;

		lvlText.text = playerStatus.level.ToString();
	}

	public void SetCharacterType()
	{
		if(playerStatus == null)
			return;

		PlayableCharacter currentCharSelected = playerStatus.characterType;
		switch(currentCharSelected)
		{
			case PlayableCharacter.Dryad:
				characterTypeText.text = "Dryad Sorceress";
				break;
			case PlayableCharacter.Knight:
				characterTypeText.text = "Paladin Knight";
				break;
			case PlayableCharacter.VampireKing:
				characterTypeText.text = "Fallen King";
				break;
		}
	}

	public void FillHealthGauge()
	{
		if(playerStatus == null)
			return;

		currentHealth = playerStatus.PlayerHealth.health;
		maxHealth = playerStatus.PlayerHealth.maxHealth;
		float healthPercentage = currentHealth / maxHealth;
		healthGauge.GetComponent<Image>().fillAmount = healthPercentage;
	}

	public void FillManaGauge()
	{
		if(playerStatus == null)
			return;

		currentMana = playerStatus.PlayerMana.mana;
		maxMana = playerStatus.PlayerMana.maxMana;
		float manaPercentage = currentMana / maxMana;
		manaGauge.GetComponent<Image>().fillAmount = manaPercentage;
	}

	public void SetHealthText()
	{
		if(playerStatus == null)
			return;
		healthText.text = playerStatus.PlayerHealth.health.ToString() + "/" + playerStatus.PlayerHealth.maxHealth.ToString();
	}

	public void SetManaText()
	{
		if(playerStatus == null)
			return;
		manaText.text = playerStatus.PlayerMana.mana.ToString()+"/"+ playerStatus.PlayerMana.maxMana.ToString();
	}
}
