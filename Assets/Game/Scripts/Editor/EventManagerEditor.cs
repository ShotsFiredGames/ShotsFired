using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using InControl.ReorderableList;

[CustomEditor(typeof(EventManager))]
public class EventManagerEditor : Editor {

    SerializedProperty allAddOns;

    void OnEnable()
    {
        allAddOns = serializedObject.FindProperty("AllAddOns");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ReorderableListGUI.ListField(allAddOns);
        serializedObject.ApplyModifiedProperties();
    }
}
