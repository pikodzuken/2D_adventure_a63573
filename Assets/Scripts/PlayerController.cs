using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public InputAction moveAction;
    Rigidbody2D rigidbody2D;
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;
    Vector2 move;
    public float speed = 3.0f;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;
     Animator animator;
    Vector2 moveDirection = new Vector2(1,0);
    // Start is called before the first frame update
    void Start()
    {
        moveAction.Enable();
        rigidbody2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y,0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        // Debug.Log("Object that entered the trigger: " + other);
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
    }
    void FixedUpdate()
    {
        Vector2 position = (Vector2)transform.position + move * speed * Time.fixedDeltaTime;
        rigidbody2D.MovePosition(position);
    }
    public void changeHealth(int amount = 1)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }
}