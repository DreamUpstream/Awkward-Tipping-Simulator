using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [Header("Components")] 
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private MouseUtilities mouseUtilities;
    [SerializeField] private Animator animator;

    private Vector2 _moveInput;
    private Vector2 _previousInput;

    private void Update()
    {
        Vector2 mouseDirection = mouseUtilities.GetMouseDirection(transform.position);

        if (_moveInput.x == 0 && _moveInput.y == 0)
        {
            animator.SetFloat("speed", 0);
            spriteRenderer.flipX = mouseDirection.x < 0;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _moveInput * moveSpeed;
        rig.velocity = velocity;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (_previousInput != _moveInput)
        {
            if (_moveInput.x > 0.5)
            {
                animator.SetFloat("dir_x", 1);
                animator.SetFloat("dir_y", 0);
                animator.SetFloat("speed", 1);
                return;
            }
            if (_moveInput.x < -0.5)
            {
                animator.SetFloat("dir_x", -1);
                animator.SetFloat("dir_y", 0);
                animator.SetFloat("speed", 1);
                return;
            }
            if (_moveInput.y < -0.5)
            {
                animator.SetFloat("dir_x", 0);
                animator.SetFloat("dir_y", 1);
                animator.SetFloat("speed", 1);
                return;
            }
            if (_moveInput.y > 0.5)
            {
                animator.SetFloat("dir_x", 0);
                animator.SetFloat("dir_y", -1);
                animator.SetFloat("speed", 1);
            }

            _previousInput = _moveInput;
        }
    }
}
