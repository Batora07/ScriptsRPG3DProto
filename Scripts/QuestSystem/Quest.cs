using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Quest
{
	public string UID_Quest;
	public QuestType questType;
	public QuestStatus questStatus;
	public EntityType questEntity;
	public int requiredNumber;
	public int requiredLvl;
	public int currentObjectiveNumber;

	public string questName;			    // could be used by both UI map panel and quest log
	public string quest_ShortTitle;         // Used by UI map panel
	public string quest_ShortButtonUITitle; // Used by UI map panel
	public string quest_ShortInProgressText;// Used by UI map panel
	public string quest_ShortInFailedText;  // Used by UI map panel
	public string quest_ShortCompletedText; // Used by UI map panel
	public string quest_UnlockText;         // Used by quest log
	public string quest_CompleteText;       // Used by quest log
	public string quest_FailedText;         // Used by quest log
	public string quest_RewardText;         // Used by quest log

	public bool isLocked = true;

	public List<string> questsRequiredToUnlock = new List<string>();
	private List<bool> questRequirementsToUnlock = new List<bool>();
	
	public delegate void QuestRequirements(string questUID);
	public static event QuestRequirements requirementsToUnlock;

	public Quest()
	{
		//SetRequirements();
	}

	public void SubscribeEventLock()
	{
		requirementsToUnlock += UnlockRequirements;
	}

	public void SetRequirements()
	{
		int nbRequirements = questsRequiredToUnlock.Count;
		if(nbRequirements > 0)
		{
			for(int i = 0; i < nbRequirements; ++i)
			{
				questRequirementsToUnlock.Add(false);
			}
		}
	}

	/// <summary>
	/// when quest is in progress, send event to add this quest to the player quest Manager list
	/// </summary>
	public void InProgressQuest()
	{
		questStatus = QuestStatus.InProgress;
	}

	/// <summary>
	/// when quest is unlocked, send event to add this quest to the PNJ Quest giver as a new quest to give to the player
	/// </summary>
	public void UnlockQuest()
	{
		// unsubscribe the event to unlock this quest
		requirementsToUnlock -= UnlockRequirements;

		questStatus = QuestStatus.Unlock;
		//Debug.Log("Quest Unlock");
	}

	/// <summary>
	/// if the quest is locked, then the quest giver don't hint at the player that he can give him a quest unless certain conditions are met
	/// </summary>
	public void LockQuest()
	{
		questStatus = QuestStatus.Locked;
	}

	/// <summary>
	/// when quest is completed, send event to the quest manager, 
	/// so that the player can get the reward from PNJ quest giver for this quest
	/// </summary>
	public void CompleteQuest()
	{
		questStatus = QuestStatus.Completed;
		RequirementsToUnlock(UID_Quest);
	}

	/// <summary>
	/// Quest failed implies the quest giver won't give the player the quest again, also the player can't be rewarded and can't progress anymore in this quest
	/// for this playthrought
	/// </summary>
	public void FailedQuest()
	{
		questStatus = QuestStatus.Failed;
	}

	/// <summary>
	///  now that the quest has been done and rewarded, the quest giver can't give the reward anymore, quest removed from 
	///  both player to be rewarded and quest giver to give, but still available in the quest log
	/// </summary>
	public void RewardedQuest()
	{
		questStatus = QuestStatus.Rewarded;
	}

	/// <summary>
	/// now that the quest has been aborted, 
	/// it's removed from quests list of player, but become once again available at quest giver
	/// </summary>
	public void AbortQuest()
	{
		questStatus = QuestStatus.Aborted;
	}

	/// <summary>
	/// Set the status from a string - for example if we're trying to load the quests xml database
	/// By default if the string don't exist in the QuestStatus enum, set questStatus as Locked
	/// </summary>
	/// <param name="statusString"></param>
	public void GetStatusFromString(string statusString)
	{
		List<string> statusEnumList = new List<string>(Enum.GetNames(typeof(QuestStatus)));
		bool isInEnum = statusEnumList.Any(s => statusString.ToLower().Contains(s.ToLower()));
		QuestStatus qStatus = QuestStatus.Locked;
		if(isInEnum)
		{
			// will parse the string as enum without regarding the case sensitive
			qStatus = (QuestStatus)(Enum.Parse(typeof(QuestStatus), statusString, false));
		}
		questStatus = qStatus;
	}

	/// <summary>
	/// Event that checks if any quest has been unlocked once this quest has been completed
	/// </summary>
	/// <param name="_UID_quest">UID of the current quest completed</param>
	public void RequirementsToUnlock(string _UID_quest)
	{
		if(requirementsToUnlock != null)
		{
			//requirementsToUnlock(_UID_quest);
			requirementsToUnlock(_UID_quest);
		}
	}

	/// <summary>
	/// Method triggered by event, force check all quests subscribed to the event to check if they are now unlocked to the NPC quest giver
	/// </summary>
	/// <param name="_UID_Quest">Unique Identifier for the current quest completed</param>
	public void UnlockRequirements(string _UID_Quest)
	{
	//	Debug.Log("Unlock next quest ? / Current quest  = " + UID_Quest + " Quest done = " + _UID_Quest);
		CheckUnlocksRequirementsMet(_UID_Quest);
	}

	/// <summary>
	/// Check if the current quest triggered by the event can now be Unlocked to NPC quest giver
	/// </summary>
	/// <param name="_UID_quest">Unique Identifier for the completed quest that triggered this event</param>
	private void CheckUnlocksRequirementsMet(string _UID_quest)
	{
		// Locked => use this event to know if we can unlock this quest now
	//	Debug.Log("yes");
		if(questStatus == QuestStatus.Locked)
		{
	//		Debug.Log("quest Locked");
			int nbRequirements = questsRequiredToUnlock.Count;

			if(nbRequirements > 0)
			{
				// add this requirement as a requirement met to the bool list
				for(int i = 0; i < nbRequirements; ++i)
				{
					if(questsRequiredToUnlock[i] == _UID_quest)
					{
						if(i < questRequirementsToUnlock.Count )
						{
							questRequirementsToUnlock[i] = true;
						}
					}
				}

				// we met all the requirements to unlock this current quest
				if(CheckFullRequirementsMet(nbRequirements))
				{
					UnlockQuest();
				}
			}
		}
	}

	/// <summary>
	/// Are all the requirements met to unlock this quest ? (in case of multiple quests to first complete in order for this quest to appear
	/// to the quest giver)
	/// </summary>
	/// <param name="nbRequirements">The number of requirements to be met for this quest to unlock</param>
	/// <returns></returns>
	private bool CheckFullRequirementsMet(int nbRequirements)
	{
		for(int i = 0; i < nbRequirements; ++i)
		{
			if(i < questRequirementsToUnlock.Count)
			{
				if(questRequirementsToUnlock[i] == false)
				{
					return false;
				}
			}
		}

		isLocked = false;
		return true;
	}

}
