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
    public float Vib;
    public float Radius;

    public LayerMask TargetMask;

    // Start is called before the first frame update
    void Start() {
        Speed = 5.0f;
        Power = 0;
        Vib = 0;
        Radius = 0;
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
            Vib = 0;
        }
        if(Input.GetKey(KeyCode.Space)) {
            Power += Time.deltaTime;
            Radius += Time.deltaTime;
            Vib += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space)) {
            RaycastHit hit;
            if (Physics.Raycast(FirePoint.position, FirePoint.transform.forward, out hit, 100.0f, TargetMask)) {

                Vector3 offset = new Vector3(
                    Mathf.Cos(90 * Mathf.Deg2Rad),
                    Mathf.Sin(90 * Mathf.Deg2Rad),
                    1.0f) * Radius + FirePoint.position;
                    

                Vector3 Vibration = new Vector3(
                    Random.Range(-0.2f, 0.2f),
                    Random.Range(-0.2f, 0.2f),
                    0.0f
                    ) * Vib;

                Debug.Log(Vibration);
                Debug.DrawLine(FirePoint.position, offset + Vibration, Color.red);
                /*
                if(hit.transform.tag != "Bullet") {
                    GameObject obj = Instantiate(BulletPrefab);

                    obj.transform.position = hit.point;
                }
                 */

                GameObject obj = Instantiate(BulletPrefab);

                obj.transform.position = offset + Vibration;


            }

            /*
            GameObject obj = Instantiate(BulletPrefab);

            obj.transform.position = FirePoint.position;

            Rigidbody Rigid = obj.GetComponent<Rigidbody>();
            Rigid.AddForce(FirePoint.transform.forward * Power * 1000);
             */
        }
    }
}