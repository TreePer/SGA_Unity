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


    void Start()
    {
        //Count = 5;
        Radius = 15.0f;

        Count = 10;
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

        transform.position += transform.forward * 5.0f * Time.deltaTime;

        foreach (Vector3 Point in PointList)
            Debug.DrawLine(
                transform.position,  // 시작점.
                (Point.normalized * Radius) + transform.position,  // 도착지점.
                Color.red); // 라인 색
    }
}
