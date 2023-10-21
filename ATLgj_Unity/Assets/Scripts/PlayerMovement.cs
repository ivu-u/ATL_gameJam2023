using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDelay;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private LayerMask ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    [SerializeField] private bool _isGrounded = true;
    private Transform _groundChecker;
    private Camera _cam;

    void Start() {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        _cam = Camera.main;
    }

    void Update() {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        
        Vector3 camDirection = _cam.transform.rotation * _inputs; //This takes all 3 axes (good for something flying in 3d space)    
        Vector3 targetDirection = new Vector3(camDirection.x, 0, camDirection.z); //This line removes the "space ship" 3D flying effect. We take the cam direction but remove the y axis value
        
        if (_inputs != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(targetDirection),
                Time.deltaTime * turnSpeed);
        }

        if (Input.GetButtonDown("Jump") && _isGrounded) {
            StartCoroutine("jump");
        }
    }

    IEnumerator jump() {
        yield return new WaitForSeconds(jumpDelay);
        Vector3 dashVelocity = Vector3.Scale(transform.forward, 
            dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime),
                0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
        _body.AddForce(new Vector3(dashVelocity.x, 1 * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), dashVelocity.z), ForceMode.VelocityChange);
    }
    void FixedUpdate() {
        _body.MovePosition(_body.position + _inputs.normalized * (speed * Time.fixedDeltaTime));
    }
}
