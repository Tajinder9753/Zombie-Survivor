using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeBeforeDie = 2f;
    Rigidbody2D rb;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float angleOffset = 270f;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Fire(Vector2 direction)
    {
        // Add logic to fire the bullet
        Debug.Log("Firing Bullet");

        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);

        Destroy(gameObject, timeBeforeDie);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
