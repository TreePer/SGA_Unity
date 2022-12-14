using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Node : MonoBehaviour {

    public Node next;
    private int index;
    private bool check;
    private Rigidbody rigid;
    private SphereCollider coll;

    public Node() { }
    public Node(int index) { this.index = index; }

    public int GetIndex() { return index; }
    public void SetIndex(int index) { this.index = index; }


    private void Awake() {
        
        coll = GetComponent<SphereCollider>();

        rigid = GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        coll.radius = 0.2f;

    }

    private void Update() {
        Debug.DrawLine(transform.position, next.transform.position, Color.black);
    }

    IEnumerator Start() {
        check = true;
        /*
        while (check) {
            transform.position = new Vector3(Random.Range(-15, 15), 25.0f, Random.Range(-15, 15));
            transform.tag = "Node";
            

        }
         */
            yield return null;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.name == "Ground")
            check = false;
        //rigid.isKinematic = true;
        rigid.useGravity = false;
        coll.isTrigger = true;
    }


}
