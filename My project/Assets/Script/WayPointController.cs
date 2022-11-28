using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointController : MonoBehaviour {

    void Start() {
        int Count = Random.Range(6, 10);

        for (int i = 0; i < Count; ++i) {
            GameObject obj = new GameObject(i.ToString());
            obj.transform.SetParent(this.transform);
            obj.AddComponent<Node>();
            
            if(i > 1) {
                Node frontNode = transform.GetChild(i - 1).GetComponent<Node>();
                Node Node = transform.GetChild(i).GetComponent<Node>();

                frontNode.next = Node;
                Node.next = transform.GetChild(0).GetComponent<Node>();
            }
             

            obj.AddComponent<MyGizmo>();
        }

    }
}
