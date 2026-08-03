using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler inputHandler;

    private bool isFacingRight = true;
    private Rigidbody2D rigidBody;
    private Vector2 moveVelocity;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private GameObject projectile;

    #region awake,update,start
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateMovement();
    }

    #endregion


    #region movement
    private void UpdateMovement()
    {
        //if movement accelerate to target velocity
        if (inputHandler.movement != Vector2.zero)
        {
            //check if needs to turn
            FlipCheck();

            Vector2 targetVelocity = inputHandler.movement * moveSpeed;
            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            rigidBody.linearVelocity = moveVelocity;
        }
        // if no input, decelerate to zero
        else if (inputHandler.movement == Vector2.zero)
        {
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
}
