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
    [SerializeField] private Vector3 _camDirection;
    private bool _isGrounded = true;
    private Transform _groundChecker;
    private Camera _cam;

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
        float facing = _cam.transform.eulerAngles.y;
        _turnedInputs = Quaternion.Euler( 0, facing, 0) * _inputs;


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
        _camDirection = _turnedInputs;
        if (_camDirection.z < 0) {
            _targetDirection = new Vector3(_camDirection.x, 0, _camDirection.z * -1);
        } else {
            _targetDirection = new Vector3(_camDirection.x, 0, _camDirection.z);
        }

        if (_inputs != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(_targetDirection),
                Time.deltaTime * turnSpeed);
        }
        
        if (_isGrounded) {
            if (_camDirection.z < 0) {
                _body.velocity = new Vector3(_targetDirection.x, _targetDirection.y, _targetDirection.z * -1) * speed;
            } else {
                _body.velocity = _targetDirection.normalized * speed;    
            }
        } else {
            if (_camDirection.z < 0) {
                _body.AddForce(new Vector3(_targetDirection.x, _targetDirection.y, _targetDirection.z * -1) * (speed * 10), ForceMode.Force);   
            } else {
                _body.AddForce(_targetDirection.normalized * (speed * 10), ForceMode.Force);   
            }
        }
    }
}
