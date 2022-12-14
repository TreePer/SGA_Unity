using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    public static NodeManager Instance = null;

    private NodeManager() { }

    [Range(-100.0f, 100.0f)]
    private float Height;

    private Vector3 StartNode;
    private Vector3 EndNode;

 

    private List<Vector3> Points = new List<Vector3>();

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        Height = 10.0f;
        StartNode = new Vector3(-75.0f, 0.0f, 0.0f);
        EndNode = new Vector3(-60.0f, 0.0f, 0.0f);
 

        BezierCurve();

    }
    void Update() {

        for (int i = 0; i < Points.Count - 1; ++i) {
            Debug.DrawLine(Points[i], Points[i + 1], Color.magenta);
        }
    }

    private void BezierCurve() {
        Vector3 temp = new Vector3(StartNode.x, StartNode.y, StartNode.z + Height);
        Vector3 dest = new Vector3(EndNode.x, EndNode.y, EndNode.z + Height);

        Vector3[] Nodes = new Vector3[5];

        float ratio = 0.0f;

        Points.Clear();

        Points.Add(StartNode);

        while (ratio < 1.0f) {
            ratio += Time.deltaTime;
            Nodes[0] = Vector3.Lerp(StartNode, temp, ratio);
            Nodes[1] = Vector3.Lerp(temp, dest, ratio);
            Nodes[2] = Vector3.Lerp(dest, EndNode, ratio);
            Nodes[3] = Vector3.Lerp(Nodes[0], Nodes[1], ratio);
            Nodes[4] = Vector3.Lerp(Nodes[1], Nodes[2], ratio);

            Points.Add(Vector3.Lerp(Nodes[3], Nodes[4], ratio));
        }

        Points.Add(EndNode); 
    }


    public static float NodeResult(List<Node> nodes) {
        float result = 0.0f;

        for(int i = 0; i < nodes.Count - 1; ++i) {
            result += Vector3.Distance(nodes[i].transform.position, nodes[i + 1].transform.position);
        }

        return result;
    }


    public static List<Vector3> GetVertices(GameObject obj) {
        List<Vector3> VertexList = new List<Vector3>();

        MeshFilter filter = obj.transform.GetComponent<MeshFilter>();

        if (filter != null) {
            Mesh mesh = filter.mesh;

            for (int i = 0; i < mesh.vertices.Length; ++i) {
                if (!VertexList.Contains(mesh.vertices[i]) && 0.0f > mesh.vertices[i].y) {
                    VertexList.Add(mesh.vertices[i]);
                }
            }
        }

        return VertexList;
    }

    public Node GetNode(GameObject Object, RaycastHit hit) {

        TestControoler test = Object.GetComponent<TestControoler>();
        Node front = test.OldTarget;
        Node end = front.next;


        GameObject obj = hit.transform.gameObject;
        List<Vector3> Vertices = GetVertices(obj);

        float[] frontDistance = new float[Vertices.Count];      
        float[] middleDistance = new float[Vertices.Count];     

        for(int i = 0; i < Vertices.Count; ++i) {
            frontDistance[i] = 0;
            middleDistance[i] = 0;
        }

        Vector3 middle = Vector3.Lerp(front.transform.position, end.transform.position, 0.3f);

        for (int i = 0; i < Vertices.Count; ++i) {
            frontDistance[i] += Vector3.Distance(front.transform.position, Vertices[i]);
            middleDistance[i] += Vector3.Distance(middle, Vertices[i]);
        }
        float fResult = 0.0f;
        if (frontDistance.Length > 0)
            fResult = frontDistance[0] + middleDistance[0];
        int index = 0;

        for (int i = 1; i < Vertices.Count; ++i) {
            if(fResult > frontDistance[i] + middleDistance[i]) {
                fResult = frontDistance[i] + middleDistance[i];
                index = i;
            }
        }

        List<Vector3> VertexList = new List<Vector3>();

        Vector3[] BottomPoint = new Vector3[Vertices.Count];

        for (int i = 0; i < BottomPoint.Length; ++i) {
            Matrix4x4 Matrix;
            Matrix4x4 RotMatrix;
            Matrix4x4 PosMatrix;
            Matrix4x4 ScaleMatrix;

            Vector3 eulerAngles = obj.transform.eulerAngles * Mathf.Deg2Rad;

            BottomPoint[i] = new Vector3(Vertices[i].x, 0.1f, Vertices[i].z);

            RotMatrix = MathManager.RotationX(eulerAngles.x) * MathManager.RotationY(eulerAngles.y) * MathManager.RotationZ(eulerAngles.z);
            PosMatrix = MathManager.Translate(obj.transform.position);
            ScaleMatrix = MathManager.Scale(obj.transform.lossyScale * 1.3f);

            Matrix = PosMatrix * RotMatrix * ScaleMatrix;

            VertexList.Add(Matrix.MultiplyPoint(Vertices[i]));
        }

        VertexList.Sort((temp, dest) => Vector3.Distance(hit.point, temp).CompareTo(Vector3.Distance(hit.point, dest)));

        Node node = front;


        for (int i = 0; i < 2; ++i) {
            GameObject CurrentObject = new GameObject("a" + i.ToString());
            CurrentObject.transform.parent = Object.transform.parent;
            CurrentObject.layer = 9;
            Node tempnode = CurrentObject.AddComponent<Node>();
            node.next = tempnode;
            CurrentObject.AddComponent<MyGizmo>();
            tempnode.transform.position = VertexList[i] + new Vector3 (0.0f, 3.0f, 0.0f);
            node = tempnode;
        }
            node.next = end;

        return front;
    }
}
