using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public GameObject Fx;
    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject, 0.05f);

        GameObject obj = Instantiate(Fx);

        Vector3 Direction = (obj.transform.position - collision.transform.position).normalized;

        obj.transform.position = transform.position + (Direction * 2.0f);

        Destroy(obj.gameObject, 1.5f);
    }
}
