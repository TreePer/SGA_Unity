using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;
    private GameObject JammoObj;

    private GameManager() { }

    private void Awake() {
        if (Instance == null)
            Instance = this;

        JammoObj = Resources.Load("Points/JammoPoint") as GameObject;

        new GameObject("PointList");

        DontDestroyOnLoad(this);
    }

    private int Count = 0;

    private IEnumerator Start() {
        Count = 1;
        

        while(true) {
            yield return null;

            GameObject obj = Instantiate(JammoObj);

            obj.AddComponent<WayPointController>();
            obj.transform.name = Count.ToString();
            obj.transform.position = new Vector3(0.0f, 0.0f, 0.0f + Count);
            obj.transform.parent = GameObject.Find("PointList").transform;

            ++Count;

            if (2 <= Count) {
                break;
            }
        }
    }
}
