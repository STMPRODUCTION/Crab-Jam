using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrabController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Collider2D attackCollider;
    public Animator animator;
    public Transform cameraTransform;

    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        controls = new PlayerControls();

        // Bind the Move action
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Bind the Attack action
        controls.Player.Attack.performed += ctx => Attack();

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        // Get the camera's forward direction and ignore the y component
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        // Calculate the direction to move based on camera's forward direction
        Vector3 moveDirection = (cameraForward * moveInput.y + cameraTransform.right * moveInput.x).normalized;

        // Apply movement
        rb.velocity = new Vector2(moveDirection.x, moveDirection.z) * moveSpeed;

        // Update the speed parameter in the animator
        animator.SetFloat("speed", rb.velocity.magnitude);

        // Rotate the crab to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * moveSpeed);
        }
    }

    private void Attack()
    {
        // Trigger the attack animation
        animator.SetTrigger("crab_attack");

        // Activate the collider
        StartCoroutine(ActivateAttackCollider());
    }

    private IEnumerator ActivateAttackCollider()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        attackCollider.enabled = false;
    }
}
