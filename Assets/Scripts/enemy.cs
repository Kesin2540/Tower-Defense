using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int target = 0;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float navigationUpdate;
    [SerializeField] private float healthPoints;
    [SerializeField] private int rewardAmt;

    private Transform enemy;
    private Collider2D enemyCollider;
    private Animator animator;
    private float navigationTime = 0;
    private bool isDead = false;

    public bool IsDead
    {
        get { return isDead; }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(wayPoints != null && !isDead)
        {
            navigationTime += Time.deltaTime;
            if(navigationTime > navigationUpdate)
            {
                if(target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }

                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Waypoint")
        {
            target += 1;
        }
        else if(other.tag == "Finish")
        {
            GameManager.Instance.UnregisterEnemy(this);
        }
        else if (other.tag == "Projectiles")
        {
            Projectiles newP = other.gameObject.GetComponent<Projectiles>();
            enemyHit(newP.AttackStrength);
            Destroy(other.gameObject);
        }
        
    }
    public void enemyHit(int hitpoints)
    {
        if(healthPoints - hitpoints > 0)
        {
            //hurt animation is called
            healthPoints -= hitpoints;
            animator.Play("Hurt");
        }
        else
        {
            //the enemy dies here
            //die animation is called
            animator.SetTrigger("didDie");
            enemyDead();

        }
    }

    public void enemyDead()
    {
        isDead = true;
        enemyCollider.enabled = false;
    }
}
