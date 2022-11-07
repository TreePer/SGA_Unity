using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class FieldOfView : MonoBehaviour {
    [Header("시야 각도")]
    [Range(0.1f, 180.0f)]
    public float Angle;

    [Header("시야 갯수")]
    [Range(20, 200)]
    public int Count;

    [Header("길이")]
    [Range(5.0f, 100.0f)]
    public float Radius;

    [HideInInspector] public List<Vector3> ViewList = new List<Vector3>();
    public List<Transform> TargetList = new List<Transform>();

    
   [SerializeField] private LayerMask Mask;
   [SerializeField] private LayerMask TargetMask;

    private MeshFilter meshFilter;
    private Mesh mesh;

    private void Awake() {
        GameObject obj = new GameObject("View");

        obj.transform.parent = this.transform;
        obj.transform.position = transform.position;
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        meshFilter = obj.AddComponent<MeshFilter>();

        Material material = Resources.Load("Materials/Mesh") as Material;
        renderer.material = material;

        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    private void Start() {
        Angle = 45.0f;
        Count = 25;
        Radius = 10.0f;
    }
    private void Update() {

        FieldView();

        Collider[] CollObj = Physics.OverlapSphere(transform.position, Radius, TargetMask);

        TargetList.Clear();
        
        foreach(Collider coll in CollObj) {
            Vector3 Direction = (coll.transform.position - transform.position).normalized;
            
            if(Vector3.Angle(transform.forward, Direction) < Angle) {
                float fDistance = Vector3.Distance(transform.position, coll.transform.position);

                if(!Physics.Raycast(transform.position, Direction, fDistance, Mask)) {
                    TargetList.Add(coll.transform);
                }
            }
        }
    }

    public void FieldView() {
        float fAngle = (-Angle);

        ViewList.Clear();

        for (int i = 0; i < Count; ++i) {
            Vector3 point = new Vector3(Mathf.Sin(fAngle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(fAngle * Mathf.Deg2Rad)).normalized;

            Ray ray = new Ray(transform.position, point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Radius, Mask)) {
                ViewList.Add(hit.point - transform.position);
            }
            else {
                ViewList.Add(point * Radius);
            }

            fAngle += (Angle * 2) / (Count - 1);
        }

        Vector3[] vertices = new Vector3[ViewList.Count + 1];
        int[] triangles = new int[(ViewList.Count - 1) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < ViewList.Count; ++i) {
            vertices[i + 1] = ViewList[i];
        }

        for (int i = 0; i < ViewList.Count - 1; ++i) {
            vertices[i + 1] = ViewList[i];

            for (int j = 0; j < 3; ++j) {
                triangles[i * 3 + j] = j == 0 ? 0 : i + j;
            }
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.name = "mesh";
        mesh.RecalculateNormals();
    }
}

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {
    private void OnSceneGUI() {
        FieldOfView Target = (FieldOfView)target;

        foreach(Transform Info in Target.TargetList) {
            Debug.DrawLine(Target.transform.position, Info.transform.position, Color.red);
        }



        Handles.color = Color.red;
        Handles.DrawWireArc(Target.transform.position, Target.transform.up, Target.transform.forward, 360.0f, Target.Radius);

        foreach (Vector3 point in Target.ViewList) {
            Debug.DrawLine(Target.transform.position, Target.transform.position + point, Color.blue);
        }

        Vector3 leftPoint = new Vector3(Mathf.Sin(Target.Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Target.Angle * Mathf.Deg2Rad));
        Vector3 rightPoint = new Vector3(Mathf.Sin(-Target.Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(-Target.Angle * Mathf.Deg2Rad));

        Handles.DrawLine(Target.transform.position, Target.transform.position + leftPoint * Target.Radius);
        Handles.DrawLine(Target.transform.position, Target.transform.position + rightPoint * Target.Radius);
    }
}