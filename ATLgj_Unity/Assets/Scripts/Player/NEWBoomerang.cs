using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWBoomerang : MonoBehaviour {
    private bool _throw;
    private GameObject player;
    private GameObject weapon;
    private Transform itemToRotate;

    private Vector3 throwDirection;
    public float throwSpeed;
    public float throwDistance;
    public float scrollSensitivity;

    private void Start() {
        _throw = false;
        player = GameObject.Find("Player"); // Assuming "Player" is the name of your player GameObject
        weapon = GameObject.Find("weapon"); // The weapon you want to make visible

        weapon.GetComponent<MeshRenderer>().enabled = false;

        itemToRotate = transform; // Assuming this script is attached to the boomerang GameObject

        // Calculate the initial throw direction based on the player's position and forward direction
        throwDirection = player.transform.position + player.transform.forward * throwDistance;

        StartCoroutine(Boom());
    }

    IEnumerator Boom() {
        _throw = true;
        yield return new WaitForSeconds(1.5f);
        _throw = false;
    }

    private void Update() {
        itemToRotate.Rotate(0, Time.deltaTime * 500, 0);

        if (_throw) {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            throwDirection.y += scrollInput * scrollSensitivity;

            // Move the boomerang towards the throw direction
            transform.position = Vector3.MoveTowards(transform.position, throwDirection, Time.deltaTime * throwSpeed);
        }
        else {
            // Return to the player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 40);
        }

        // Once it's close to the player, make the weapon visible and destroy the boomerang
        if (!_throw && Vector3.Distance(player.transform.position, transform.position) < 1.5) {
            weapon.GetComponent<MeshRenderer>().enabled = true;
            Destroy(gameObject);
        }
    }

    // OnCollisionEnter is a Unity callback method that is triggered when colliders make contact
    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Enemy") && collision.transform.TryGetComponent(out Damageable damageable)) {
            Debug.Log("Enemy hit");
            damageable.ApplyDamage(new Damageable.DamageMessage() {
                damageAmount = 1,
                damageSource = transform.position,
            });
        }
    }
}
