using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
#endif

[System.Serializable]
public class SoundList
{
	public string name;
	public SceneName scene;
	public Sounds[] sounds;
}

[System.Serializable]
public class Sounds
{
	public string name;
	public AudioClip trackFile;
	public AudioType audioType;
	public bool playOnAwake = false;
	public bool loop = false;
	public bool randomizeSound = false;
	public float audioVolume = -100.0f;
}

[CreateAssetMenu(fileName = "SoundObjects", menuName = "Configs/SoundObjects", order = 1)]
public class SoundObjects : ScriptableObject
{
	public SoundList[] sounds;

	public SoundList Get(string sceneName)
	{
		for(int i = sounds.Length - 1; i >= 0; --i)
		{
			if(sounds[i].scene.ToString() == sceneName)
			{
				return sounds[i];
			}
		}

		return null;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundObjects), true)]
public class SoundObjectsEditor : Editor
{
	private ReorderableList list;

	public void OnEnable()
	{
		list = new ReorderableList(serializedObject, serializedObject.FindProperty("SoundObjects"), true, true, true, true)
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
		SoundObjects config = target as SoundObjects;

		int size = config.sounds.Length;
		System.Array.Resize(ref config.sounds, size + 1);
		config.sounds[size] = new SoundList();
	}

}
#endif