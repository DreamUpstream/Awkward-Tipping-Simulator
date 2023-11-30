using System.Collections;
using UnityEngine;
using Pathfinding;
using System.IO;

public class Waiter : MonoBehaviour
{

    public enum State
    {
        Work,
        Chase,
        CheckPlayer,
        Alert,
        Enraged,
    }

    protected State currentState;

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float chaseDistance;
    [SerializeField] protected float alertRange = 10f;
    [SerializeField] protected int awareness = 1;

    [SerializeField] protected GameObject[] workingPositions;
    protected GameObject currentTarget;


    // We need to set target for AIDestinationSetter for the AI to move
    protected AIDestinationSetter target;

    // Not used atm.
    protected float targetDistance;

    // Class which calculates the path
    private Seeker seeker;
    private Pathfinding.Path path;
    [SerializeField] private AIPath aiPath;

    private int currentWaypoint = 0;
    private bool isPathSet = false;


    // [Header("Components")]
    // [SerializeField] protected SpriteRenderer spriteRenderer;

    void Start()
    {
        target = FindObjectOfType<AIDestinationSetter>();
        currentState = State.Work; // Work or Enranged works

        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        EnableMeleeEnemy();
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);

        // spriteRenderer.flipX = GetTargetDirection().x < 0;

        switch (currentState)
        {
            case State.Work:
                Work();
                break;
            case State.CheckPlayer:
                CheckPlayer();
                break;
            case State.Alert:
                Alert();
                break;
            case State.Enraged:
                Enraged();
                break;
            default:
                break;
        }
    }

    public void EnableMeleeEnemy()
    {
        StartCoroutine(MakeEnemyYellow(2));
        StartCoroutine(EnableMeleeEnemyAfterSeconds(4));
    }

    IEnumerator MakeEnemyYellow(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    IEnumerator EnableMeleeEnemyAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<MeleeEnemy>().enabled = true;
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    protected void Work()
    {

        // set default values(?) 
        if (currentTarget == null || target.target == null)
        {
            currentTarget = workingPositions[currentWaypoint];
            target.target = workingPositions[currentWaypoint].transform;
        }

        // Rotate through positions
        if (aiPath.reachedEndOfPath || aiPath.remainingDistance <= 0.5f)
        {
            currentWaypoint++;
            if (currentWaypoint >= workingPositions.Length)
            {
                currentWaypoint = 0;
            }

            currentTarget = workingPositions[currentWaypoint];
            target.target = workingPositions[currentWaypoint].transform;
        }

    }


    protected void CheckPlayer()
    {
        // if (targetDistance < chaseDistance)
        // {
        //     currentState = State.Chase;
        // }
        // else if (targetDistance < alertRange)
        // {
        //     currentState = State.Alert;
        //     StartCoroutine(AlertOtherWaiters());
        // }
        // else
        // {
        //     currentState = State.Work;
        // }
    }

    protected void Alert()
    {

    }


    // Function which should alert nearby waiters 
    protected IEnumerator AlertOtherWaiters()
    {
        Waiter[] waiters = FindObjectsOfType<Waiter>();

        foreach (Waiter otherWaiter in waiters)
        {
            if (otherWaiter == this)
            {
                continue;
            }

            float distanceToOtherWaiter = Vector2.Distance(transform.position, otherWaiter.transform.position);
            if (distanceToOtherWaiter <= alertRange)
            {
                otherWaiter.UpdateAwarenessLevel();
            }
        }

        yield return null;
    }

    protected void Enraged()
    {
        // For now if waiter is Enraged, it always follows the player. 
        // Can be done only if it's in range, and otherwise maybe switches to angry wander
        // Where he randomly selects a position and pathfinds towards it unless the player gets 
        // in the range again
        var player = FindObjectOfType<Player>();
        if (player != null)
        {
            target.target = player.transform.transform;
            currentTarget = player.gameObject;
        }
    }


    // protected Vector2 GetTargetDirection()
    // {
    //     return (target.transform.position - transform.position).normalized;
    // }


    protected void UpdateAwarenessLevel()
    {
        awareness++;

        Debug.Log("[Waiter.cs] (UpdateAwarnessLevel): Updated awarness level " + awareness);
    }

    IEnumerator UpdatePath()
    {
        while (true)
        {
            RecalculatePath();
            yield return new WaitForSeconds(1f);
        }
    }

    void RecalculatePath()
    {
        isPathSet = false; // Reset the flag

        if (currentTarget == null)
        {
            return;
        }

        // this if statement might be unnecessary. (made by gpt)
        if (currentState == State.Work || currentState == State.Chase || currentState == State.Enraged)
        {
            // Calculate a new path to the target position
            seeker.StartPath(transform.position, currentTarget.transform.position, OnPathComplete);
        }
    }


    void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error && p.IsDone())
        {
            path = p;

            // Check if the path has been completely calculated before using it
            if (aiPath != null && !isPathSet)
            {
                // There is a slight issue with this. It works, but
                // you can see an error appear in console sometimes
                aiPath.SetPath(p);
                isPathSet = true;
            }
        }
    }

}
