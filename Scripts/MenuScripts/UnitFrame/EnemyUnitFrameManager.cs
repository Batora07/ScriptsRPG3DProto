using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnitFrameManager : MonoBehaviour
{
	public static EnemyUnitFrameManager instance;

	public Text userName;
	public Text characterType;
	public Text lvlText;
	public Text healthText;
	public Text manaText;
	public Transform panelUI;
	public Transform healthGauge;
	public Transform manaGauge;

	public Color enemyHealthTint;
	public Color playerHealthTint;
	public Color enemyTypeTint;
	public Color playerTypeTint;

	public EntityInfos entityInfos;

	public void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	/*public void Start()
	{
		CompleteSetInfosUnitFrame();
	}*/

	public void EnablePanel()
	{
		CompleteSetInfosUnitFrame();
		panelUI.gameObject.SetActive(true);
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
		if(entityInfos.level == 0)
			return;

		if(string.IsNullOrEmpty(entityInfos.entityName))
			return;

		userName.text = entityInfos.entityName;
	}

	public void SetLvl()
	{
		if(entityInfos.level == 0)
			return;

		lvlText.text = entityInfos.level.ToString();
	}

	public void SetCharacterType()
	{
		if(entityInfos.level == 0)
			return;

		characterType.text = entityInfos.entityType;
	}

	public void FillHealthGauge()
	{
		if(entityInfos.level == 0)
			return;

		float healthPercentage = entityInfos.health / entityInfos.maxHealth;
		healthGauge.GetComponent<Image>().fillAmount = healthPercentage;
	}

	public void FillManaGauge()
	{
		if(entityInfos.level == 0)
			return;

		float manaPercentage = entityInfos.mana / entityInfos.maxMana;
		manaGauge.GetComponent<Image>().fillAmount = manaPercentage;
	}

	public void SetHealthText()
	{
		if(entityInfos.level == 0)
			return;
		healthText.text = entityInfos.health.ToString() + "/" + entityInfos.maxHealth.ToString();
	}

	public void SetManaText()
	{
		if(entityInfos.level == 0)
			return;
		manaText.text = entityInfos.mana.ToString() + "/" + entityInfos.maxMana.ToString();
	}

	public void SetupEntityInfos(EntityInfos _entityInfos)
	{
		entityInfos.UID = _entityInfos.UID;
		entityInfos.entityName = _entityInfos.entityName;
		entityInfos.level = _entityInfos.level;
		entityInfos.entityType = _entityInfos.entityType;
		entityInfos.health = _entityInfos.health;
		entityInfos.maxHealth = _entityInfos.maxHealth;
		entityInfos.maxMana = _entityInfos.maxMana;
		entityInfos.maxMana = _entityInfos.mana;
	}

	public void SetHealthBarColor(LivingObjectStatus status)
	{
		switch(status)
		{
			case LivingObjectStatus.enemy:
				healthGauge.GetComponent<Image>().color = enemyHealthTint;
				break;
			case LivingObjectStatus.player:
				healthGauge.GetComponent<Image>().color = playerHealthTint;
				break;
		}
	}


	public void SetCharacterTypePlayer()
	{
		try
		{
			PlayableCharacter typePlayer = (PlayableCharacter)System.Enum.Parse(typeof(PlayableCharacter), entityInfos.entityType);

			switch(typePlayer)
			{
				case PlayableCharacter.Dryad:
					characterType.text = "Dryad Sorceress";
					characterType.color = playerTypeTint;
					break;
				case PlayableCharacter.Knight:
					characterType.text = "Paladin Knight";
					characterType.color = playerTypeTint;
					break;
				case PlayableCharacter.VampireKing:
					characterType.text = "Fallen King";
					characterType.color = playerTypeTint;
					break;
			}
		}
		catch(System.Exception e)
		{
			// Debug.Log("not a player");
			// this is not a player character selected, then set the color of the type text to enemy
			characterType.color = enemyTypeTint;
		}
	}
}
