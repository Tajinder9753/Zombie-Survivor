using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private InputHandler inputHandler;

    private bool isFacingRight = true;
    private Rigidbody2D rigidBody;
    private Vector2 moveVelocity;
    private float bulletTimer = 0f;
    private Animator animator;
    private bool canTakeDamage = true;

    [SerializeField] private Slider healthBar;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform firingPoint;


    #region awake,update,start
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    private void Update()
    {
        UpdateMovement();
        ResetTimer();
    }

    #endregion


    #region movement
    private void UpdateMovement()
    {
        //if movement accelerate to target velocity
        if (inputHandler.movement != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            //check if needs to turn
            FlipCheck();

            Vector2 targetVelocity = inputHandler.movement * moveSpeed;
            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            rigidBody.linearVelocity = moveVelocity;
        }
        // if no input, decelerate to zero
        else if (inputHandler.movement == Vector2.zero)
        {
            animator.SetBool("isMoving", false);
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rigidBody.linearVelocity = moveVelocity;

        }
    }
    //check if need to flip
    private void FlipCheck()
    {
        if (isFacingRight && moveVelocity.x < 0)
        {
            Flip(false);

        }
        else if (!isFacingRight && moveVelocity.x > 0)
        {
            Flip(true);
        }
    }
    //flip based on move direction
    private void Flip(bool turnRight)
    {
        if (turnRight)
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = true;
        }
        else
        {
            transform.Rotate(0, -180, 0);
            isFacingRight = false;
        }
    }

    #endregion

    #region shooting

    private void Shoot()
    {
        bulletTimer = fireRate;
        GameObject bullet = Instantiate(projectile, firingPoint.position, firingPoint.rotation);
        bullet.GetComponent<Bullet>().damage = damage;
        bullet.GetComponent<Bullet>().Fire(GetAimDirection());
    }

    private Vector2 GetAimDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 objPos = Camera.main.WorldToScreenPoint(firingPoint.position);
        return (mousePos - objPos).normalized;
    }

    #endregion

    #region Damage

    public void TakeDamage(float damage)
    {
        if (!canTakeDamage) return;
        health -= damage;
        UpdateHealthBar();
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Transform[] childTransforms = transform.GetComponentsInChildren<Transform>();
        foreach (Transform childTransform in childTransforms)
        {
            if (childTransform != transform)
            {
                Destroy(childTransform.gameObject);
            }
        }
        Enemy[] enemies = FindObjectsByType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.RemoveTarget();
        }
        animator.SetTrigger("Dead");
        Menu_Manager menuManager = FindAnyObjectByType<Menu_Manager>();
        menuManager.ShowGameOverPanel();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = health;
    }

    #endregion

    #region timers

    private void ResetTimer()
    {
        if (bulletTimer > 0f)
        {
            bulletTimer -= Time.deltaTime;
        }
        else if (inputHandler.isShooting && bulletTimer <= 0f)
        {
            Shoot();
        }
    }
    #endregion

    #region PickupEffects

    public void GetHealth(float amount, bool isPermanent)
    {
        if (isPermanent)
        {
            maxHealth += amount;
            healthBar.maxValue = maxHealth;
        }
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthBar();
    }

    public void ActivateSpeedBoost(float speedMultiplier, float duration, bool isPermanent)
    {
        if (isPermanent)
        {
            moveSpeed += speedMultiplier;
            return;
        }
        moveSpeed += speedMultiplier;
        StartCoroutine(TemporarySpeedBoost(speedMultiplier, duration));
    }

    private IEnumerator TemporarySpeedBoost(float speedMultiplier, float duration)
    {
        yield return new WaitForSeconds(duration);
        moveSpeed -= speedMultiplier;
    }

    public void ActivateInvulnerability(float duration)
    {
        StartCoroutine(TemporaryInvulnerability(duration));
    }

    private IEnumerator TemporaryInvulnerability(float duration)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(duration);
        canTakeDamage = true;
    }

    public void ActivateFireRateBoost(float fireRateMultiplier,float duration, bool isPermanent)
    {
        if (isPermanent)
        {
            fireRate += fireRateMultiplier;
            return;
        }

        fireRate += fireRateMultiplier;
        StartCoroutine(TemporaryFireRateIncrease(fireRateMultiplier, duration));
    }

    private IEnumerator TemporaryFireRateIncrease(float fireRateMultiplier, float duration)
    {
        yield return new WaitForSeconds(duration);
        fireRate -= fireRateMultiplier;
    }

    #endregion
}
