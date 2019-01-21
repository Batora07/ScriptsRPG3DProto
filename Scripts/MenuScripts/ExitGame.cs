using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
	private Button btn;

	private void Awake()
	{
		btn = GetComponent<Button>();
	}

	private void OnEnable()
	{
		btn.onClick.AddListener(QuitGame);
	}

	private void OnDisable()
	{
		btn.onClick.RemoveListener(QuitGame);
	}

    public void QuitGame()
	{
		Application.Quit();
	}
}
