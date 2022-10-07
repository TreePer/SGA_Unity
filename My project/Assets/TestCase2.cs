using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCase2 : MonoBehaviour
{
    float angle = 2.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up, angle, Space.World);
        
    }
}
