using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager Instance = null;

    private EnemyManager() { }

    //[SerializeField]private List<GameObject> EnemyList { get; set; }

    private void Awake() {
        if (Instance == null)
            Instance = this;

        new GameObject("EnemyList");
    }

    private List<GameObject> EnemyList = new List<GameObject>();

    public void AddEnemy(GameObject Obj) {
        EnemyList.Add(Obj);
    }

    
}
