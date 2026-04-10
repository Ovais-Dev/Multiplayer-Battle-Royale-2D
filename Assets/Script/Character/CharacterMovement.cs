using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class CharacterMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Sprint Setting")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashStopDime = .2f;
    [SerializeField] private float dashCoolDownTime = 0.05f;
    private bool isDashing = false;

    [Header("Animation Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float animationFrameRate = 0.1f; // Time between frames

    // Animation structure for each direction
    [System.Serializable]
    public struct DirectionAnimation
    {
        public string directionName;
        public Sprite[] idleFrames;    // Single frame or multiple for idle animation
        public Sprite[] walkFrames;     // Walking animation frames
    }

    [Header("Animation Frames")]
    [SerializeField] private DirectionAnimation upAnimation;
    [SerializeField] private DirectionAnimation downAnimation;
    [SerializeField] private DirectionAnimation rightAnimation; // Base right animation

    // input mapping
    private InputAction moveAction;
    private InputAction dashAction;
    private Vector2 moveInput;
    // Animation state variables
    private Vector2 lastMoveDirection = Vector2.down; // Default facing down
    private DirectionAnimation currentAnimation;

    // Animation timing
    private float animationTimer = 0f;
    private int currentFrameIndex = 0;
    private bool isMoving = false;

    private Vector2 lastAnimatedDirection;

    private NetworkVariable<Vector2> netMoveDir = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> netIsMoving = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);

    Rigidbody2D rb2D;
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        dashAction = InputSystem.actions.FindAction("Sprint");
        // Auto-get sprite renderer if not assigned
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Set default animation
        SetAnimationDirection(lastMoveDirection);
        lastAnimatedDirection = lastMoveDirection;

        if (IsOwner)
            netMoveDir.Value = lastMoveDirection;

        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!IsSpawned) return;
        if (IsOwner)
        {
            HandleMovement();
        }

        HandleAnimation();
    }

    void HandleMovement()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        if (dashAction.WasPressedThisFrame())
        {
            StartCoroutine(Dashing(lastMoveDirection)); // LOCAL
            DashServerRpc(lastMoveDirection); // SYNC
        }
        if (isDashing) return;
        if (moveInput.magnitude > 0.01f)
        {
            Vector2 newDirection;
            // Update last move direction
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Horizontal movement
                newDirection = new Vector2(Mathf.Sign(moveInput.x), 0);
            }
            else
            {
                // Vertical movement
                newDirection = new Vector2(0, Mathf.Sign(moveInput.y));
            }

            // Move the character
            rb2D.linearVelocity = (Vector3)moveInput.normalized * moveSpeed;
            isMoving = true;
           
            // Set animation based on move direction
            if(newDirection != lastMoveDirection)
            {
                lastMoveDirection = newDirection;
                netMoveDir.Value = newDirection;
            }
        }
        else
        {
            rb2D.linearVelocity = Vector2.zero;
            isMoving = false;
        }
        netIsMoving.Value = isMoving;
    }

    void HandleAnimation()
    {
        Vector2 direction = netMoveDir.Value;
        bool isMoving = netIsMoving.Value;

        if ((direction - lastAnimatedDirection).magnitude > 0.01f)
        {
            lastAnimatedDirection = direction;
            SetAnimationDirection(direction);
        }

        animationTimer += Time.deltaTime;

        if (animationTimer >= animationFrameRate)
        {
            animationTimer = 0f;

            // Choose correct frame array based on movement state
            Sprite[] currentFrames = isMoving ? currentAnimation.walkFrames : currentAnimation.idleFrames;

            if (currentFrames != null && currentFrames.Length > 0)
            {
                // Loop through frames
                currentFrameIndex = (currentFrameIndex + 1) % currentFrames.Length;
                spriteRenderer.sprite = currentFrames[currentFrameIndex];
            }
        }
    }

    #region Dashing Controls
    [ServerRpc]
    void DashServerRpc(Vector2 dir)
    {
        StartCoroutine(Dashing(dir));
    }
    IEnumerator Dashing(Vector2 direction)
    {
        isDashing = true;
        rb2D.linearVelocity = direction.normalized * dashSpeed;
        yield return new WaitForSeconds(dashStopDime);
        rb2D.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(dashCoolDownTime);
        isDashing = false;
    }
    #endregion
    void SetAnimationDirection(Vector2 direction)
    {
        // Determine which animation set to use
        if (direction.y > 0.5f)
        {
            // Facing up
            currentAnimation = upAnimation;
            spriteRenderer.flipX = false;
        }
        else if (direction.y < -0.5f)
        {
            // Facing down
            currentAnimation = downAnimation;
            spriteRenderer.flipX = false;
        }
        else if (direction.x > 0.5f)
        {
            // Facing right (no flip needed)
            currentAnimation = rightAnimation;
            spriteRenderer.flipX = false;
        }
        else if (direction.x < -0.5f)
        {
            // Facing left (flip the right animation)
            currentAnimation = rightAnimation;
            spriteRenderer.flipX = true; // Flip horizontally for left
        }

        // Reset animation frame when changing direction
        currentFrameIndex = 0;
        animationTimer = 0f;
    }

    // Optional: Public method to change animation sets at runtime
    public void SetAnimationFrames(DirectionAnimation up, DirectionAnimation down, DirectionAnimation right)
    {
        upAnimation = up;
        downAnimation = down;
        rightAnimation = right;
    }
}