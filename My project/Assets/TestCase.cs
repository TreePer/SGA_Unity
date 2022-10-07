using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCase : MonoBehaviour
{
    private float Angle = 0.5f;
 
    void Update() {
        
        transform.RotateAround(transform.position, Vector3.up, Angle);
        
    }
}
