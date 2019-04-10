using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

    private Enemy target;
    private Player player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();

            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if (other.gameObject.CompareTag("enemy"))
                {
                    target = hit.GetComponent<Enemy>();
                    target.currentState = EnemyState.stagger;
                    target.Knock(hit, knockTime, damage);
                }

                if (other.gameObject.CompareTag("Player"))
                {
                    player = hit.GetComponent<Player>();

                    if (player.currentState != PlayerState.stagger)
                    {
                        player.currentState = PlayerState.stagger;
                        player.Knock(knockTime, damage);
                    }
                }
            }
        }

        if (other.gameObject.CompareTag("breakable") && gameObject.CompareTag("Player"))
        {
            other.GetComponent<Pot>().Smash();
        }
    }
}
