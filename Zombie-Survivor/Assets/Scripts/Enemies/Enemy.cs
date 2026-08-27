using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int scoreAward = 10;
    [SerializeField] private GameObject deathParticleEffect;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;
    public int chanceToSpawn = 30;
    Rigidbody2D rb;
    private Transform target;
    private bool isFacingRight = true;
    private Score_Manager scoreManager;
    private Animator anim;
    public bool isDead;
    private Enemy_Manager enemyManager;
    private Pickup_Manager pickupManager;
    private Sound_Manager soundManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindAnyObjectByType<PlayerController>().transform;
        scoreManager = FindAnyObjectByType<Score_Manager>();
        anim = GetComponent<Animator>();
        enemyManager = FindAnyObjectByType<Enemy_Manager>();
        enemyManager.AddEnemy();
        pickupManager = FindAnyObjectByType<Pickup_Manager>();
        soundManager = FindAnyObjectByType<Sound_Manager>();
    }

    private void Update()
    {
        MoveToTarget();
    }

    //simply moves to the player (target)
    private void MoveToTarget()
    {
        if (target != null && !isDead)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // Update facing direction
            if (direction.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction.x < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

    //flips the facing direction
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        health -= damage;
        soundManager.PlaySoundEffect(hitSound, this.transform, 1f);
        anim.SetTrigger("isHit");
        if (health <= 0f)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isDead = true;
            rb.linearVelocity = Vector2.zero;
            Die();
            soundManager.PlaySoundEffect(deathSound, this.transform, 1f);
        }
    }

    public void RemoveTarget()
    {
        target = null;
    }

    private void Die()
    {
        scoreManager.AddScore(scoreAward);
        enemyManager.EnemyDead();
        anim.SetBool("isDead", true);
        pickupManager.CheckDropPickup(this.transform.position);
        GameObject effectInstance = Instantiate(deathParticleEffect, this.transform.position, Quaternion.identity);
        Destroy(effectInstance, 3f);
        Destroy(gameObject, 4f);
    }

    //damages player if the enemy collides
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isDead)
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
