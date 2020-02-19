using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillQuest : Quest
{
	public int maxKillCount;
	public int currentKillCount;

	public EntityType entityToKill;

	public KillQuest()
	{

	}

	public KillQuest(Quest questToSetup)
	{
		UID_Quest = questToSetup.UID_Quest;
		questType = QuestType.Kill;
		questStatus = questToSetup.questStatus;
		requiredLvl = questToSetup.requiredLvl;
		questName = questToSetup.questName;
		quest_UnlockText = questToSetup.quest_UnlockText;
		quest_CompleteText = questToSetup.quest_CompleteText;
		quest_FailedText = questToSetup.quest_FailedText;
		quest_RewardText = questToSetup.quest_RewardText;

		isLocked = questToSetup.isLocked;
		questsRequiredToUnlock = questToSetup.questsRequiredToUnlock;
		entityToKill = questToSetup.questEntity;
		maxKillCount = questToSetup.requiredNumber;
	}

	public new void InProgressQuest()
	{
		questStatus = QuestStatus.InProgress;
		EnemyHealth.entityKilled += IncreaseKillCount;
	}

	public void UnlockedQuest()
	{
		questStatus = QuestStatus.InProgress;
		EnemyHealth.entityKilled += IncreaseKillCount;
	}

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
		QuestsManager.instance.ChangeCurrentQuest(null);
	}

	public void IncreaseKillCount(EntityType entityKilled)
	{
		if(entityKilled == entityToKill)
		{
			if(currentKillCount < maxKillCount)
			{
				currentKillCount++;
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
