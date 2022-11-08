using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLine : MonoBehaviour {

    [Range(-100.0f, 100.0f)]
    private float Height;

    private Vector3 StartNode;
    private Vector3 EndNode;

    private List<Vector3> Points = new List<Vector3>();

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
}
