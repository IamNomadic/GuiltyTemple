using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]
    private GameObject patrolA;
    [SerializeField]
    private GameObject patrolB;
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;
    private Transform currentPatrolTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPatrolTarget = patrolB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPatrolTarget.position - transform.position;

        if (currentPatrolTarget == patrolB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < .2f && currentPatrolTarget == patrolA.transform)
        {
            currentPatrolTarget = patrolB.transform;
        }
        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < .2f && currentPatrolTarget == patrolB.transform)
        {
            currentPatrolTarget = patrolA.transform;
        }
    }
}
