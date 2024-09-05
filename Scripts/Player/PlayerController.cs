using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Transform mTransform;
    Transform cameraTransform;
    Animator animator;
    AnimatorHook animatorHook;

    [Header("Movement")]
    [SerializeField] float maxWalkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float inAirMoveSpeed;
    float moveSpeed;
    [SerializeField] float walkRotationSpeed;
    [SerializeField] float runRotationSpeed;
    [SerializeField] float inAirRotationSpeed;
    float rotationSpeed;

    Vector3 moveDirection;
    Vector3 verticalVelocity = Vector3.zero;
    float moveAmount;

    [Header("Jumping")]
    [SerializeField] float maxJumpHeight = 1;
    [SerializeField] float maxJumpTime = .5f;
    float gravity;
    [SerializeField] float groundedGravity = -.5f;
    float intialJumpVelocity;
    
    [Header("Door Check")]
    [SerializeField] float doorCheckRadius;
    [SerializeField] int doorLayer;
    [SerializeField] Transform checkPosition;

    [Header("Vehicle Check")]
    [SerializeField] float carCheckRadius;
    [SerializeField] int carLayer;

    [Header("Actions")]
    [SerializeField] DamageCollider damageCollider;

    [Header("Flags")]
    [SerializeField] bool isInteracting;
    [SerializeField] bool isRunning;
    [SerializeField] bool isJumping;
    [SerializeField] bool isDoubleJumping;   

    public void PlayerInit()
    {
        characterController = GetComponent<CharacterController>();
        characterController.skinWidth = characterController.radius * 0.1f;
        mTransform = transform;
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
        animatorHook = GetComponentInChildren<AnimatorHook>();
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        intialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    public void PlayerUpdate(InputContainer inputContainer, float delta)
    {
        isInteracting = animator.GetBool("isInteracting");
        isRunning = inputContainer.b_input.held;

        damageCollider.ColliderStatus(animatorHook.damageColliderStatus);

        if (inputContainer.x_input.pressed && !isInteracting && !isJumping)
            HandleAttacking();

        if (!isInteracting)
        {
            HandleMovement(inputContainer, delta);
            HandleRotation(delta);
            CheckForDoor(inputContainer);
            CheckForVehicle(inputContainer);
        }

        HandleGravity(delta);
        HandleJump(inputContainer);
        HandleAnimations(delta);
    }

    void HandleMovement(InputContainer inputContainer, float delta)
    {
        moveAmount = inputContainer.leftStick.magnitude;
        if (moveAmount >= .5f)
        {
            moveDirection = cameraTransform.forward * inputContainer.leftStick.y;
            moveDirection += cameraTransform.right * inputContainer.leftStick.x;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (isJumping)
                moveSpeed = inAirMoveSpeed;
            else
                moveSpeed = isRunning ? runSpeed : maxWalkSpeed;
            moveAmount = isRunning ? 1 : moveAmount;

            moveDirection *= moveSpeed * moveAmount;                      
        }
        else
        {
            moveAmount = 0;
            moveDirection = Vector3.zero;
        }

        characterController.Move(moveDirection * delta);
    }

    void HandleRotation(float delta)
    {
        Vector3 targetDir = moveDirection;
        if (targetDir == Vector3.zero)
            targetDir = mTransform.forward;

        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        if (isJumping)
            rotationSpeed = inAirRotationSpeed;
        else
            rotationSpeed = isRunning ? runRotationSpeed : walkRotationSpeed;
        Quaternion targetRotation = Quaternion.Slerp(mTransform.rotation, lookRotation, delta * rotationSpeed);
        mTransform.rotation = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
    }

    void HandleGravity(float delta)
    {
        if (characterController.isGrounded && !isJumping)
            verticalVelocity.y = groundedGravity;
        else
            verticalVelocity.y += gravity * delta;

        characterController.Move(verticalVelocity * delta);
    }

    void HandleAnimations(float delta)
    {
        float forwardAmount = moveAmount;

        if (isRunning && moveAmount > 0)
            forwardAmount = 2;

        animator.SetFloat("forward", forwardAmount, .25f, delta);
        animator.SetBool("isGrounded", characterController.isGrounded);
    }

    void HandleAttacking()
    {
        PlayTargetAnimation("Kicking", true);
    }

    void HandleJump(InputContainer inputContainer)
    {
        if(!isInteracting && inputContainer.a_input.pressed && (characterController.isGrounded || !isDoubleJumping))
        {
            if(isJumping)
                isDoubleJumping = true;
            isJumping = true;
            animator.CrossFadeInFixedTime("Jumping Up", .05f);
            float jumpVelocity = isDoubleJumping? intialJumpVelocity * .75f : intialJumpVelocity;
            verticalVelocity.y = jumpVelocity;
        }
        else if (isJumping && characterController.isGrounded)
        {
            isJumping = false;
            isDoubleJumping = false;
        }
    }

    void CheckForDoor(InputContainer inputContainer)
    {
        Collider[] cols = Physics.OverlapSphere(checkPosition.position, doorCheckRadius, 1<<doorLayer);
        if(cols.Length > 0 && inputContainer.a_input.pressed)
        {
            foreach(Collider col in cols)
            {
                Door door = col.GetComponent<Door>();
                if(door != null)
                {
                    SpawnManager.instance.nextSpawnPointID = door.exitSpawnPointID;
                    SceneHandler.instance.LoadScene(door.sceneName);
                    return;
                }
            }
        }
    }

    void CheckForVehicle(InputContainer inputContainer)
    {
        Collider[] cols = Physics.OverlapSphere(checkPosition.position, carCheckRadius, 1 << carLayer);

        if (cols.Length > 0 && inputContainer.y_input.pressed)
        {
            foreach (Collider col in cols)
            {
                VehicleController vehicle = col.GetComponent<VehicleController>();
                if (vehicle != null)
                {
                    ControllerManager.instance.EnterVehicle(vehicle);
                    return;
                }
            }
        }
    }

    void PlayTargetAnimation(string animName, bool isInteracting, float crossFadeTime = .25f)
    {
        animator.CrossFadeInFixedTime(animName, crossFadeTime);
        animator.SetBool("isInteracting", isInteracting);
    }
}
