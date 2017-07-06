using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(EventManager))]
public class GameEventEditor : Editor {

    int flags = 0;
    List<string> options;
    SerializedProperty allAddOns;

    void OnEnable()
    {
        allAddOns = serializedObject.FindProperty("AllAddOns");
    }

    void Update()
    {
        for(int i = 0; i < allAddOns.arraySize; i++)
        {
            if (options.Capacity != allAddOns.arraySize)
                options.Capacity = allAddOns.arraySize;
            options[i] = allAddOns.GetArrayElementAtIndex(i).ToString();
        }        
    }

    public override void OnInspectorGUI()
    {
        flags = EditorGUI.MaskField(new Rect(0, 0, 300, 20), new GUIContent("Player Flags"), flags, options.ToArray(), EditorStyles.popup);
    }
}