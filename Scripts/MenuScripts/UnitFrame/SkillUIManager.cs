using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
	public static SkillUIManager instance;
	public SkillUIHandler[] skillsUI;

	public void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	public void OnEnable()
	{
		SkillUIHandler.useSkillFromUI += UseSkillFromUI;
	}

	public void OnDisable()
	{
		SkillUIHandler.useSkillFromUI -= UseSkillFromUI;
	}

	private void UseSkillFromUI(int nbSkills)
	{
		switch(nbSkills)
		{
			case 0:
				PlayerStatus.instance.GetComponent<CharacterMovement>().Skill_1();
				break;
			case 1:
				PlayerStatus.instance.GetComponent<CharacterMovement>().Skill_2();
				break;
		}
	}
}
