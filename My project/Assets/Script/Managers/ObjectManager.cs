using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    [HideInInspector] public static ObjectManager Instance = null;

    [HideInInspector] public Dictionary<string, GameObject> ObjectList = new Dictionary<string, GameObject>();


    private ObjectManager() { }

    private void Awake() {
        if (Instance == null)
            Instance = this;

        object[] Object = Resources.LoadAll("Prefabs/Objects");

        foreach (GameObject Element in Object)
            ObjectList.Add(Element.name, Element);

        DontDestroyOnLoad(this);
    }
}
