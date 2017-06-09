using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PickUp)), CanEditMultipleObjects]
public class PickUpEditor : Editor
{
    public SerializedProperty 
        pickUpType_Prop, 
        gun_Prop, 
        ability_Prop;

    private void OnEnable()
    {
        pickUpType_Prop = serializedObject.FindProperty("type");
        gun_Prop = serializedObject.FindProperty("gun");
        ability_Prop = serializedObject.FindProperty("ability");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(pickUpType_Prop, new GUIContent("Pick Up Type"));
        PickUp.PickUpType type = (PickUp.PickUpType)pickUpType_Prop.enumValueIndex;

        switch (type)
        {
            case PickUp.PickUpType.None:
                EditorGUILayout.LabelField("Choose a pickup type");
                break;
            case PickUp.PickUpType.Gun:
                EditorGUILayout.PropertyField(gun_Prop, new GUIContent("Gun Type"));
                break;

            case PickUp.PickUpType.Ability:
                EditorGUILayout.PropertyField(ability_Prop, new GUIContent("Ability Type"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
