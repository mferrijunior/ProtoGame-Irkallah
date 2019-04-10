using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public enum PlayerDirection
{
    up,
    down,
    left,
    right
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public float speed;
    public PlayerState currentState;
    public PlayerDirection currentDirection;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;
    public SpriteValue playerSprite;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    private Rigidbody2D myRigidbody;
    private Animator animator;
    private Vector3 movement;
    private SpriteRenderer spriteRenderer;
    private bool canMove;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("move_x", 0);
        animator.SetFloat("move_y", -1);
        SetCanMove(true);
        currentState = PlayerState.idle;
        currentDirection = PlayerDirection.down;
        transform.position = startingPosition.initialValue;
        spriteRenderer.sprite = playerSprite.initialValue;
    }

    void Update()
    {
        movement = Vector3.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger && currentState != PlayerState.interact)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    public void SetState(PlayerState state)
    {
        currentState = state;
    }

    public void SetCanMove(bool canPlayerMove)
    {
        canMove = canPlayerMove;
    }

    public void SetAnimatorBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void RaiseItem()
    {
        if (currentState == PlayerState.interact && !animator.GetBool("receive_item"))
        {
            animator.SetBool("receive_item", true);
            receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
        }
        else
        {
            receivedItemSprite.sprite = null;
        }
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
    }

    public void Knock(float knockTime, float damage)
    {
        TakeDamage(damage);
        playerHealthSignal.Raise();

        if (currentHealth.runtimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth.runtimeValue -= damage;

        if (currentHealth.runtimeValue <= 0)
        {
            currentHealth.runtimeValue = 0;
        }
    }

    void UpdateAnimationAndMove()
    {
        if (canMove && movement != Vector3.zero)
        {
            MoveCharacter();
            UpdatePlayerDirection();
            animator.SetFloat("move_x", movement.x);
            animator.SetFloat("move_y", movement.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        movement.Normalize();
        myRigidbody.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }

    IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null; // Waits 1 frame
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.3f);
        currentState = PlayerState.walk;
    }

    IEnumerator KnockCo(float knockTime)
    {
        yield return new WaitForSeconds(knockTime);
        myRigidbody.velocity = Vector2.zero;
        currentState = PlayerState.idle;
        myRigidbody.velocity = Vector2.zero;
    }

    void UpdatePlayerDirection()
    {
        if ((int)movement.x == -1)
        {
            currentDirection = PlayerDirection.left;
        }

        if ((int)movement.x == 1)
        {
            currentDirection = PlayerDirection.right;
        }

        if ((int)movement.y == -1)
        {
            currentDirection = PlayerDirection.down;
        }

        if ((int)movement.y == 1)
        {
            currentDirection = PlayerDirection.up;
        }
    }
}