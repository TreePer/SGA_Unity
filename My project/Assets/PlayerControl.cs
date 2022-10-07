using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private float Speed;
    private Rigidbody Rigid = null;
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public float Power;

    // Start is called before the first frame update
    void Start() {
        Speed = 5.0f;
        Power = 0;
    }

    private void Awake() {
        Rigid = GetComponent<Rigidbody>();
        
    }
    private void FixedUpdate() {
        
    }

    private void Reset() {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localScale  = new Vector3(0.0f, 0.0f, 0.0f);
        
    }

    // Update is called once per frame
    void Update() {
        float fHor = Input.GetAxis("Horizontal");
        float fVer = Input.GetAxis("Vertical");

        Vector3 Movement = new Vector3(fHor, 0.0f, fVer) * Speed * Time.deltaTime;
        transform.position += Movement;
        
        //Vector3 Movment = new Vector3(fHor, 0.0f, fVer);
        //transform.position += Movment * Speed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space)) {
            Power = 0;
        }
        if(Input.GetKey(KeyCode.Space)) {
            Power += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            
            GameObject obj = Instantiate(BulletPrefab);

            obj.transform.position = FirePoint.position;

            Rigidbody Rigid = obj.GetComponent<Rigidbody>();
            Rigid.AddForce(FirePoint.transform.forward * Power * 1000);
        }
    }
}