using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log("hit smthing");
        Debug.Log(other.tag);

        if (other.CompareTag("Enemy") && other.transform.TryGetComponent(out Damageable damageable)) {
            Debug.Log("boss hit");
            damageable.ApplyDamage(new Damageable.DamageMessage() {
                damageAmount = 1,
                damageSource = transform.position,
            });
        }
    }
}
