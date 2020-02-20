using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentQuestUIPanel : MonoBehaviour
{
	public Text questName;
	public Text questDescription;


	public void OnEnable()
	{
		UpdateCurrentQuestPanelUI();
	}

	public void UpdateCurrentQuestPanelUI()
	{
		if(QuestsManager.instance._currentQuest == null)
		{
			questName.text = "No quest is currently tracked."; // <----------- TODO : CHANGE WITH A KEY FOR A DICTIONARY THAT WILL CHECK THE PROPER TERM IN THE CSV LOCALIZATION FILE
			questDescription.text = "You don't have any quest currently tracked, try to look around in town and speak to people to progress in the story."; // <----------- TODO : CHANGE WITH A KEY FOR A DICTIONARY THAT WILL CHECK THE PROPER TERM IN THE CSV LOCALIZATION FILE
			return;
		}
		
		questName.text = QuestsManager.instance._currentQuest.questName;
		
		switch(QuestsManager.instance._currentQuest.questStatus)
		{
			case QuestStatus.Unlock:
			case QuestStatus.InProgress:
			case QuestStatus.Current:
				questDescription.text = QuestsManager.instance._currentQuest.quest_UnlockText;
				break;
			case QuestStatus.Completed:
				questDescription.text = QuestsManager.instance._currentQuest.quest_CompleteText;
				break;
			case QuestStatus.Failed:
				questDescription.text = QuestsManager.instance._currentQuest.quest_FailedText;
				break;
			case QuestStatus.Rewarded:
				questDescription.text = QuestsManager.instance._currentQuest.quest_RewardText;
				break;
		}
	}
}
