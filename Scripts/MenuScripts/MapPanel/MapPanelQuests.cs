using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanelQuests : MonoBehaviour
{
	public static MapPanelQuests instance;

	public Text currentQuestTitle;
	public Text currentQuestDescShort;

	public Text buttonQuestTitleText;

	public void Awake()
	{
		MakeSingleton();
		QuestsManager.currentQuestUpdated += UpdateCurrentQuestTexts;
		UpdateCurrentQuestTexts();
	}

	void MakeSingleton()
	{		
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;		
		}
	}


	public void OnDestroy()
	{
		QuestsManager.currentQuestUpdated -= UpdateCurrentQuestTexts;
	}

	public void UpdateCurrentQuestTexts()
	{
		if(QuestsManager.instance._currentQuest != null)
		{
			currentQuestTitle.text = QuestsManager.instance._currentQuest.quest_ShortTitle;

			switch(QuestsManager.instance._currentQuest.questStatus)
			{
				case QuestStatus.Current:
				case QuestStatus.InProgress:
					currentQuestDescShort.text = ObjectiveCount() + "\n" + QuestsManager.instance._currentQuest.quest_ShortInProgressText;
					break;
				case QuestStatus.Completed:
					currentQuestDescShort.text = ObjectiveCount() + "\n" + QuestsManager.instance._currentQuest.quest_ShortCompletedText;
					break;
			}
			
			buttonQuestTitleText.text = QuestsManager.instance._currentQuest.quest_ShortButtonUITitle;
		}
		else
		{
			currentQuestTitle.text = "To be continued..."; // <----------- TODO : CHANGE WITH A KEY FOR A DICTIONARY THAT WILL CHECK THE PROPER TERM IN THE CSV LOCALIZATION FILE
			currentQuestDescShort.text = "Travel in the world and talk to people to continue the story."; // <----------- TODO : CHANGE WITH A KEY FOR A DICTIONARY THAT WILL CHECK THE PROPER TERM IN THE CSV LOCALIZATION FILE
			buttonQuestTitleText.text = "To be continued..."; // <----------- TODO : CHANGE WITH A KEY FOR A DICTIONARY THAT WILL CHECK THE PROPER TERM IN THE CSV LOCALIZATION FILE
		}
	}

	/// <summary>
	/// update the map quest panel UI only if the UUID of the current quest that triggers this method
	/// is the same as the current quest, it's a waste of memory management and UI logic otherwise
	/// </summary>
	/// <param name="UUID"></param>
	public void UpdatePanelMapQuestUI(string UUID)
	{
		if(UUID == QuestsManager.instance._currentQuest.UID_Quest)
		{
			UpdateCurrentQuestTexts();
		}
	}

	public string ObjectiveCount()
	{
		string res = "0/0";

		string currentObjectiveCount = QuestsManager.instance._currentQuest.currentObjectiveNumber.ToString();
		string requiredAmount = QuestsManager.instance._currentQuest.requiredNumber.ToString();
		string objectiveName = "";
		string entityName = "";
		switch(QuestsManager.instance._currentQuest.questType)
		{
			case QuestType.Kill:
				objectiveName = "Killed";  // <----------- TODO : CHANGE WITH A KEY FOR A DICTIONARY THAT WILL CHECK THE PROPER TERM IN THE CSV LOCALIZATION FILE
				break;
		}

		// objective not null nor undefined ? good then display the entity type
		entityName = QuestsManager.instance._currentQuest.questEntity != EntityType.Undefined ||
			QuestsManager.instance._currentQuest.questEntity != EntityType.None ? QuestsManager.instance._currentQuest.questEntity.ToString() : "";

		// now we assemble the completed string, also if the first parameter is null... just don't use it :
		res = string.IsNullOrEmpty(entityName) ? (currentObjectiveCount + "/" + requiredAmount + " " + objectiveName) :
			(entityName + " " + currentObjectiveCount + "/" + requiredAmount + " " + objectiveName);

		return res;
	}
}
