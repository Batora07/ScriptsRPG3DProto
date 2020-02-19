using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSaveButton : MonoBehaviour
{
	[SerializeField]
	private Button btnSave;
	[SerializeField]
	private Button validateSaveName;
	[SerializeField]
	private InputField inputFieldNameSave;
	[SerializeField]
	private Transform panelConfirmSaveState;
	[SerializeField]
	private LoadMenu loadMenu;

	private bool isPanelEnabled = false;

	private void OnEnable()
	{
		if(!loadMenu._isLoadMenu)
		{
			if(btnSave != null)
			{
				btnSave.onClick.AddListener(OpenPopupConfirmSave);
			}

			if(validateSaveName != null)
			{
				validateSaveName.onClick.AddListener(CreateTheSaveState);
			}
		}
	}

	private void OnDisable()
	{
		btnSave.onClick.RemoveAllListeners();
		validateSaveName.onClick.RemoveAllListeners();
	}

	private void OpenPopupConfirmSave()
	{
		isPanelEnabled = !isPanelEnabled;
		panelConfirmSaveState.gameObject.SetActive(isPanelEnabled);
	}

	private void CreateTheSaveState()
	{
		// set the name of the save with the input of the user
		if(!string.IsNullOrEmpty(inputFieldNameSave.text))
		{
			CreateSaveState newSaveState = new CreateSaveState(inputFieldNameSave.text);
		}
		// take the time when the save has been made has a name for the savefile
		else
		{
			CreateSaveState newSaveState = new CreateSaveState();
		}
		// Once the saveState is created, reupdate the list -> trigger event
		SavingData.instance.LoadFilesListing.UpdatedSaveList();
		panelConfirmSaveState.gameObject.SetActive(false);
	}
}
