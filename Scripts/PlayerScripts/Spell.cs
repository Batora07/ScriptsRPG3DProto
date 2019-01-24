using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Spell
{
	public string nameSkill;
	public string skillDescription;
	public float cooldown;
	public float damage;
	public bool isHealing;
	public bool isManaFiller;
}
