using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode, CustomEditor(typeof(CaptureTheFlag))]
public class GameEventEditor : Editor {

    int flags = 0;
    List<string> options;
    //SerializedProperty allAddOns;
    CaptureTheFlag ctf;

    void OnEnable()
    {
        options = new List<string>();
        //allAddOns = serializedObject.FindProperty("allAddOns");
        ctf = (CaptureTheFlag)target;
    }

    public override void OnInspectorGUI()
    {
        Debug.Log("options: " + options.Capacity);
        Debug.Log(ctf.allAddOns.Capacity);
        for (int i = 0; i < ctf.allAddOns.Count; i++)
        {
            if (options.Capacity != ctf.allAddOns.Count)
                options.Capacity = ctf.allAddOns.Count;
            options[i] = ctf.allAddOns[i].ToString();
        }
        Debug.Log("options: " + options.Capacity);
        Debug.Log(ctf.allAddOns.Capacity);
        flags = EditorGUI.MaskField(new Rect(0, 250, 300, 20), new GUIContent("Player Flags"), flags, options.ToArray(), EditorStyles.popup);
    }
}