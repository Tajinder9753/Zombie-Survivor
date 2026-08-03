using UnityEngine;

public class InputHandler : MonoBehaviour, PlayerInput.IPlayerActions
{
    public Vector2 movement;
    public bool isShooting;

    private PlayerInput playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInput();
        playerInputActions.Player.SetCallbacks(this);
    }
    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isShooting = true;
        }
        else if (context.canceled)
        {
            isShooting = false;
        }
    }
}
