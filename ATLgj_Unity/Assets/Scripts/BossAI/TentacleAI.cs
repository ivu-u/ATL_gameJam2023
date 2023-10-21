using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// reference: https://learn.unity.com/tutorial/using-animation-rigging-damped-transform?uv=2019.3&projectId=5f9350ffedbc2a0020193331

/// <summary>
/// Basic enemy tentacle AI
/// </summary>
public class TentacleAI : MonoBehaviour, IMessageReceiver {
    [Header("Pathing")]
    [SerializeField] CinemachineSmoothPath path = default;
    [SerializeField] CinemachineDollyCart cart = default;
    [SerializeField] LayerMask terrainLayer = default;  // default so it doesn't filter any layers
    PlayerMovement player;
    // ui here

    [HideInInspector] public Vector3 startPos, endPos;

    RaycastHit hitInfo;
    int totalHealth;
    int currentHealth;
    // Damageable[] damageables;        // look at later

    private void Start() {
        // set ui
        //set damageables

        currentHealth = totalHealth;

        player = Object.FindObjectOfType<PlayerMovement>();

        AI();
    }

    private void AI() {

    }

    /// <summary>
    /// Determins two random postions in the world for the start and end points, but uses player position for the middle point of the arc
    /// </summary>
    private void UpdatePath() {
        Vector3 playerPos = player.transform.position; // might have to add ridgid body stuff here
        playerPos.y = Mathf.Max(10, playerPos.y);
        Vector3 randomRange = Random.insideUnitSphere * 100;
        randomRange.y = 0;
        startPos = playerPos + randomRange;
        endPos = playerPos - randomRange;

        // checks if the end/start postions touch ground, and if it does set the postions there
        if (Physics.Raycast(startPos, Vector3.down, out hitInfo, 1000, terrainLayer.value)) {
            startPos = hitInfo.point;
        }

        if (Physics.Raycast(endPos, Vector3.down, out hitInfo, 1000, terrainLayer.value)) {
            endPos = hitInfo.point;
            // GroundDetection.Invoke(false, hitInfo.transform.CompareTag("Terrain") ? 0 : 1);      // unsure what this does yet
        }

        path.m_Waypoints[0].position = startPos + (Vector3.down * 15);
        path.m_Waypoints[1].position = playerPos+ (Vector3.up * 10);
        path.m_Waypoints[2].position = endPos + (Vector3.down * 45);

        path.InvalidateDistanceCache();
        cart.m_Position = 0;

        // speed
        cart.m_Speed = cart.m_Path.PathLength / 1500;

        // OnBossReveal.invoke for event
    }

    public void OnRecieveMessage(MessageType type, object sender, object msg) {
        throw new System.NotImplementedException();
    }
}
