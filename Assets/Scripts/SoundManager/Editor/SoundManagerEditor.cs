using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor {
	
	Vector2 scroll = Vector2.zero;
	
	public override void OnInspectorGUI ()
	{
		SoundManager targetObject = target as SoundManager;

		EditorGUILayout.BeginVertical();

		targetObject.resourceLocation = EditorGUILayout.TextField("Resource Location", targetObject.resourceLocation);

		EditorGUILayout.BeginVertical("box");
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label("Assets");
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add", EditorStyles.toolbarButton)) targetObject.Add();
		EditorGUILayout.EndHorizontal();
		
		scroll = EditorGUILayout.BeginScrollView(scroll);
		
		int removeIndex = -1;
		for (int i = 0; i < targetObject.soundDataList.Length; i++)
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Label(targetObject.soundDataList[i].soundKey);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Remove", EditorStyles.toolbarButton)) removeIndex = i;
			EditorGUILayout.EndHorizontal();
			targetObject.soundDataList[i].soundKey = EditorGUILayout.TextField("Sound Key", targetObject.soundDataList[i].soundKey);
			targetObject.soundDataList[i].soundFile = EditorGUILayout.TextField("File Name", targetObject.soundDataList[i].soundFile);
			EditorGUILayout.EndVertical();
		}
		if (removeIndex != -1) targetObject.Remove(removeIndex);
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.EndVertical();
		
		if (GUI.changed)
			EditorUtility.SetDirty(target);

	}
	
	[MenuItem("Game Tools/Asset Editor/Sound Manager")]
	public static void GetSoundManager()
	{
		if (SoundManager.Instance == null) Selection.activeObject = SoundManager.CreateData("Assets/Game/Resources");
		else Selection.activeObject = SoundManager.Instance;
	}
}
