using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public partial class Damageable : MonoBehaviour {
    public int maxHitPoints;
    public float invulnerabilityTime;

    [Tooltip("The angle from the which that damageable is hitable. Always in the world XZ plane, with the forward being rotate by hitForwardRoation")]
    [Range(0.0f, 360.0f)]
    public float hitAngle = 360.0f;

    [Tooltip("Allow to rotate the world forward vector of the damageable used to define the hitAngle zone")]
    [Range(0.0f, 360.0f)]
    public float hitForwardRotation = 360.0f;

    public bool isInvunerable {  get; set; }
    public int currentHitpoints;

    public UnityEvent OnDeath, OnRecieveDamage, OnHitWhileInvunerable, OnBecomeVunerable, OnResetDamage;

    [Tooltip("When this gameObject is damaged, these other gameObjects are notified.")]
    [EnforceType(typeof(IMessageReceiver))]
    List<MonoBehaviour> onDamageMessageRecievers;

    protected float m_timeSinceLastHit = 0.0f;
    protected Collider m_Collider;

    System.Action schedule;

    // IMessageReceiver mono
    public void addToListeners() {
        Debug.Log("test");
        //onDamageMessageRecievers.Add(mono);
    }

    private void Start() {
        ResetDamage();
        m_Collider = GetComponent<Collider>();
        onDamageMessageRecievers = new List<MonoBehaviour>();
    }

    private void Update() {
        m_timeSinceLastHit += Time.deltaTime;
        if (m_timeSinceLastHit > invulnerabilityTime) {
            m_timeSinceLastHit = 0.0f;
            isInvunerable = false;
            OnBecomeVunerable.Invoke();
        }
    }

    public void ResetDamage() {
        currentHitpoints = maxHitPoints;
        isInvunerable = false;
        m_timeSinceLastHit = 0.0f;
        OnResetDamage.Invoke();
    }

    public void SetColliderState(bool enabled) {
        m_Collider.enabled = enabled;
    }

    public void ApplyDamage(DamageMessage data) {
        if (currentHitpoints < 0) { return;  }  // do nothing if dead

        if (isInvunerable) {
            OnHitWhileInvunerable.Invoke();
            return;
        }

        // determine whether the player is in the hitZone
        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        // project the direction to the damager to the plane formed by direction of damage
        Vector3 positionToDamager = data.damageSource - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f) { return; }

        isInvunerable = true;
        currentHitpoints -= data.damageAmount;

        if (currentHitpoints <= 0) {
            OnDeath.Invoke();
        }
        else {
            OnRecieveDamage.Invoke();
        }

        var messageType = currentHitpoints <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

        // send message to all recievers
        Debug.Log(onDamageMessageRecievers.Count);
        for (var i = 0; i < onDamageMessageRecievers.Count; ++i) {
            Debug.Log(onDamageMessageRecievers[i]);
            var reciever = onDamageMessageRecievers[i] as IMessageReceiver;
            reciever.OnRecieveMessage(messageType, this, data);
        }
    }

    // not sure what this does yet
    private void LateUpdate() {
        if (schedule != null) {
            schedule();
            schedule = null;
        }
    }
}
