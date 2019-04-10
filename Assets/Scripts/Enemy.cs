using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger,
    sleeping,
    dead
}

public class Enemy : MonoBehaviour
{
    public EnemyState currentState;
    public float health;
    public FloatValue maxHealth;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    void Awake()
    {
        health = maxHealth.initialValue;
    }

    public void Knock(Rigidbody2D myRigidBody, float knockTime)
    {
        StartCoroutine(KnockCo(myRigidBody, knockTime));
    }

    public void Knock(Rigidbody2D myRigidBody, float knockTime, float damage)
    {
        if (health > 0)
        {
            StartCoroutine(KnockCo(myRigidBody, knockTime));
            TakeDamage(damage);
        }
    }

    IEnumerator KnockCo(Rigidbody2D myRigidBody, float knockTime)
    {
        if (myRigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidBody.velocity = Vector2.zero;
        }
    }

    protected void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            gameObject.SetActive(false);
        }
    }
}
