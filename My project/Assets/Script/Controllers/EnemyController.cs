using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private LayerMask TargetMask;
    private List<Vector3> PointList = new List<Vector3>();

    //private int Count;
    private float Radius;

    float Angle = 0.0f;

    [Tooltip("장애물 감지 선 갯수")]
    [Range (5, 30)] public int Count = 0;

    [SerializeField] private List<Vector3> VertexList = new List<Vector3>();
    public Vector3 TargetPosition;


    void Start()
    {
        //Count = 5;
        Radius = 5.0f;

        Count = 10;

        //GameObject obj = 
    }

    void Update()
    {
        Angle = transform.eulerAngles.y - 45.0f;

        PointList.Clear();

        for (int i = 0; i < Count; ++i)
        {
            PointList.Add(new Vector3(Mathf.Sin(Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Angle * Mathf.Deg2Rad)));

            Angle += 90.0f / (Count - 1);
        }
        float fAngle = 0.0f;

        for (int i = 0; i < PointList.Count; ++i)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, PointList[i].normalized, out hit, Radius, TargetMask))
            {
                fAngle = Vector3.Angle(transform.forward, PointList[i]);

                fAngle *= (i > PointList.Count / 2 - 1) ? -2 : 2;
                
            }
        }

        transform.Rotate(transform.up * fAngle * Time.deltaTime);

        foreach (Vector3 Point in PointList)
            Debug.DrawLine(
                transform.position,
                (Point.normalized * Radius) + transform.position,
                Color.red);

        RayPoint();
    }

    public void RayPoint() { 
        
    }

    private void OnDrawGizmos() {

        for (int i = 0; i < VertexList.Count; ++i) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(VertexList[i] + TargetPosition, 0.2f);
        }
        
    }

}