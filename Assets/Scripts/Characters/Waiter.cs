using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public abstract class Waiter : Character
{

    public enum State
    {
        Work,
        Chase,
        CheckPlayer,
        Alert,
        Enreanged,
    }

    protected State CurrentState;

    [SerializeField] protected int awarners = 0;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float chaseDistance;


    [SerializeField] protected GameObject[] tables;
    [SerializeField] protected GameObject restPosition;

    protected GameObject Target;

    protected float TargetDistance;

    [Header("Components")]
    [SerializeField] protected SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Target = FindObjectOfType<Player>().gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        TargetDistance = Vector2.Distance(transform.position, Target.transform.position);

        spriteRenderer.flipX = GetTargetDirection().x < 0;

        switch (CurrentState)
        {
            case State.CheckPlayer:
                CheckPlayer();
                break;

            default:
                break;
        }

    }

    protected void CheckPlayer()
    {

    }

    protected Vector2 GetTargetDirection()
    {
        return (Target.transform.position - transform.position).normalized;
    }
}
