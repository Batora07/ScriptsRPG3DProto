using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardQuestButton : MonoBehaviour
{
	public Sprite rewardSprite;
	public Sprite defaultRewardSprite;

	public Transform selectedReward;

	private Button btn;

	private void Awake()
	{
		btn = GetComponent<Button>();
		btn.onClick.AddListener(SelectedReward);
	}

	public void SelectedReward()
	{
		selectedReward.gameObject.SetActive(!selectedReward.gameObject.activeSelf);
	}
}
