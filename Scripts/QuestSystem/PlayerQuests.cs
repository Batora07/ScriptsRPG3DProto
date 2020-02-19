using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerQuests
{
	public List<KillQuest> playerKillQuests = new List<KillQuest>();

	// constructor
	public PlayerQuests()
	{
		// when all tests are good, make a proper initialization 
		// (from save state of player) of all of the quests the player got from npc quests givers.
		TestQuests();
	}

	/// <summary>
	/// Use this to debug the quests given to the player
	/// </summary>
	public void TestQuests()
	{
		/* 1 / DEBUG KILL QUEST TEST */
		KillQuest killWolfQuest = new KillQuest();
		killWolfQuest.UID_Quest = "000";
		killWolfQuest.currentKillCount = 0;
		killWolfQuest.maxKillCount = 3;
		killWolfQuest.questName = "test wolf kill";
		killWolfQuest.entityToKill = EntityType.Wolf;
		killWolfQuest.InProgressQuest();
		killWolfQuest.SetRequirements();

		/* 2 / DEBUG TEST UNLOCK QUEST needs #1 to work */
		KillQuest killOrcQuest = new KillQuest();
		killOrcQuest.UID_Quest = "001";
		killOrcQuest.currentKillCount = 0;
		killOrcQuest.maxKillCount = 2;
		killOrcQuest.questName = "test ork kill";
		killOrcQuest.entityToKill = EntityType.Orc;
		killOrcQuest.questsRequiredToUnlock.Add("000");
		killOrcQuest.LockQuest();

		killOrcQuest.SetRequirements();

		playerKillQuests.Add(killWolfQuest);
		playerKillQuests.Add(killOrcQuest);

		for(int i = 0; i < playerKillQuests.Count; ++i)
		{
			if(playerKillQuests[i].questStatus == QuestStatus.Locked)
			{
				playerKillQuests[i].SubscribeEventLock();
			}
		}
	}
}
