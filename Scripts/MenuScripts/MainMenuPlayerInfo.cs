using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPlayerInfo : MonoBehaviour
{
	public Text nameCharacter;
	public Text levelCharacter;

	public void OnEnable()
	{
		UpdatePlayerInfos();
	}

	public void UpdatePlayerInfos()
	{
		nameCharacter.text = GameManager.instance.CharacterName;
		levelCharacter.text = "Level " + GameManager.instance.LevelCharacter.ToString();
	}
}
