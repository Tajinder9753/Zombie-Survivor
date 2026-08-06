using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int scoreAward = 10;
    Rigidbody2D rb;
    private Transform target;
    private bool isFacingRight = true;
    private Score_Manager scoreManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindAnyObjectByType<PlayerController>().transform;
        scoreManager = FindAnyObjectByType<Score_Manager>();
    }


    private void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if (target != null)
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void RemoveTarget()
    {
        target = null;
    }

    private void Die()
    {
        scoreManager.AddScore(scoreAward);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
