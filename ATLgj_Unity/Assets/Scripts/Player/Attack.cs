using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public GameObject weapon;
    public float attackCooldown = 1.0f; // Time in seconds between attacks

    private float lastAttackTime;

    private void Start() {
        // Set the initial value of lastAttackTime to allow immediate attack
        lastAttackTime = Time.time - attackCooldown;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackCooldown) {
            // Check if enough time has passed since the last attack
            lastAttackTime = Time.time; // Update the last attack time

            GameObject clone = Instantiate(weapon, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation) as GameObject;
        }
    }
}
