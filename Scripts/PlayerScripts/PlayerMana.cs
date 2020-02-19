using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
	public float mana = 100f;
	public float maxMana = 100f;

	public void Start()
	{
		PlayerStatus.instance.PlayerMana = this;
	}

	public void UseMana(float manaAmount)
	{
		mana -= manaAmount;
//		Debug.Log("Urgh I lost Mana !");

		if(mana <= 0)
		{
			// display FX for missing mana and lock use of spells
			Debug.Log("missing mana");
		}
	}

	public void FillMana(float manaAmount)
	{
		if(manaAmount + mana > maxMana)
		{
			mana = maxMana;
		}
		else
		{
			mana += manaAmount;
		}
	}

}// class