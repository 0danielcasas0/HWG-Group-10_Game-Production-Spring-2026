// Gives access to core Unity engine features
using UnityEngine;

// Gives access to the New Input System
using UnityEngine.InputSystem;



// Defines a component that can be attached to a GameObject
public class PlayerMovement : MonoBehaviour
{
    #region === Movement Settings ===
    // Groups movement-related variables in the Inspector
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;                 // Base walking speed
    public Rigidbody rb;                         // Reference to the player's Rigidbody
    private Vector2 MoveInput;                   // Stores movement input (WASD / left stick)
    #endregion

    #region === Look Settings ===
    // Groups look-related variables in the Inspector
    [Header("Look Settings")]
    public float MouseSensitivity = 0.1f;        // Controls mouse look sensitivity
    public Transform CameraTransform;             // Reference to the camera transform
    private Vector2 lookInput;                   // Stores mouse movement input
    private float xRotation = 0f;                // Vertical camera rotation (pitch)
    #endregion

    #region === Jump Settings ===
    // Groups jump-related variables in the Inspector
    [Header("Jump Settings")]
    public float JumpHeight = 2f;                // Desired jump height
    public bool IsGrounded;                      // Tracks if the player is touching the ground
    #endregion

    #region === Sprint Settings ===
    // Groups sprint-related variables in the Inspector
    [Header("Sprint Settings")]
    public float SprintMultiplier = 2f;          // Speed multiplier while sprinting
    private float currentSpeedMultiplier = 1f;  // Active speed multiplier
    #endregion

    #region === Crouch Settings ===
    // Groups crouch-related variables in the Inspector
    [Header("Crouch Settings")]
    public float CrouchHeight = 1f;              // Player height while crouching
    public float NormalHeight = 2f;              // Player height while standing
    public float CrouchSpeedMultiplier = 0.5f;  // Speed reduction while crouching
    #endregion

    #region === Stamina Settings ===
    // Groups stamina-related variables in the Inspector
    [Header("Stamina Settings")]
    public float MaxStamina = 100f;              // Maximum stamina value
    public float CurrentStamina;                 // Current stamina amount
    public float StaminaDrainRate = 20f;         // Stamina drained per second while sprinting
    public float StaminaRegenRate = 15f;         // Stamina regenerated per second
    public float RegenThreshold = 20f;           // Stamina required before sprinting again
    private bool IsSprinting;                    // Tracks if sprint input is held
    private bool IsExhausted;                    // Prevents sprinting when stamina hits zero
    #endregion

    #region === Interaction Settings ===
    // Groups interaction-related variables in the Inspector
    [Header("Interaction Settings")]
    public float interactRange = 3f;
    public LayerMask interactableLayer; // Assign this in Inspector!
    #endregion

    #region === Unity Lifecycle ===
    // Reference PlayerStats for potential future use (e.g. checking if player is caught or has key)
    private PlayerStats playerStats;

    // Called once when the script starts
    void Start()
    {
        rb = GetComponent<Rigidbody>();           // Gets the Rigidbody attached to this GameObject
        Cursor.lockState = CursorLockMode.Locked; // Locks the mouse to the game window
        playerStats = GetComponent<PlayerStats>(); // Gets the PlayerStats component on the same GameObject
    }

    // Called once per frame
    void Update()
    {
        HandleLook();                             // Handles camera and player rotation
    }

    // Called at a fixed time step (used for physics)
    void FixedUpdate()
    {
        HandleMove();                             // Handles movement physics
        HandleStamina();                          // Handles stamina drain and regen
    }

    #endregion

    #region === Input Callbacks ===

    // Input callback methods are called by the Unity Input System when the corresponding input actions are triggered

    // OnInteract check what the player is looking at and interact with it if possible (e.g. picking up keys, opening doors)
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            Debug.Log("Interact button pressed, checking for interactables...");
            
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 2f);

            // Added QueryTriggerInteraction.Collide to include Triggers in the raycast
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }

    // Called by the Input System when movement input changes
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>(); // Reads WASD / joystick movement input
    }

    // Crouching may have to change from scaling to just lowering camera position
    // Called by the Input System when crouch input changes
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)                    // When crouch button is pressed
        {
            // Scales the player down vertically to crouch
            transform.localScale = new Vector3(1, CrouchHeight / NormalHeight, 1);
            playerStats.IsStealthy = true;
        }
        else if (context.canceled)                // When crouch button is released
        {
            // Resets player scale to normal height
            transform.localScale = new Vector3(1, 1, 1);
            playerStats.IsStealthy = false;

        }
    }

    // Called by the Input System when jump input changes
    public void OnJump(InputAction.CallbackContext context)
    {
        // Only jump when the button is first pressed AND the player is grounded
        if (context.started && IsGrounded)
        {
            // Calculates the upward velocity needed to reach the desired jump height
            float jumpVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);

            // Applies jump velocity while preserving horizontal movement
            rb.linearVelocity = new Vector3(rb.linearVelocity.x,jumpVelocity,rb.linearVelocity.z);
        }
    }

    // Called by the Input System when sprint input changes
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started || context.performed) // Sprint button pressed or held
            IsSprinting = true;
        else if (context.canceled)                // Sprint button released
            IsSprinting = false;
    }

    // Called by the Input System when mouse look input changes
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>(); // Reads mouse movement delta
    }

    #endregion

    #region === Look & Movement Logic ===

    // Handles mouse look logic
    private void HandleLook()
    {
        // Horizontal rotation (turns the player body)
        float mouseX = lookInput.x * MouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // Vertical rotation (tilts the camera)
        float mouseY = lookInput.y * MouseSensitivity;
        xRotation -= mouseY;                      // Invert for natural FPS feel
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents over-rotation
        CameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // Handles player movement physics
    private void HandleMove()
    {
        // Converts input into world-space movement direction
        Vector3 moveDir = transform.forward * MoveInput.y
                        + transform.right * MoveInput.x;

        // Calculates final movement speed
        float finalSpeed = MoveSpeed * currentSpeedMultiplier;

        // Applies movement while preserving vertical velocity (gravity/jumping)
        rb.linearVelocity = new Vector3(
            moveDir.x * finalSpeed,
            rb.linearVelocity.y,
            moveDir.z * finalSpeed
        );
    }

    #endregion

    #region === Stamina Logic ===

    // Stamina may move to a PlayerStats
    // Handles stamina drain, regeneration, and exhaustion logic
    private void HandleStamina()
    {
        // If stamina reaches zero, mark player as exhausted
        if (CurrentStamina <= 0)
        {
            IsExhausted = true;
        }

        // Recover from exhaustion once stamina reaches the threshold
        if (IsExhausted && CurrentStamina >= RegenThreshold)
        {
            IsExhausted = false;
        }

        // Drain stamina if sprinting, moving, and not exhausted
        if (IsSprinting && MoveInput.magnitude > 0 && CurrentStamina > 0 && !IsExhausted)
        {
            CurrentStamina -= StaminaDrainRate * Time.deltaTime;
            currentSpeedMultiplier = SprintMultiplier;
        }
        else
        {
            // Regenerate stamina when not sprinting
            CurrentStamina += StaminaRegenRate * Time.deltaTime;
            currentSpeedMultiplier = 1f;
        }

        // Ensures stamina stays within valid bounds
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
    }

    #endregion

    #region === Collision Handling ===

    // Called every physics frame while colliding with another object
    private void OnCollisionStay(Collision collision)
    {
        // If touching anything, assume the player is grounded
        IsGrounded = true;
    }

    // Called when collision with an object ends
    private void OnCollisionExit(Collision collision)
    {
        // Player is no longer grounded
        IsGrounded = false;
    }

    #endregion
}
