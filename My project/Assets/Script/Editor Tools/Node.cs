using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class Node : MonoBehaviour {

    [HideInInspector] public Node node;
    private bool check;

    private void Awake() {
        Rigidbody rigid = GetComponent<Rigidbody>();
        SphereCollider coll = GetComponent<SphereCollider>();

        rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        coll.radius = 0.2f;
    }

    IEnumerator Start() {
        check = true;
        while (check) {
            transform.position = new Vector3(Random.Range(-15, 15), 25.0f, Random.Range(-15, 15));

            yield return new WaitForSecondsRealtime(5.0f);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.name == "Ground")
            check = false;
    }


}
