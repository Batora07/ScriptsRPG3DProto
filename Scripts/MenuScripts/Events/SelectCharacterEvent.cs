using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterEvent : MonoBehaviour
{
	private Button btn;

	private void Start()
	{
		btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(SendCharacterSelectionToGameManager);
	}

	public void OnDisable()
	{
		btn.onClick.RemoveListener(SendCharacterSelectionToGameManager);
	}

	public void SendCharacterSelectionToGameManager()
	{
		PlayableCharacter characterSelected = gameObject.GetComponent<CharacterType>().characterType;
		GameManager.instance.SelectedCharacterIndex = (int)characterSelected;
	}
}