using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointEditor : EditorWindow {
    [MenuItem("Editor/WayPointEditor")]
    static void ShowWindows() {
        GetWindow(typeof(WayPointEditor)).Show();
    }

    public GameObject PointList = null;
    private void OnGUI() {

        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("PointList"));


        if (PointList != null && GUILayout.Button("Create")) {
            GameObject Object = new GameObject("Node");
            Object.transform.parent = PointList.transform;
            Object.AddComponent<Node>();
            Object.AddComponent<MyGizmo>();
        }

        obj.ApplyModifiedProperties();
    }

}
