using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointController : MonoBehaviour {

    void Start() {
        int Count = 2;

        for (int i = 0; i < Count; ++i) {
            GameObject obj = new GameObject(i.ToString());
            obj.transform.SetParent(this.transform);
            obj.AddComponent<Node>();
            obj.AddComponent<MyGizmo>();
            obj.GetComponent<Node>().SetIndex(i);
            obj.layer = 9;
            obj.transform.position = new Vector3(Random.Range(-15, 15), 3.0f, Random.Range(-15, 15));
            if (i > 0) {
                Node frontNode = transform.GetChild(i - 1).GetComponent<Node>();
                Node Node = transform.GetChild(i).GetComponent<Node>();

                frontNode.next = Node;
                Node.next = transform.GetChild(0).GetComponent<Node>();
            }
        }

    }
}