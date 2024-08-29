using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2.5f;
    public float sprintSpeed = 5.0f;
    public float rotationSpeed = 1.0f;
    public float speedChangeRate = 10.0f;

    public float max_stamina = 8.0f;
    public float stamina = 0;

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

    public bool isAlive = true;

    public GameObject ramesisTarget;

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
    private GameObject visuals;
    private Animator animator;
    private GameObject mainCamera;
    private UI ui;

    private const float threshold = 0.01f;
    private const float speedOffset = 0.1f;

    private GameObject currentCloset;

    public Transform[] winPoints;

    private bool IsCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    private void Start()
    {
        // Getting componets
        controller = GetComponent<CharacterController>();
        input = GetComponent<Inputs>();
        playerInput = GetComponent<PlayerInput>();
        visuals = transform.Find("Visuals").gameObject;
        animator = visuals.GetComponent<Animator>();
        mainCamera = GameObject.Find("MainCamera");

        Transform canvas = GameObject.Find("Canvas").transform;
        ui = canvas.GetComponent<UI>();
    }

    private void Update()
    {
        // Functions
        GroundedCheck();
        Gravity();
        Animations();

        if (isAlive)
        {
            Move();
            Interact();
            Keybinds();
            Closets();
        }
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
    private void Animations()
    {
        // Plays animation if player is still alive
        if (isAlive)
        {
            Vector2 move = input.move;
            bool sprint = input.sprint;

            // animations
            if (sprint) animator.SetFloat("Speed", 1);
            else if (move != Vector2.zero) animator.SetFloat("Speed", 0.5f);
            else animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    private void Move()
    {
        Vector2 move = input.move;
        bool sprint = input.sprint;

        // If the player is out of stamina, no sprinting
        // If the player is not moving, no sprinting
        if (sprint && (stamina <= 0 || move == Vector2.zero)) sprint = false;

        visuals.transform.localPosition = new Vector3(0, 0.1f, 0);

        // When the player sprints, stamina decreases
        // When the player walks, stamina increases slowly
        // When the player stands still, stamina increases
        if (sprint) stamina -= Time.deltaTime;
        else if (stamina < max_stamina) stamina += ((move != Vector2.zero) ? 0.125f : 0.5f) * Time.deltaTime;

        // Sets target speed based on move speed
        float targetSpeed = sprint ? sprintSpeed : walkSpeed;

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

        // if the player is close to a win point then the player wins
        foreach (Transform winPoint in winPoints)
        {
            if (Vector3.Distance(transform.position, winPoint.position) < reach)
                ui.YouWin();
        }
    }

    private void Interact()
    {
        // Checks whether the player has clicked
        bool use1 = input.use1;
        bool use2 = input.use2;

        bool interact = input.interact;
        bool valid = false;

        // Casts ray to see where the player is looking
        Ray ray = new(mainCamera.transform.position, mainCamera.transform.forward);
        bool cast = Physics.Raycast(ray, out RaycastHit hit, reach, ~(1 << 2));
        if (cast)
        {
            // Gives info on the object the player is looking at
            GameObject objectHit = hit.collider.gameObject;

            if (use1) // Left click
            {
                if (objectHit.CompareTag("Object"))
                {
                    if (objectHit.GetComponent<Target>() != null)
                    {
                        if (heldObject != null)
                        {
                            heldObject.GetComponent<Object>().Use(objectHit);
                        }
                        else
                        {
                            objectHit.GetComponent<Target>().ObjectInteraction(false); // Displays required tool
                        }
                    }
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
                        // Drops object if already holding
                        if (heldObject != null)
                            heldObject.GetComponent<Object>().Dropped(); 
                        heldObject = objectHit;
                        heldObject.GetComponent<Object>().PickedUp();
                    }
                }

                // Flip switch
                if (objectHit.CompareTag("Switch")) 
                {
                    objectHit.transform.parent.GetComponent<PowerBox>().flipSwitch(objectHit);
                }
                // Interact with keypad
                if (objectHit.GetComponent<KeypadButton>() != null) 
                {
                    objectHit.GetComponent<KeypadButton>().PressButton();
                }
            }

            if (!objectHit.CompareTag("Untagged"))
                valid = true;

            // closets
            if (objectHit.CompareTag("Closet"))
            {
                ui.flashMessage("[E] to hide", 1);
                if (interact)
                {
                    // drops any objects on enter
                    if (heldObject != null) 
                    {
                        heldObject.GetComponent<Object>().Dropped();
                        heldObject = null;
                    }
                    currentCloset = objectHit;
                    currentCloset.GetComponent<Closet>().Interact();
                }
            }

            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            // When not looking at an object
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * reach, Color.green);
        }

        // call ui to set corsshair
        ui.crosshair(valid);
    }

    private void Keybinds()
    {
        bool drop = input.drop;

        if (drop)
        {
            // Drops an object
            if (heldObject != null)
            {
                heldObject.GetComponent<Object>().Dropped();
                heldObject = null;
            }
        }

        // speeds up game 
        // bool speedTime = input.timeSpeed;
        // Time.timeScale = speedTime ? 10.0f : 1.0f;
    }

    // Allows player to exits closets
    private void Closets()
    {
        bool close = input.close;

        if (currentCloset != null)
        {
            transform.position = currentCloset.GetComponent<Closet>().insidePosition.position;
            ui.flashMessage("[Q] to exit", 1);
            // Teleports player out
            if (close) 
            {
                currentCloset.GetComponent<Closet>().Interact();
                transform.position = currentCloset.GetComponent<Closet>().outsidePosition.position;
                controller.enabled = false;
                controller.transform.position = currentCloset.GetComponent<Closet>().outsidePosition.position;
                controller.enabled = true;
                currentCloset = null;
            }
        }
    }

    private void CameraRotation()
    {
        // When the player is dead they face the ramesis
        if (!isAlive)
        {
            cinemachineCameraTarget.transform.LookAt(ramesisTarget.transform.position);
            return;
        }

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

            visuals.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void ObjectPositioning()
    {
        // Positions the held object to infront of the player and to the right
        if (heldObject != null)
        {
            Vector3 objectPosition = mainCamera.transform.position + ((mainCamera.transform.forward + mainCamera.transform.right) * 0.225f) - (mainCamera.transform.up * 0.125f);
            heldObject.transform.position = Vector3.MoveTowards(heldObject.transform.position, objectPosition, Time.deltaTime * 10);
            heldObject.transform.rotation = mainCamera.transform.rotation;
            heldObject.transform.localScale = Vector3.MoveTowards(heldObject.transform.localScale, Vector3.one * 0.5f, Time.deltaTime * 10);
        }
    }

    // Limits camera movment
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
