using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using UnityEditorInternal;
#endif



[CreateAssetMenu(fileName = "Skills", menuName = "Configs/SkillsListing", order = 1)]
public class SkillsListing : ScriptableObject
{
	public List<Spell> skillsList = new List<Spell>();

	public Spell Get(string _nameSkill)
	{
		Spell result = new Spell();
		for(int i = skillsList.Count - 1; i >= 0; --i)
		{
			if(skillsList[i].nameSkill == _nameSkill)
			{
				return skillsList[i];
			}
		}

		return result;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SkillsListing), true)]
public class SkillsListingEditor : Editor
{
	private ReorderableList list;

	public void OnEnable()
	{
		list = new ReorderableList(serializedObject, serializedObject.FindProperty("SkillsListing"), true, true, true, true)
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
		SkillsListing config = target as SkillsListing;

		int size = config.skillsList.Count;
		Spell newSkillData = new Spell();
		config.skillsList.Add(newSkillData);
	}
}
#endif