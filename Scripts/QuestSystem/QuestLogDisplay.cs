using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
	public Quest currentQuestDisplayed;

	public void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			trackQuest.onClick.AddListener(TrackCurrentQuest);
		}
	}

	public void OnEnable()
	{
		ClearQuestLogTab();
		ClearSelectedQuest();
		FillQuestLists();
		FillQuestLogTab();
		SelectFirstOrCurrent();
	}

	public void FillQuestLists()
	{
		int lenghtQuestList = QuestsManager.instance.questsScriptableObject.questsLists.Length;
		for(int i = 0 ; i < lenghtQuestList; ++i)
		{
			QuestStatus qS = QuestsManager.instance.questsScriptableObject.questsLists[i].questStatus;
			switch(qS)
			{
				case QuestStatus.Current:
				case QuestStatus.InProgress:
				case QuestStatus.Unlock:
					inProgressQuests.AddRange(QuestsManager.instance.questsScriptableObject.questsLists[i].killQuests.Cast<Quest>().ToList());
					inProgressQuests = RemoveDuplicate(inProgressQuests);
					break;
				case QuestStatus.Completed:
				case QuestStatus.Rewarded:
					completedQuests.AddRange(QuestsManager.instance.questsScriptableObject.questsLists[i].killQuests.Cast<Quest>().ToList());
					completedQuests = RemoveDuplicate(completedQuests);
					break;
				case QuestStatus.Failed:
					failedQuests.AddRange(QuestsManager.instance.questsScriptableObject.questsLists[i].killQuests.Cast<Quest>().ToList());
					failedQuests = RemoveDuplicate(failedQuests);
					break;
			}
		}
		inProgressQuests = UpdateQuestList(inProgressQuests, completedQuests); // remove quests from in progress if they are in completedQuests
		inProgressQuests = UpdateQuestList(inProgressQuests, failedQuests);    // remove quests from in progress if they are failed
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
				questDescription.text = questInfos.quest_UnlockText;
				questDescription.text += "\n\n" + questInfos.quest_CompleteText;
				break;
			case QuestStatus.Rewarded:
				questStatusPlayer += "Completed.";
				questDescription.text = questInfos.quest_UnlockText;
				questDescription.text += "\n\n" + questInfos.quest_CompleteText;
				questDescription.text += "\n\n" + questInfos.quest_RewardText;
				break;
			case QuestStatus.Failed:
				questStatusPlayer += "Failed.";
				questDescription.text = questInfos.quest_UnlockText;
				questDescription.text += "\n\n" + questInfos.quest_FailedText;
				break;
			case QuestStatus.InProgress:
			case QuestStatus.Current:
				questStatusPlayer += "In Progress.";
				questDescription.text = questInfos.quest_UnlockText;
				break;
		}
		questStatus.text = questStatusPlayer;
		questName.text = questInfos.questName;
		
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

	public List<Quest> RemoveDuplicate(List<Quest> listWithDuplicate)
	{
		return listWithDuplicate.Distinct().ToList();
	}

	/// <summary>
	/// remove elements from previous list after the next list has been updated
	/// </summary>
	/// <param name="previousList"></param>
	/// <param name="nextList"></param>
	public List<Quest> UpdateQuestList(List<Quest> previousList, List<Quest> nextList)
	{
		List<Quest> qToRemove = previousList.Intersect(nextList).ToList();
		previousList = previousList.Except(qToRemove).ToList();
		return previousList;
 	}

	private void TrackCurrentQuest()
	{
		switch(currentQuestDisplayed.questStatus)
		{
			case QuestStatus.Current:
			case QuestStatus.InProgress:
			case QuestStatus.Unlock:
			case QuestStatus.Completed:			
				MapPanelQuests.instance.UpdatePanelMapQuestUI(currentQuestDisplayed.UID_Quest);
				QuestsManager.instance.ChangeCurrentQuest(currentQuestDisplayed);
				break;
		}
	}

	public void SelectFirstOrCurrent()
	{
		bool shouldSelectFirst = QuestsManager.instance._currentQuest == null ? true : false;
		QuestLogTab[] otherQuests = questsListContainer.GetComponentsInChildren<QuestLogTab>();
		if(shouldSelectFirst && otherQuests[0] != null)
		{
			otherQuests[0].DisplayQuestInfos();
		}
		else
		{
			foreach(QuestLogTab q in otherQuests)
			{
				if(q.questInfos.Equals(QuestsManager.instance._currentQuest))
				{
					q.DisplayQuestInfos();
				}
			}
		}
	}
}