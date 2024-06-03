using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float sneakSpeed = 1.0f;
    public float walkSpeed = 2.5f;
    public float sprintSpeed = 5.0f;
    public float rotationSpeed = 1.0f;
    public float speedChangeRate = 10.0f;

    public bool sneaking = false;

    public float stamina = 100.0f;

    public float gravity = -9.8f;

    public float reach = 4f;
    public GameObject heldObject;

    public bool grounded = true;
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.28f;
    public LayerMask groundLayers;

    public GameObject cinemachineCameraTarget;
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;

    public float standingPosition = 1.375f;
    public float sneakingPosition = 0.75f;

    // cinemachine
    private float cinemachineTargetPitch;

    // Player
    private float speed;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    private PlayerInput playerInput;
    private CharacterController controller;
    private Inputs input;
    private GameObject mainCamera;

    private GameObject crosshairOn;
    private GameObject crosshairOff;

    private const float threshold = 0.01f;
    private const float speedOffset = 0.1f;

    private bool IsCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<Inputs>();
        playerInput = GetComponent<PlayerInput>();
        mainCamera = GameObject.Find("MainCamera");

        Transform canvas = GameObject.Find("Canvas").transform;
        crosshairOff = canvas.Find("Crosshair_Off").gameObject;
        crosshairOn = canvas.Find("Crosshair_On").gameObject;
    }

    private void Update()
    {
        GroundedCheck();
        Gravity();
        Sneak();
        Move();
        Interact();
        Keybinds();
    }

    private void LateUpdate()
    {
        CameraRotation();
        ObjectPositioning();
    }

    private void GroundedCheck()
    {
        // Set sphere position, with offset
        Vector3 spherePosition = new(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    private void Gravity()
    {
        if (grounded)
        {
            // Stop velocity from dropping infinitely when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }
        }

        // Apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Sneak()
    {
        bool sneak = input.sneak;
        if (sneak) sneaking = !sneaking;

        float targetY = cinemachineCameraTarget.transform.localPosition.y;
        if (sneaking)
        {
            if (targetY > sneakingPosition)
                targetY -= 0.05f;
            else
                targetY = sneakingPosition;
        }
        else
        {
            if (targetY < standingPosition)
                targetY += 0.05f;
            else
                targetY = standingPosition;
        }
        cinemachineCameraTarget.transform.localPosition = new Vector3(0, targetY, 0);
    }

    private void Move()
    {
        Vector2 move = input.move;
        bool sprint = input.sprint;

        // When sprinting you can't sneak
        if (sneaking && sprint) sneaking = false;

        // Sets target speed based on move speed
        float targetSpeed = sneaking ? sneakSpeed : (sprint ? sprintSpeed : walkSpeed);

        // If there is no input, set the target speed to 0
        if (move == Vector2.zero) targetSpeed = 0.0f;

        // A reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float inputMagnitude = input.IsAnalog() ? move.magnitude : 1f;

        // Accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // Creates curved result rather than a linear one giving a more organic speed change
            // Note T in Lerp is clamped, so we don't need to clamp our speed
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

            // Round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        // Normalise input direction
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        // If there is a move input rotate player when the player is moving
        if (move != Vector2.zero)
        {
            // Move
            inputDirection = transform.right * move.x + transform.forward * move.y;
        }

        // Move the player
        controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void Interact()
    {
        // Checks whether the player has clicked
        bool use1 = input.use1;
        bool use2 = input.use2;

        // Casts ray to see where the player is looking
        Ray ray = new(mainCamera.transform.position, mainCamera.transform.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, reach, ~(1 << 3));
        if (cast)
        {
            // Gives info on the object the player is looking at
            GameObject objectHit = hit.collider.gameObject;

            if (use1) // Left click
            {
                if (heldObject != null)
                    heldObject.GetComponent<Object>().Use(objectHit);
                else
                {
                    if (objectHit.CompareTag("Object"))
                        objectHit.GetComponent<Target>().ObjectInteraction(false);
                }
            }
            else if (use2) // Right click
            {
                // Opening and closing doors
                if (objectHit.CompareTag("Door"))
                    objectHit.GetComponent<Door>().PlayerInteract();

                // Pick up on object
                if (objectHit.CompareTag("Object"))
                {
                    if (objectHit.GetComponent<Object>() != null)
                    {
                        heldObject = objectHit;
                        heldObject.GetComponent<Object>().PickedUp();
                    }
                }
            }

            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            // When not looking at an object
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * reach, Color.green);
        }

        crosshairOff.SetActive(!cast);
        crosshairOn.SetActive(cast);
    }

    private void Keybinds()
    {
        bool drop = input.drop;

        if (drop)
        {
            // Drops an object
            if (heldObject != null)
            {
                heldObject.GetComponent<Rigidbody>().useGravity = true;
                heldObject.GetComponent<Object>().Dropped();
                heldObject = null;
            }
        }
    }

    private void CameraRotation()
    {
        Vector2 look = input.look;
        // If there is an input
        if (look.sqrMagnitude >= threshold)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            cinemachineTargetPitch += look.y * rotationSpeed * deltaTimeMultiplier;
            rotationVelocity = look.x * rotationSpeed * deltaTimeMultiplier;

            // Clamp our pitch rotation
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            // Update Cinemachine camera target pitch
            cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

            // Rotate the player left and right
            transform.Rotate(Vector3.up * rotationVelocity);
        }
    }

    private void ObjectPositioning()
    {
        // Positions the held object to infront of the player in to the right
        if (heldObject != null)
        {
            Vector3 objectPosition = mainCamera.transform.position + ((mainCamera.transform.forward + mainCamera.transform.right) * 0.5f) - (mainCamera.transform.up * 0.25f);
            heldObject.transform.position = objectPosition;
            heldObject.transform.rotation = mainCamera.transform.rotation;
            heldObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
