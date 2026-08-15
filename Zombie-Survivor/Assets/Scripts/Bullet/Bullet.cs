using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float timeBeforeDie = 2f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float angleOffset = 270f;
    Rigidbody2D rb;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //fires the bullet in the given direction
    public void Fire(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);

        Destroy(gameObject, timeBeforeDie);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //damage the enemy if collides with it
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
