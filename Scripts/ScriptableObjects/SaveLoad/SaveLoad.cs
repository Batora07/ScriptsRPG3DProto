using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
#endif

[System.Serializable]
[SerializeField]
public struct SaveTransform
{
	public float positionX;
	public float positionY;
	public float positionZ;

	public float rotationX;
	public float rotationY;
	public float rotationZ;

	public float scaleX;
	public float scaleY;
	public float scaleZ;
}

[System.Serializable]
[SerializeField]
public class SaveSettings
{
	// AUDIO SETTINGS
	public bool isSoundMuted = false;
	public float soundMasterLvl;
	public float soundMusicLvl;
	public float soundAmbiantLvl;
	public float soundVoiceLvl;
	public float soundSFXLvl;

	// GRAPHICS SETTINGS
	public GraphicsQuality graphicsQuality;
	public GraphicsResolution graphicsResolution;
	public bool isShadowsEnabled = true;

	// CAMERA SETTINGS
	public SaveTransform cameraPosAtSave;
	public float cameraFOVZoom;
}

[System.Serializable]
[SerializeField]
public class SavePlayerInfos
{
	public string nameCharacter;              // character name
	public PlayableCharacter typeCharacter;   // type of character selected
	public float level;                       // Level of the character
	public float currentHP;                   // current life state for this character
	public float maxHP;                       // maximum life the character can get 
	public float currentMP;                   // current mana state for this character
	public float maxMP;                       // maximum mana level the character can get

	public SaveTransform currentPos;              // position of the character when saving
	public SceneName currentScene;            // which scene needs to be loaded
}

[CreateAssetMenu(fileName = "Save", menuName = "Configs/SaveFile", order = 1)]
public class SaveLoad : ScriptableObject
{
	public List<PlayerData> savedInfos = new List<PlayerData>();

	public PlayerData Get(string _nameSave)
	{
		for(int i = savedInfos.Count - 1; i >= 0; --i)
		{
			if(savedInfos[i].NameSave == _nameSave)
			{
				return savedInfos[i];
			}
		}

		return null;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SaveLoad), true)]
public class SaveLoadEditor : Editor
{
	private ReorderableList list;

	public void OnEnable()
	{
		list = new ReorderableList(serializedObject, serializedObject.FindProperty("Save"), true, true, true, true)
		{
			drawElementCallback = DrawItem,
			elementHeightCallback = Height,
			drawHeaderCallback = DrawHeader,
			onAddCallback = AddItem
		};
	}

	/*
	public override void OnInspectorGUI()
	{
		if(GUILayout.Button("Reload Preconfig"))
		{
			Fill();
			return;
		}
		
		serializedObject.Update();
		list.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
		EditorUtility.SetDirty(target);
	}
	*/

	private void DrawItem(Rect rect, int index, bool isActive, bool isFocused)
	{
		SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
		rect.y += 2f;
		rect.x += 10f;
		rect.width -= 10f;
		EditorGUI.PropertyField(rect, element, true);
	}

	private float Height(int index)
	{
		SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
		return EditorGUI.GetPropertyHeight(element) + 5f;
	}

	private void DrawHeader(Rect rect)
	{
		EditorGUI.LabelField(rect, "Elements Preconfig");
	}

	private void AddItem(ReorderableList list)
	{
		SaveLoad config = target as SaveLoad;

		int size = config.savedInfos.Count;
		PlayerData newPlayerData = new PlayerData();
		config.savedInfos.Add(newPlayerData);
	}
}
#endif