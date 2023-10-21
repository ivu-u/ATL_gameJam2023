using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Boomerang : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 10);
    }
}
