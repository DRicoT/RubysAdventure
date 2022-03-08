using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class RubyController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
    private Vector2 position, move;
    public int maxHealth = 5;
    public int health
    {
        get { return currentHealth; }
    }
    private int currentHealth;
    

    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float invincibleTimer;

    [SerializeField] private float speed = 3.0f;

    [SerializeField] private GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
    }
    
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);
        if (Mathf.Approximately(move.x, 0f) || Mathf.Approximately(move.y, 0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", lookDirection.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
                Debug.Log(invincibleTimer);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }

    private void FixedUpdate()
    {
        position = _rigidbody2D.position;
        position = position + move * speed * Time.deltaTime;
        _rigidbody2D.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible) return;
            isInvincible = true;
            invincibleTimer = timeInvincible;

        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, _rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 100f);
        
        animator.SetTrigger("Launch");
    }
}
