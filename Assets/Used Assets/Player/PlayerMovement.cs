using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;

    private Vector2 rawInput;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        var moveAction = playerInput.actions["Move"];
        moveAction.performed += ctx => rawInput = ctx.ReadValue<Vector2>();
        moveAction.canceled  += ctx => rawInput = Vector2.zero;
    }

    private void Update()
    {
        if (Time.timeScale == 0 || WiseNPCInteractable.IsNPCUIOpen)
            return;

        // Snap to horizontal or vertical only
        if (rawInput != Vector2.zero)
        {
            if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
            {
                moveInput = new Vector2(Mathf.Sign(rawInput.x), 0f);
            }
            else
            {
                moveInput = new Vector2(0f, Mathf.Sign(rawInput.y));
            }
        }
        else
        {
            moveInput = Vector2.zero;
        }

        bool moving = moveInput != Vector2.zero;

        if (moving)
            lastMoveDir = moveInput;

        // Feed blend tree parameters
        Vector2 dir = moving ? moveInput : lastMoveDir;
        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);
        animator.SetBool("Moving", moving);
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0 || WiseNPCInteractable.IsNPCUIOpen)
            return;

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
