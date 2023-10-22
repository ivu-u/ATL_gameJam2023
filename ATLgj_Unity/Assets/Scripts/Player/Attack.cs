using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject weapon;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            GameObject clone;
            clone = Instantiate(weapon, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
        }
    }
}
