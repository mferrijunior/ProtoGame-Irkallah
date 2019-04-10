using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class LogEnemy : Enemy
{
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    private Rigidbody2D myRigidBody;
    private Animator anim;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myRigidBody = GetComponent<Rigidbody2D>();
        currentState = EnemyState.sleeping;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.walk || currentState == EnemyState.sleeping && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeAnimation(temp - transform.position);
                myRigidBody.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("sleeping", false);
                anim.SetBool("wake_up", true);
            }
            else
            {
                StartCoroutine(StopStaggerCo());
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            anim.SetBool("wake_up", false);
            anim.SetBool("sleeping", true);
            ChangeState(EnemyState.sleeping);
        }
    }

    void ChangeAnimation(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                SetAnimFloat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                SetAnimFloat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimFloat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimFloat(Vector2.down);
            }
        }
    }

    void SetAnimFloat(Vector2 setVector)
    {
        anim.SetFloat("move_x", setVector.x);
        anim.SetFloat("move_y", setVector.y);
    }

    IEnumerator StopStaggerCo()
    {
        yield return new WaitForSeconds(0.4f);
        ChangeState(EnemyState.walk);
    }
}
