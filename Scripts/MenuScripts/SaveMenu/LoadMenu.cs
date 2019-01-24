using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
	public bool _isLoadMenu = false;

	private string LOAD_MENU = "LOAD";
	private string SAVE_MENU = "SAVE";
	private string CONFIRM_LOAD = "ACCEPT";
	private string CONFIRM_SAVE = "SAVE YOUR PROGRESS";

	[SerializeField]
	private LoadButton btnPrefab;
	[SerializeField]
	private Transform listLoadButtonsPanel;

	[SerializeField]
	private Button btn;
	[SerializeField]
	private Transform spacer;
	[SerializeField]
	private Text titlePanel;
	[SerializeField]
	private Text buttonConfirm;

	private List<LoadButton> listLoadButtons = new List<LoadButton>();

	private void OnEnable()
	{
		LoadFileListing.updatedSaveList += UpdatedSaveList;
		if(_isLoadMenu)
		{
			titlePanel.text = LOAD_MENU;
			if(buttonConfirm != null)
				buttonConfirm.text = CONFIRM_LOAD;
			if(btn != null)
				btn.gameObject.SetActive(false);
			if(spacer != null)
				spacer.gameObject.SetActive(true);
		}
		else
		{
			titlePanel.text = SAVE_MENU;
			if(buttonConfirm != null)
				buttonConfirm.text = CONFIRM_SAVE;
			if(btn != null)
				btn.gameObject.SetActive(true);
			if(spacer != null)
				spacer.gameObject.SetActive(false);
		}
		GenerateListButtons();
	}

	private void OnDisable()
	{
		LoadFileListing.updatedSaveList -= UpdatedSaveList;
		ClearListsButtons();
	}

	public void ClearListsButtons()
	{
		int nbButtons = listLoadButtons.Count;
		listLoadButtons.Clear();
		SavingData.instance.LoadFilesListing.listPlayerData.Clear();
		for(int i = nbButtons - 1; i >= 0; --i)
		{
			Destroy(listLoadButtonsPanel.GetChild(i).gameObject);
		}
	}

	public void SetLoadMenu(bool isLoadMenu)
	{
		_isLoadMenu = isLoadMenu;
	}

	public void GenerateListButtons()
	{
		ClearListsButtons();
		SavingData.instance.LoadFilesListing.GetSaveFiles();
		List<PlayerData> playerDatas = SavingData.instance.LoadFilesListing.listPlayerData;
		int nbLoadButtons = playerDatas.Count;
		bool foundAutoSave = false;

		for(int i = 0; i < nbLoadButtons; ++i)
		{
			if(playerDatas[i].IsAutoSave && playerDatas[i].SavePlayerInfos.level == 0)
				continue;

			if(foundAutoSave && playerDatas[i].IsAutoSave && playerDatas[i].SavePlayerInfos.level > 0)
				continue;

			if(playerDatas[i].SavePlayerInfos.level < 0)
				continue;

			LoadButton newButton = Instantiate(btnPrefab, listLoadButtonsPanel);
			newButton.PlayerDataSaveState = playerDatas[i];
			newButton.SetInfosButton();
			newButton.saveNumber.text = playerDatas[i].IsAutoSave ? newButton.saveNumber.text : i.ToString();
			listLoadButtons.Add(newButton);

			if(playerDatas[i].IsAutoSave && playerDatas[i].SavePlayerInfos.level > 0)
				foundAutoSave = true;	
		}
	}

	private void UpdatedSaveList()
	{
		GenerateListButtons();
	}
}
