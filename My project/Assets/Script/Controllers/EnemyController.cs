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

        //transform.position += transform.forward * 5.0f * Time.deltaTime;


        foreach (Vector3 Point in PointList)
            Debug.DrawLine(
                transform.position,  // 시작점.
                (Point.normalized * Radius) + transform.position,  // 도착지점.
                Color.red); // 라인 색

        RayPoint();
    }

    public void RayPoint() {
        
        //Ray ray = new Ray(transform.position, transform.forward);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            TargetPosition = hit.transform.position;
            MeshFilter filter = hit.transform.GetComponent<MeshFilter>();

            if (filter != null) {
                Mesh mesh = filter.mesh;
                Vector3[] vertices = new Vector3[mesh.vertices.Length];

                for (int i = 0; i < mesh.vertices.Length; ++i)
                    vertices[i] = mesh.vertices[i];

                List<Vector3> Temp = new List<Vector3>();

                for (int i = 0; i < vertices.Length; ++i) {
                    if (!Temp.Contains(vertices[i]) && hit.transform.position.y > vertices[i].y) {
                        Temp.Add(vertices[i]);
                    }
                }

                VertexList.Clear();

                Vector3[] BottomPoint = new Vector3[4];
                for (int i = 0; i < BottomPoint.Length; ++i) {
                    Matrix4x4 Matrix;
                    Matrix4x4 RotMatrix;
                    Matrix4x4 PosMatrix;
                    Matrix4x4 ScaleMatrix;

                    Vector3 eulerAngles = hit.transform.eulerAngles * Mathf.Deg2Rad;
                        
                    BottomPoint[i] = new Vector3(Temp[i].x, 0.1f, Temp[i].z);
                    
                    RotMatrix = RotationX(eulerAngles.x) * RotationY(eulerAngles.y) * RotationZ(eulerAngles.z);
                    PosMatrix = Translate(hit.transform.position);
                    ScaleMatrix = Scale(hit.transform.lossyScale * 1.5f);

                    Matrix = PosMatrix * RotMatrix * ScaleMatrix;

                    VertexList.Add(Matrix.MultiplyPoint(BottomPoint[i]));
                }
            }
        }
        else
            TargetPosition = new Vector3();
    }

    private void OnDrawGizmos() {

        for (int i = 0; i < VertexList.Count; ++i) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(VertexList[i] + TargetPosition, 0.2f);
        }
        
    }

    public Matrix4x4 Translate(Vector3 position) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m03 = position.x;
        matrix.m13 = position.y;
        matrix.m23 = position.z;


        return matrix;
    }

    public Matrix4x4 RotationX(float _angle) {
        Matrix4x4 matrix = Matrix4x4.identity;


        matrix.m11 = Mathf.Cos(_angle);
        matrix.m12 = -Mathf.Sin(_angle);
        matrix.m21 = Mathf.Sin(_angle);
        matrix.m22 = Mathf.Cos(_angle);


        return matrix;
    }

    public Matrix4x4 RotationY(float _angle) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = Mathf.Cos(_angle);
        matrix.m02 = Mathf.Sin(_angle);
        matrix.m20 = -Mathf.Sin(_angle);
        matrix.m22 = Mathf.Cos(_angle);

        return matrix;
    }

    public Matrix4x4 RotationZ(float _angle) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = Mathf.Cos(_angle);
        matrix.m01 = -Mathf.Sin(_angle);
        matrix.m10 = Mathf.Sin(_angle);
        matrix.m11 = Mathf.Cos(_angle);

        return matrix;
    }

    public Matrix4x4 Scale(Vector3 scale) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = scale.x;
        matrix.m11 = scale.y;
        matrix.m22 = scale.z;
        matrix.m33 = 1;

        return matrix;
    }

}
