using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quests requiring killing some unit type with a required amount
/// </summary>
[System.Serializable]
public class KillQuest : Quest
{
	public int maxKillCount;
	public int currentKillCount;

	public EntityType entityToKill;

	public KillQuest()
	{

	}

	/// <summary>
	/// Constructor that will setup a quest loaded from XML database to a proper and working KillQuest
	/// </summary>
	/// <param name="questToSetup"></param>
	public KillQuest(Quest questToSetup)
	{
		UID_Quest = questToSetup.UID_Quest;
		questType = QuestType.Kill;
		questStatus = questToSetup.questStatus;
		requiredLvl = questToSetup.requiredLvl;
		// Quest log UI
		questName = questToSetup.questName;
		quest_UnlockText = questToSetup.quest_UnlockText;
		quest_CompleteText = questToSetup.quest_CompleteText;
		quest_FailedText = questToSetup.quest_FailedText;
		quest_RewardText = questToSetup.quest_RewardText;

		// UI Map Quest Panel
		quest_ShortTitle = questToSetup.quest_ShortTitle;         
		quest_ShortButtonUITitle = questToSetup.quest_ShortButtonUITitle; 
		quest_ShortInProgressText = questToSetup.quest_ShortInProgressText;
		quest_ShortInFailedText = questToSetup.quest_ShortInFailedText;  
		quest_ShortCompletedText = questToSetup.quest_ShortCompletedText; 

		isLocked = questToSetup.isLocked;
		questsRequiredToUnlock = questToSetup.questsRequiredToUnlock;
		entityToKill = questToSetup.questEntity;
		maxKillCount = questToSetup.requiredNumber;
		questEntity = questToSetup.questEntity;
		requiredNumber = questToSetup.requiredNumber;
	}

	/// <summary>
	/// Quest is in progress, we now subscribe to the IncreaseKillCount event
	/// </summary>
	public new void InProgressQuest()
	{
		questStatus = QuestStatus.InProgress;
		EnemyHealth.entityKilled += IncreaseKillCount;
	}

	/// <summary>
	/// Quest has been unlocked, we now subscribe to the IncreaseKillCount event
	/// </summary>
	public void UnlockedQuest()
	{
		questStatus = QuestStatus.InProgress;
		EnemyHealth.entityKilled += IncreaseKillCount;
	}

	/// <summary>
	/// Unsubscribe from the kill count and proceed the next status for the quest : Completed Event
	/// </summary>
	public new void CompleteQuest()
	{
		questStatus = QuestStatus.Completed;
		EnemyHealth.entityKilled -= IncreaseKillCount;
		RequirementsToUnlock(UID_Quest);
		ChangeQuestsListTypeForScriptableObject(QuestStatus.InProgress, questStatus);
		// <------------- TODO ----------------->
		/*
		 * DISPLAY ANIMATION WHEN QUEST IS FINISHED, also change text of the quest to display the end text short description
		 * */
		QuestsManager.instance.CurrentQuestUpdated();
	}

	/// <summary>
	/// This will trigger the currentkill event check and complete the quest if required amount has been validated
	/// </summary>
	/// <param name="entityKilled"></param>
	public void IncreaseKillCount(EntityType entityKilled)
	{
		if(entityKilled == entityToKill)
		{
			if(currentKillCount < maxKillCount)
			{
				currentKillCount++;
				currentObjectiveNumber = currentKillCount;
				// update the UI if the current quest is the same as this quest
				MapPanelQuests.instance.UpdatePanelMapQuestUI(this.UID_Quest);
				if(currentKillCount == maxKillCount)
				{
					CompleteQuest();
				}
			}
		}		
	}

	/// <summary>
	/// Once the quest is finished, reorganizes the lists to get this current status quest to the list of the next status
	/// </summary>
	/// <param name="currentStatus"></param>
	/// <param name="nextStatus"></param>
	public void ChangeQuestsListTypeForScriptableObject(QuestStatus currentStatus, QuestStatus nextStatus)
	{
		int nbLists = QuestsManager.instance.questsScriptableObject.questsLists.Length;
		int nbPreviousListIndex = 0;
		int nbNextListIndex = 0;
		
		List<KillQuest> previousKillQuestList = new List<KillQuest>();
		List<KillQuest> nextKillQuestList = new List<KillQuest>();

		for(int i = 0; i < nbLists; ++i)
		{
			if(currentStatus == QuestsManager.instance.questsScriptableObject.questsLists[i].questStatus)
			{
				nbPreviousListIndex = i;
				previousKillQuestList = QuestsManager.instance.questsScriptableObject.questsLists[i].killQuests.ToList();
			}
			else if(nextStatus == QuestsManager.instance.questsScriptableObject.questsLists[i].questStatus)
			{
				nbNextListIndex = i;
				nextKillQuestList = QuestsManager.instance.questsScriptableObject.questsLists[i].killQuests.ToList();
			}
		}

		nextKillQuestList.Add(this);
		previousKillQuestList.Remove(previousKillQuestList.Single(q => q.UID_Quest == this.UID_Quest));

		QuestsManager.instance.questsScriptableObject.questsLists[nbPreviousListIndex].killQuests = previousKillQuestList.ToArray();
		QuestsManager.instance.questsScriptableObject.questsLists[nbNextListIndex].killQuests = nextKillQuestList.ToArray();
	}
}
