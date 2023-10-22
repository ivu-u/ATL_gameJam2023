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
    private Vector3 _turnedInputs;
    private Vector3 _targetDirection;
    //[SerializeField] private Vector3 _camDirection;
    private bool _isGrounded = true;
    private Transform _groundChecker;
    private Camera _cam;

    public GameObject test;

    void Start() {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        _cam = Camera.main;
        _inputs = Vector3.zero;
    }

    void Update() {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");

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
        float facing = test.transform.eulerAngles.y;
        _turnedInputs = Quaternion.Euler(0, facing, 0) * _inputs;

        float dotProduct = Vector3.Dot(transform.forward, _body.velocity);  // if the character is facing forward

        //if (dotProduct < 0) {
        //    _targetDirection = new Vector3(_turnedInputs.x, 0, _inputs.z);
        //}
        //else {
            _targetDirection = new Vector3(_turnedInputs.x, 0, _turnedInputs.z);
        //}

        if (_inputs != Vector3.zero && dotProduct >= 0) {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(_targetDirection),
                Time.deltaTime * turnSpeed);
        }
        
        if (_isGrounded) {
            if (_turnedInputs.z < 0) {
                _body.velocity = new Vector3(_targetDirection.x, _targetDirection.y, _targetDirection.z) * speed;
            } else {
                _body.velocity = _targetDirection.normalized * speed;    
            }
        } else {
            if (_turnedInputs.z < 0) {
                _body.AddForce(new Vector3(_targetDirection.x, _targetDirection.y, _targetDirection.z) * (speed * 10), ForceMode.Force);   
            } else {
                _body.AddForce(_targetDirection.normalized * (speed * 10), ForceMode.Force);   
            }
        }
    }
}
