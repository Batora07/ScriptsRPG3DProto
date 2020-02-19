using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIHandler : MonoBehaviour
{
	public Transform skillActive;
	public Image skillInCooldown;
	public Text timingText;

	private float cooldownSkill = 0.5f;
	private Button btnSkill;
	public bool isOnCooldown = false;
	public int slotNumber = 0;

	public delegate void UseSkillEvent(int index);
	public static event UseSkillEvent useSkillFromUI;

	public float CooldownSkill
	{
		get
		{
			return cooldownSkill;
		}

		set
		{
			cooldownSkill = value;
		}
	}

	public void Update()
	{
		ProgressCooldown();
	}

	public void Awake()
	{
		btnSkill = GetComponent<Button>();
	}

	public void OnEnable()
	{
		btnSkill.onClick.AddListener(delegate { UseSkill(); });
	}

	public void OnDisable()
	{
		btnSkill.onClick.RemoveListener(delegate { UseSkill(); });
	}

	public void UseSkill()
	{
		if(isOnCooldown)
			return;

		UseSkillFromUI();
	}

	public void DisplayCooldownSkill(float cd)
	{
		if(isOnCooldown)
			return;

		StartCoroutine(SetCooldown(cd));

		skillActive.gameObject.SetActive(true);
		skillInCooldown.fillAmount = 0;
	}

	private IEnumerator SetCooldown(float cd)
	{
		isOnCooldown = true;
		timingText.gameObject.SetActive(true);
		yield return new WaitForSeconds(cd);
		isOnCooldown = false;
		skillActive.gameObject.SetActive(false);
		timingText.gameObject.SetActive(false);
	}

	private void ProgressCooldown()
	{
		if(!isOnCooldown)
			return;

		skillInCooldown.fillAmount += ((1 / cooldownSkill) * Time.deltaTime);
		if(timingText.gameObject.activeSelf)
		{
			timingText.text = (cooldownSkill - (cooldownSkill * skillInCooldown.fillAmount)).ToString("0.0");
		}
	}

	public void UseSkillFromUI()
	{
		if(useSkillFromUI != null)
		{
			useSkillFromUI(slotNumber);
		}
	}
}
