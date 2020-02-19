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
			currentQuestTitle.text = QuestsManager.instance._currentQuest.questName;
			currentQuestDescShort.text = QuestsManager.instance._currentQuest.quest_UnlockText;
			buttonQuestTitleText.text = QuestsManager.instance._currentQuest.questName;
		}
		else
		{
			currentQuestTitle.text = "To be continued...";
			currentQuestDescShort.text = "Travel in the world and talk to people to continue the story.";
			buttonQuestTitleText.text = "To be continued...";
		}
	}
}
