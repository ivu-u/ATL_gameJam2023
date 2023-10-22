using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// reference: https://enchantedexpression.com/2021/08/27/unity-3d-boomerang-weapon-throw-and-return/
public class Boomerang : MonoBehaviour
{
    private bool _throw;

    private GameObject player;
    private GameObject weapon;

    Transform itemToRotate; // weapon that is a child of the empty game object

    Vector3 throwDirection;
    public float throwSpeed; // The maximum distance the boomerang can travel
    public float throwDistance;

    private float height;
    public float scrollSensitivity;

    private void Start() {
        _throw = false;    // boomerang not reutning yet

        player = GameObject.Find("weapon");     //gameObject to return to
        weapon = GameObject.Find("weapon");   // the weapon

        weapon.GetComponent<MeshRenderer>().enabled = false; // turn off mesh rendereer to make weapon invisible

        itemToRotate = gameObject.transform;

        throwDirection = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z) + player.transform.forward * throwDistance;

        StartCoroutine(Boom());
    }

    IEnumerator Boom() {
        _throw = true;
        yield return new WaitForSeconds(1.5f);
        _throw = false;
    }

    private void Update() {
        itemToRotate.transform.Rotate(0, Time.deltaTime * 500, 0);

        if (_throw) {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            //height += scrollInput * scrollSensitivity;

            throwDirection.y += scrollInput * scrollSensitivity;

            //Debug.Log("throwdir y: " + throwDirection.y);

            transform.position = Vector3.MoveTowards(transform.position, throwDirection, Time.deltaTime * throwSpeed); // change the position to the location in front of player
        } else {
            // return to player
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Time.deltaTime * 40);
        }

        // once it's close to the player make weapon visible & destroy clone
        if (!_throw && Vector3.Distance(player.transform.position, transform.position) < 1.5) {
            weapon.GetComponent<MeshRenderer>().enabled = true;
            Destroy(this.gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision) {
    //    if (collision.transform.CompareTag("Enemy") && collision.transform.TryGetComponent(out Damageable damageable)) {
    //        Debug.Log("boss hit");
    //        damageable.ApplyDamage(new Damageable.DamageMessage() {
    //            damageAmount = 1,
    //            damageSource = transform.position,
    //        });
    //    }
    //}
}
