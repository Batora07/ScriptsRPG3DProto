using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogDisplay : MonoBehaviour
{
	public static QuestLogDisplay instance;

	public List<Quest> inProgressQuests = new List<Quest>();
	public List<Quest> completedQuests = new List<Quest>();
	public List<Quest> failedQuests = new List<Quest>();

	public Transform questsListContainer;
	public QuestLogTab questLogTabPrefab;

	public Text questName;
	public Text questStatus;
	public Text questDescription;

	public Text rewardText;
	public Text rewardChooseOneText;

	public Button trackQuest;
	public Button abortQuest;

	public RewardQuestButton[] rewardQuestButtons;

	public void Awake()
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

	public void OnEnable()
	{
		ClearQuestLogTab();
		ClearSelectedQuest();
		FillQuestLogTab();
	}

	public void FillQuestLogTab()
	{
		// Add the In Progress Quests first
		foreach(Quest item in inProgressQuests)
		{
			InstantiateNewQuestLogTab(item);
		}

		// Then add the completed Quests 
		foreach(Quest item in completedQuests)
		{
			InstantiateNewQuestLogTab(item);
		}

		// Finally add the Failed Quests 
		foreach(Quest item in failedQuests)
		{
			InstantiateNewQuestLogTab(item);
		}
	}

	public void ClearQuestLogTab()
	{
		foreach(Transform child in questsListContainer)
		{
			Destroy(child.gameObject);
		}
	}

	public void ClearSelectedQuest()
	{
		questStatus.text = "";
		questName.text = "";
		questDescription.text = "";
		rewardText.text = "";
		rewardChooseOneText.text = "";
		for(int i = 0; i < rewardQuestButtons.Length ; ++i)
		{
			rewardQuestButtons[i].rewardSprite = rewardQuestButtons[i].defaultRewardSprite;
			rewardQuestButtons[i].selectedReward.gameObject.SetActive(false);
		}
	}

	public void DisplaySelectedQuest(Quest questInfos)
	{
		string questStatusPlayer = "Quest : ";
		switch(questInfos.questStatus)
		{
			case QuestStatus.Completed:
				questStatusPlayer += "Completed.";
				break;
			case QuestStatus.Failed:
				questStatusPlayer += "Failed.";
				break;
			case QuestStatus.InProgress:
				questStatusPlayer += "In Progress.";
				break;
		}
		questStatus.text = questStatusPlayer;
		questName.text = questInfos.questName;
		questDescription.text = questInfos.quest_UnlockText;
		rewardText.text = "Reward";
		rewardChooseOneText.text = "-- Choose one --";
	}

	public void InstantiateNewQuestLogTab(Quest item)
	{
		QuestLogTab newQuestTab = Instantiate(questLogTabPrefab) as QuestLogTab;
		newQuestTab.transform.SetParent(questsListContainer, false);

		// set ui of the item
		newQuestTab.Display(item);
		newQuestTab.questInfos = item;
	}
}
