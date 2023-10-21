using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAttack : MonoBehaviour {
    [SerializeField] private Transform boomerangPoint;
    [SerializeField] private GameObject boomerang;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Instantiate(boomerang, boomerangPoint);
        }
    }
    
    public void increaseSpin(float amt) {
        damage += amt;
    }
}
