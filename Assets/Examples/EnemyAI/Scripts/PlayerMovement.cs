using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputs;

public class PlayerMovement : MonoBehaviour, IGameplayActions
{
    public float speed = 1f;
    private new Rigidbody rigidbody;
    private Vector2 movementInput;
    Vector3 movementDirection;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
        movementDirection = new Vector3(movementInput.x, 0f, movementInput.y);
        rigidbody.AddForce(movementDirection * speed, ForceMode.VelocityChange);

    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = Vector2.ClampMagnitude(context.ReadValue<Vector2>(), 1f);
    }


}
