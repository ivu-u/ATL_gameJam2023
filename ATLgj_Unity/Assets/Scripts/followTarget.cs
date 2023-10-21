using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTarget : MonoBehaviour {
    [SerializeField] private Transform look;
    [SerializeField] private float offset;

    // Update is called once per frame
    void Update() {
        transform.LookAt(look.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, look.transform.position, 5 * Time.deltaTime);
    }
}
