using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSettings : MonoBehaviour
{
	public SettingsPanel settingsPanel;

	public Sprite spriteNormalOnHighlight;
	public Sprite spriteNormalOnClicked;
	public Sprite spriteHoversNormal;
	public Sprite spriteHoversSelected;
	public Sprite spriteSelectedOnHighlight;
	public Sprite spriteSelectedOnPress;

	public Color normalFontColor = new Color();
	public Color selectedFontColor = new Color();

	public Button btn;
	public Text txt;

	public void Awake()
	{
		btn = gameObject.GetComponent<Button>();
	}

	public void OnEnable()
	{
		btn.onClick.AddListener(OnClick);
	}

	public void OnDisable()
	{
		btn.onClick.RemoveListener(OnClick);
	}

	public void OnClick()
	{
		/// UI INFOS
		ButtonSettings[] allButtons = gameObject.GetComponentInParent<ButtonSettingsArray>().buttonSettingsArray;

		/// Reset all buttons to their default state
		foreach(ButtonSettings button in allButtons)
		{
			button.GetComponent<Image>().sprite = spriteHoversNormal;

			SpriteState ssHighlight = new SpriteState();
			ssHighlight.highlightedSprite = spriteNormalOnHighlight;
			button.GetComponent<Button>().spriteState = ssHighlight;

			SpriteState ssPressed = new SpriteState();
			ssPressed.pressedSprite = spriteNormalOnClicked;
			button.GetComponent<Button>().spriteState = ssPressed;

			button.txt.color = normalFontColor;

			// disable all settings panels :
			if(button.settingsPanel != null)
			{
				button.settingsPanel.settingPanel.gameObject.SetActive(false);
			}
		}

		/// CHANGE UI FOR THE SELECTED BUTTON
		GetComponent<Image>().sprite = spriteHoversSelected;

		SpriteState thisHighlight = new SpriteState();
		thisHighlight.highlightedSprite = spriteSelectedOnHighlight;
		btn.spriteState = thisHighlight;

		SpriteState thisPressed = new SpriteState();
		thisPressed.pressedSprite = spriteSelectedOnPress;
		btn.spriteState = thisPressed;

		txt.color = selectedFontColor;

		/// CHANGE THE SELECTED PANEL

		// enable the selected panel
		if(settingsPanel != null)
		{
			settingsPanel.settingPanel.gameObject.SetActive(true);
		}
	}
}
