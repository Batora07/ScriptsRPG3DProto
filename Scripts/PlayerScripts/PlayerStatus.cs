using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerInfos
{
	public string characterName;
	public float level;
	public PlayableCharacter characterType;

	public float health;
	public float maxHealth;
	public float mana;
	public float maxMana;

	public SaveTransform playerPos;
	public SaveTransform cameraPos;
	public float cameraFoV;
}

public class PlayerStatus : MonoBehaviour {

	public static PlayerStatus instance;

	public string characterName;
	public int level;
	public PlayableCharacter characterType;

	private PlayerHealth playerHealth;
	private PlayerMana playerMana;
	public GameObject[] playerSwords;
	public EntityStatus entityStatus;

	public SkillsListing skills;
	public SelectCharacter selectCharacter;

	public PlayerQuests playerQuests;

	private GameObject itemsPanel;

	public PlayerHealth PlayerHealth
	{
		get
		{
			return playerHealth;
		}

		set
		{
			playerHealth = value;
		}
	}

	public PlayerMana PlayerMana
	{
		get
		{
			return playerMana;
		}

		set
		{
			playerMana = value;
		}
	}

	public void Awake()
	{
		if(instance == null)
		{
			instance = this;
			SetupPlayerStatus();
		}
	}

	public void SetupPlayerStatus () {
		PlayerHealth = gameObject.GetComponent<PlayerHealth>();
		PlayerMana = gameObject.GetComponent<PlayerMana>();
		if(GameManager.instance != null)
		{
			GameManager.instance.PlayerStatus = this;
			characterName = GameManager.instance.CharacterName;
			characterType = (PlayableCharacter)GameManager.instance.SelectedCharacterIndex;
			level = GameManager.instance.LevelCharacter;
		}
		/*GameObject[] btns = GameObject.FindGameObjectsWithTag("SwordBtn");
		foreach(GameObject btn in btns)
		{
			btn.GetComponent<Button>().onClick.AddListener(ChangeSword);
		}*/

		itemsPanel = GameObject.Find("Items Panel");
		if(itemsPanel != null)
		{
			itemsPanel.SetActive(false);
		}

		PlayerInfos playerInfos = SetPlayerInfos();
		entityStatus = GetComponent<EntityStatus>();
		entityStatus.SetupEntityInfosByPlayerInfos(playerInfos);
		//GameObject.Find("Item").GetComponent<Button>().onClick.AddListener(ActivateItemsPanel);
	}
	
	public void ActivateItemsPanel()
	{
		if (itemsPanel.activeInHierarchy)
		{
			itemsPanel.SetActive(false);
		} else
		{
			itemsPanel.SetActive(true);
		}
	}

	public void ChangeSword(int newSwordIndex)
	{
		for (int i = 0; i < playerSwords.Length; i++)
		{
			playerSwords[i].SetActive(false);
		}
		// for a game with multiple items, we need to save the index in the array
		// of the item we need with aa game controller e.G, so that
		// we don't have to get this for loop 
		playerSwords[newSwordIndex].SetActive(true);
	}

	public void SetPlayerStatus(PlayerInfos infosPlayer)
	{
		characterName = infosPlayer.characterName;
		level = (int)infosPlayer.level;
		characterType = infosPlayer.characterType;

		playerHealth.maxHealth = infosPlayer.maxHealth;
		playerHealth.health = infosPlayer.health;
		playerMana.maxMana = infosPlayer.maxMana;
		playerMana.mana = infosPlayer.mana;
	}

	public PlayerInfos SetPlayerInfos()
	{
		PlayerInfos newPlayerInfos = new PlayerInfos();
		newPlayerInfos.characterName = characterName;
		newPlayerInfos.level = level;
		newPlayerInfos.characterType = characterType;

		newPlayerInfos.health = playerHealth.health;
		newPlayerInfos.maxHealth = playerHealth.maxHealth;
		newPlayerInfos.mana = playerMana.mana;
		newPlayerInfos.maxMana = playerMana.maxMana;
		return newPlayerInfos;
	}
}
