using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {
    private void OnSceneGUI() {
        FieldOfView Target = (FieldOfView)target;

        Vector3 leftPoint = new Vector3(Mathf.Sin(Target.Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Target.Angle * Mathf.Deg2Rad));
        Vector3 rightPoint = new Vector3(Mathf.Sin(-Target.Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(-Target.Angle * Mathf.Deg2Rad));

        Handles.color = Color.red;
        Handles.DrawLine(Target.transform.position, Target.transform.position +  leftPoint * 10.0f);
        Handles.DrawLine(Target.transform.position, Target.transform.position + rightPoint * 10.0f);
    }
}

public class FieldOfView : MonoBehaviour {
    [Header("시야 각도")]
    [Range(0.1f, 180.0f)]
    public float Angle;

    [Header("시야 각도")]
    [Range(20, 100)]
    public int Count;

    private void Start() {
        Angle = 45.0f;
        Count = 25;
    }
    private void Update() {
        Angle = transform.eulerAngles.y - 45.0f;

        for (int i = 0; i < Count; ++i) {
            Debug.DrawRay(
                transform.position,
                new Vector3(Mathf.Sin(Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Angle * Mathf.Deg2Rad)),
                Color.blue);

            Angle += 90.0f / (Count - 1);
        }

        

        /*
        PointList.Clear();

        for (int i = 0; i < Count; ++i) {
            PointList.Add(new Vector3(Mathf.Sin(Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Angle * Mathf.Deg2Rad)));

            Angle += 90.0f / (Count - 1);
        }
        float fAngle = 0.0f;

        for (int i = 0; i < PointList.Count; ++i) {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, PointList[i].normalized, out hit, Radius, TargetMask)) {
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
         */
    }
}