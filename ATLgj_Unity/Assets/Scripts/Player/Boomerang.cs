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

    Vector3 locationInFrontOfPlayer;

    private void Start() {
        _throw = false;    // boomerang not reutning yet

        player = GameObject.Find("Player");     //gameObject to return to
        weapon = GameObject.Find("weapon");   // the weapon

        weapon.GetComponent<MeshRenderer>().enabled = false; // turn off mesh rendereer to make weapon invisible

        itemToRotate = gameObject.transform;

        // adjust the location of the player | here we add to the Y pos so the weapon doesn't go too low | pick a location infront of player
        locationInFrontOfPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z) + player.transform.forward * 10f;

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
            transform.position = Vector3.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * 40); // change the position to the location in front of player
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
}
