using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;

    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        var moveAction = playerInput.actions["Move"];
        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;
    }

    private void Update()
    {
        if (Time.timeScale == 0 || WiseNPCInteractable.IsNPCUIOpen)
            return;

        // normalize diagonal
        if (moveInput.sqrMagnitude > 1f)
            moveInput = moveInput.normalized;

        bool moving = moveInput != Vector2.zero;

        // if we just moved, remember that direction
        if (moving)
            lastMoveDir = moveInput;

        // choose which direction to feed into the blend‑trees:
        Vector2 dir = moving ? moveInput : lastMoveDir;

        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);
        animator.SetBool("Moving", moving);

        //RotateTowards(dir);
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0 || WiseNPCInteractable.IsNPCUIOpen)
            return;

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    //private void RotateTowards(Vector2 dir)
    //{
    //    if (dir == Vector2.zero) return;
    //    float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(0f, 0f, -angle);
    //}
}
