using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class Enemy : Character
{
    public enum State
    {
        Idle,
        Chase,
        Attack
    }
    
    protected State CurState;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float chaseDistance;

    [SerializeField] protected ItemData[] dropItems;
    [SerializeField] protected GameObject dropItemPrefab;

    [SerializeField] private Animator animator;
    protected Vector2 prev_dir;

    protected GameObject Target;

    protected float LastAttackTime;
    protected float TargetDistance;

    [Header("Components")] 
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        Target = FindObjectOfType<Player>().gameObject;
    }

    protected virtual void Update()
    {
        TargetDistance = Vector2.Distance(transform.position, Target.transform.position);

        spriteRenderer.flipX = GetTargetDirection().x < 0;
        
        switch (CurState)
        {
            case State.Idle: IdleUpdate();
                break;
            case State.Chase: ChaseUpdate();
                break;
            case State.Attack: AttackUpdate();
                break;
            
        }
    }

    void ChangeState(State newState)
    {
        CurState = newState;
    }

    void IdleUpdate()
    {
        animator.SetFloat("speed", 0);
        if(TargetDistance<=chaseDistance)
            ChangeState(State.Chase);
            
    }

    void ChaseUpdate()
    {
        if(InAttackRange())
            ChangeState(State.Attack);
        else if(TargetDistance>chaseDistance)
            ChangeState(State.Idle);
        
        transform.position =
            Vector3.MoveTowards(transform.position, Target.transform.position, moveSpeed * Time.deltaTime);

        Vector2 velocity = GetTargetDirection();
        Vector2 direction = GetNormalizedDirection(velocity);
        if (direction != prev_dir)
        {
            ProcessAnimation(direction);
            prev_dir = direction;
        }
    }

    void AttackUpdate()
    {
        if(TargetDistance>chaseDistance)
        {
            ChangeState(State.Idle);
        }
        else if(!InAttackRange())
            ChangeState(State.Chase);
        if (CanAttack())
        {
            LastAttackTime = Time.time;
            AttackTarget();
        }
        
    }

    protected virtual void AttackTarget()
    {
        
    }

    protected virtual bool CanAttack()
    {
        return false;
    }

    protected virtual bool InAttackRange()
    {
        return false;
    }

    protected Vector2 GetTargetDirection()
    {
        return (Target.transform.position - transform.position).normalized;
    }

    public override void Die()
    {
        // DropItems();
        // Destroy(gameObject);
    }

    protected void DropItems()
    {
        // Debug.Log("Drop items");
        // for (int i = 0; i < dropItems.Length; i++)
        // {
        //     GameObject obj = Instantiate(dropItemPrefab, transform.position, quaternion.identity);
        //     obj.GetComponent<WorldItem>().SetItem(dropItems[i]);
        // }
    }

    public override void TakeDamage(int damageToTake) {
        Debug.Log("take damage");    

        StartCoroutine(FreezeAndUnfreeze());
    }

    IEnumerator FreezeAndUnfreeze() {
        var oldSpeed = moveSpeed;
        moveSpeed = 0;
        yield return new WaitForSeconds(0.5f);
        moveSpeed = oldSpeed;
    }

    protected void ProcessAnimation(Vector2 direction)
    {
        animator.SetFloat("dir_x", direction.x);
        animator.SetFloat("dir_y", direction.y);
        animator.SetFloat("speed", 1);
    }

    private Vector2 GetNormalizedDirection(Vector2 velocity)
    {
        Vector2 direction;
        if (velocity.x > 0.5)
        {
            direction.x = 1; /// Idk why, but it works normally when both x are 1...
            direction.y = 0;
        }
        else if (velocity.x < -0.5)
        {
            direction.x = 1;
            direction.y = 0;
        }
        else if (velocity.y > 0.5)
        {
            direction.x = 0;
            direction.y = -1;
        }
        else if (velocity.y < -0.5)
        {
            direction.x = 0;
            direction.y = 1;
        }
        else
        {
            direction.x = 0;
            direction.y = 0;
        }
        return direction;
    }
}
