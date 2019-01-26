using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHealing : MonoBehaviour
{
	private PlayerHealth playerHealth;
	private Spell currentSpell;
	private bool isInAOE = false;

	public delegate void TriggerHealingEvent(bool isInAOE);
	public static event TriggerHealingEvent triggerHealing;

	void OnTriggerEnter(Collider co)
	{
		if(co.tag == "Player")
		{
			isInAOE = true;
			TriggerHealingAOE(isInAOE);
			playerHealth = PlayerStatus.instance.GetComponent<PlayerHealth>();
			currentSpell = PlayerStatus.instance.skills.skillsList[1];

			playerHealth.HealOnTime(currentSpell.cooldown, currentSpell.damage, isInAOE);
		}
	}

	void OnTriggerExit(Collider co)
	{
		if(co.tag == "Player")
		{
			isInAOE = false;
			TriggerHealingAOE(isInAOE);
			playerHealth = PlayerStatus.instance.GetComponent<PlayerHealth>();
			currentSpell = PlayerStatus.instance.skills.skillsList[1];
		}
	}

	public void OnDestroy()
	{
		TriggerHealingAOE(false);
	}

	public void TriggerHealingAOE(bool _isInAOE)
	{
		if(triggerHealing != null)
		{
			triggerHealing(_isInAOE);
		}
	}
}
