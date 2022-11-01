using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ViewEditor : EditorWindow {


    [MenuItem("Editor/FieldOfView")]
    static void ShowWindows() {
        GetWindow(typeof(ViewEditor)).Show();
    }

    public GameObject Object = null;
    private void OnGUI() {

        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("Object"));

        GUILayout.BeginHorizontal();

        if (Object != null)
            if (GUILayout.Button("Button", GUILayout.Width(100), GUILayout.Height(20), GUILayout.MaxWidth(150), GUILayout.MaxHeight(30), GUILayout.MinWidth(50), GUILayout.MinHeight(15))) {
                if(Object.GetComponent<FieldOfView>() == null) {
                    Object.AddComponent<FieldOfView>();
                }
                Object = null;
            }

        GUILayout.EndHorizontal();

        obj.ApplyModifiedProperties();
    }
}
