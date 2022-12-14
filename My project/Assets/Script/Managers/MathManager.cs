using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathManager : MonoBehaviour {

    static public Matrix4x4 Translate(Vector3 position) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m03 = position.x;
        matrix.m13 = position.y;
        matrix.m23 = position.z;


        return matrix;
    }

    static public Matrix4x4 RotationX(float _angle) {
        Matrix4x4 matrix = Matrix4x4.identity;


        matrix.m11 = Mathf.Cos(_angle);
        matrix.m12 = -Mathf.Sin(_angle);
        matrix.m21 = Mathf.Sin(_angle);
        matrix.m22 = Mathf.Cos(_angle);


        return matrix;
    }

    static public Matrix4x4 RotationY(float _angle) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = Mathf.Cos(_angle);
        matrix.m02 = Mathf.Sin(_angle);
        matrix.m20 = -Mathf.Sin(_angle);
        matrix.m22 = Mathf.Cos(_angle);

        return matrix;
    }

    static public Matrix4x4 RotationZ(float _angle) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = Mathf.Cos(_angle);
        matrix.m01 = -Mathf.Sin(_angle);
        matrix.m10 = Mathf.Sin(_angle);
        matrix.m11 = Mathf.Cos(_angle);

        return matrix;
    }

    static public Matrix4x4 Scale(Vector3 scale) {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = scale.x;
        matrix.m11 = scale.y;
        matrix.m22 = scale.z;
        matrix.m33 = 1;

        return matrix;
    }
}
