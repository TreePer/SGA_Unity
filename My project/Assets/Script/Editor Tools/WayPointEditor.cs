using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointEditor : EditorWindow {
    [MenuItem("Editor/WayPointEditor")]
    static void ShowWindows() {
        GetWindow(typeof(WayPointEditor)).Show();
    }

    public GameObject NodeList;
    private void OnGUI() {

        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("NodeList"));


        if (NodeList != null && GUILayout.Button("Create")) {
            CreateNode();
        }

        obj.ApplyModifiedProperties();
    }

    public void CreateNode() {
        GameObject Object = new GameObject(NodeList.transform.childCount.ToString());
        Object.transform.position = new Vector3(25.0f, 0.0f, 25.0f);
        Object.transform.SetParent(NodeList.transform);
    }

}
