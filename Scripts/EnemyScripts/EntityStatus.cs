using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityInfos
{
	public int UID;
	public string entityName;
	public float level;
<<<<<<< HEAD
	public EntityType entityType;
=======
	public string entityType;
>>>>>>> 29ae8ac92b7742914c4c477d588ef8ce27939288

	public float health;
	public float maxHealth;
	public float mana;
	public float maxMana;

	public SaveTransform entityPos;
}

public class EntityStatus : MonoBehaviour
{
	public EntityInfos entityInfos;

	public void Awake()
	{
		entityInfos.UID = gameObject.GetInstanceID();
	}

	public void SetEntityStatus(EntityInfos infosEntity)
	{
		entityInfos.entityName = infosEntity.entityName;
		entityInfos.level = (int)infosEntity.level;
		entityInfos.entityType = infosEntity.entityType;

		entityInfos.maxHealth = infosEntity.maxHealth;
		entityInfos.health = infosEntity.health;
		entityInfos.maxMana = infosEntity.maxMana;
		entityInfos.mana = infosEntity.mana;

		entityInfos.UID = infosEntity.UID;
	}

	public EntityInfos SetEntityInfos()
	{
		EntityInfos newEntityInfos = new EntityInfos();
		newEntityInfos.entityName = entityInfos.entityName;
		newEntityInfos.level = entityInfos.level;
		newEntityInfos.entityType = entityInfos.entityType;

		newEntityInfos.health = entityInfos.health;
		newEntityInfos.maxHealth = entityInfos.maxHealth;
		newEntityInfos.mana = entityInfos.mana;
		newEntityInfos.maxMana = entityInfos.maxMana;
		newEntityInfos.UID = entityInfos.UID;

		return newEntityInfos;
	}

	public void SetupEntityHealth(float healthAmount)
	{
		entityInfos.health = entityInfos.maxHealth = healthAmount;
	}

<<<<<<< HEAD
	public void SetupEntityType(EntityType entityType){
		bool result = System.Enum.IsDefined(typeof(EntityType), entityType);
		entityInfos.entityType = result ? entityType : EntityType.Undefined; 
	}

=======
>>>>>>> 29ae8ac92b7742914c4c477d588ef8ce27939288
	public void SetupEntityInfosByPlayerInfos(PlayerInfos playerInfos)
	{
		entityInfos.entityName = playerInfos.characterName;

<<<<<<< HEAD
=======
		entityInfos.entityType = playerInfos.characterType.ToString();
>>>>>>> 29ae8ac92b7742914c4c477d588ef8ce27939288
		entityInfos.health = playerInfos.health;
		entityInfos.mana = playerInfos.mana;
		entityInfos.maxHealth = playerInfos.maxHealth;
		entityInfos.maxMana = playerInfos.maxMana;
		entityInfos.level = playerInfos.level;
		entityInfos.entityPos = playerInfos.playerPos;
	}

	public void NotifyUI()
	{
		// notify UI only 
		if(EnemyUnitFrameManager.instance.entityInfos.UID == entityInfos.UID)
		{
			EnemyUnitFrameManager.instance.SetupEntityInfos(entityInfos);
			EnemyUnitFrameManager.instance.SetHealthText();
			EnemyUnitFrameManager.instance.FillHealthGauge();
			EnemyUnitFrameManager.instance.SetManaText();
			EnemyUnitFrameManager.instance.FillManaGauge();
		}
	}
}
