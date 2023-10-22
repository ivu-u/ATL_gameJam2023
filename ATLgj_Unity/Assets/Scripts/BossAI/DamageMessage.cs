using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Damageable : MonoBehaviour
{
    /// <summary>
    /// The information that's passed to the damage method
    /// </summary>
    public struct DamageMessage {
        public MonoBehaviour damager;
        public int damageAmount;
        public Vector3 direction;
        public Vector3 damageSource;
        public bool throwing;
        public bool stopCamera;
    }
}
