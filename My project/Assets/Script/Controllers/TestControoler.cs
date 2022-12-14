using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControoler : MonoBehaviour {

    public Node Target;
    public Node OldTarget;
    public LayerMask NodeMask;

    void Start() {

        NodeMask |= 128;
        Transform trans = transform.parent.transform;
        Target = trans.GetChild(0).GetComponent<Node>() as Node;
        this.transform.position = Target.transform.position;
    }

    void Update() {
        Vector3 dir = Target.transform.position - transform.position;

        transform.position += dir.normalized * Time.deltaTime * 5.0f;
        /*
        float fDistance = Vector3.Distance(transform.position , Target.transform.position);
        if (fDistance < 0.05)
            Target = Target.next;
         */
        
        RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, NodeMask)) {
                if(Vector3.Distance(transform.position, hit.point) <= 1.5f) {
                    Target = NodeManager.Instance.GetNode(this.gameObject, hit);
                }
            }
        transform.LookAt(Target.transform);
    }


    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Node>() == Target) {
            OldTarget = Target;
            Target = Target.next;
        }
        
    }
}
