/*
 * Robert Krawczyk, Jaden Pleasants
 * Project 5
 * Dash and get BIG every few seconds
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidCell : MonoBehaviour
{
    // References
    public GameObject player;
    Rigidbody2D rb;

    // Settings
    public float restingScale = 1;
    public float attackScale = 1.6f;
    public float attackTransitionTime = .2f;
    public float attackDuration = 1.5f;
    public float attackForce = 350;
    public float attackCooldown = 3.5f;
    public float sightRange = 10;

    // Backend
    float curr_attackCooldown = 3, curr_attackTime = 0;
    bool attacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Eventually remove this and have the Spawn Manager set it for me, to save on computation
        player = GameObject.Find("/Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Time to start attack?
        curr_attackCooldown -= Time.deltaTime;
        if (curr_attackCooldown <= 0)
        {
            curr_attackCooldown = attackCooldown;
            // If player in range, attack
            if (Vector2.Distance(transform.position, player.transform.position) <= sightRange)
            {
                StartAttack();
            }
        }

        // While attacking
        if (attacking)
        {
            curr_attackTime += Time.deltaTime;

            if (curr_attackTime <= attackTransitionTime)
            {
                // Getting BIG
                transform.localScale = Vector3.one * Mathf.Lerp(restingScale, attackScale, (curr_attackTime - 0) / attackTransitionTime);
            }
            else if (curr_attackTime > attackDuration && curr_attackTime <= attackDuration + attackTransitionTime)
            {
                // Getting back to normal
                transform.localScale = Vector3.one * Mathf.Lerp(attackScale, restingScale, (curr_attackTime - attackDuration) / attackTransitionTime);
            }
            else if (curr_attackTime > attackDuration + attackTransitionTime)
            {
                // Back to normal
                attacking = false;
                transform.localScale = Vector3.one;
            }
        }
    }

    void StartAttack()
    {
        // Dash
        rb.AddForce(attackForce * Vector3.Normalize(player.transform.position - transform.position));

        attacking = true;
        curr_attackTime = 0;
    }
}
