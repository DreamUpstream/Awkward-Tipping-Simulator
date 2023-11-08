using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float lifetime;

    private Rigidbody2D _rig;
    private bool moving = true;

    private void Awake()
    {
        Debug.Log("awake");
        _rig = GetComponent<Rigidbody2D>();
    }
    
    private void Start() {}

    private void FixedUpdate()
    {
        Debug.Log("Update");
        Debug.Log(_rig);
        if (moving) {
            _rig.velocity = transform.up * speed;
            speed -= 0.02f;
        }

        if (speed <= 0) {
            moving = false;
        }
    }
}