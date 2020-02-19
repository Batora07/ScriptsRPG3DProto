using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
	public Text saveNumber;
	public Image[] characterSigils;
	public Text characterNameText;
	public Text characterClassText;
	public Text characterLevelText;

	public Text sceneNameText;
	public Text saveStateDateText;
	public Text saveName;

	[SerializeField]
	private bool isAutoSave;
	private Button btn;
	private int saveStateNumber = 0;

	private PlayerData playerDataSaveState;

	private void Awake()
	{
		btn = GetComponent<Button>();
	}

	private void OnEnable()
	{
		SetInfosButton();
		btn.onClick.AddListener(OnClickSaveBtn);
	}

	private void OnDisable()
	{
		btn.onClick.RemoveListener(OnClickSaveBtn);
	}

	private void OnClickSaveBtn()
	{
		// SET CURRENT SAVE STATE WITH THE PLAYER DATA SAVE STATE CONTAINED IN THIS BUTTON
		SavingData.instance.CurrentSavedState = playerDataSaveState;
	}

	public void SetInfosButton()
	{
		if(playerDataSaveState == null)
			return;

		if(playerDataSaveState.IsAutoSave)
		{
			isAutoSave = true;
			saveNumber.text = "AUTO";
			saveName.gameObject.SetActive(false);
		}
		else
		{
			saveNumber.text = saveStateNumber.ToString();
			saveName.gameObject.SetActive(true);
			saveName.text = playerDataSaveState.NameSave;
		}

		PlayableCharacter characterType = playerDataSaveState.SavePlayerInfos.typeCharacter;
		for(int i = 0; i < characterSigils.Length; ++i)
		{
			characterSigils[i].gameObject.SetActive(false);
		}

		switch(characterType)
		{
			case PlayableCharacter.Knight:
				characterSigils[0].gameObject.SetActive(true);
				break;
			case PlayableCharacter.VampireKing:
				characterSigils[1].gameObject.SetActive(true);
				break;
			case PlayableCharacter.Dryad:
				characterSigils[2].gameObject.SetActive(true);
				break;
		}

		characterNameText.text = playerDataSaveState.SavePlayerInfos.nameCharacter;
		characterClassText.text = playerDataSaveState.SavePlayerInfos.typeCharacter.ToString();
		characterLevelText.text = "Level "+playerDataSaveState.SavePlayerInfos.level;

		sceneNameText.text = playerDataSaveState.SavePlayerInfos.currentScene.ToString();
		saveStateDateText.text = playerDataSaveState.TimeOfSaveState.FormatDateString(playerDataSaveState);
	}

	public bool IsAutoSave
	{
		get
		{
			return isAutoSave;
		}

		set
		{
			isAutoSave = value;
		}
	}

	public PlayerData PlayerDataSaveState
	{
		get
		{
			return playerDataSaveState;
		}

		set
		{
			playerDataSaveState = value;
		}
	}
}
