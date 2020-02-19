using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
#endif


[System.Serializable]
public class QuestsList
{
	public string name;
	public QuestStatus questStatus;
	public KillQuest[] killQuests;	
}

[CreateAssetMenu(fileName = "QuestsObjects", menuName = "Configs/Quests", order = 1)]
public class Quests : ScriptableObject
{
	public QuestsList[] questsLists;

	public QuestsList Get(QuestStatus questStatusLog)
	{
		for(int i = questsLists.Length - 1; i >= 0; --i)
		{
			if(questsLists[i].questStatus == questStatusLog)
			{
				return questsLists[i];
			}
		}

		return null;
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(Quests), true)]
public class QuestsEditor : Editor
{
	private ReorderableList list;

	public void OnEnable()
	{
		list = new ReorderableList(serializedObject, serializedObject.FindProperty("Quests"), true, true, true, true)
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
		Quests config = target as Quests;

		int size = config.questsLists.Length;
		System.Array.Resize(ref config.questsLists, size + 1);
		config.questsLists[size] = new QuestsList();
	}

}
#endif