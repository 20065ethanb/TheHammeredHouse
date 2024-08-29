using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool use1;
	public bool use2;
	public bool sprint;
	public bool drop;
	public bool timeSpeed;
	public bool interact;
	public bool close;

	private InputAction _moveAction;
	private InputAction _lookAction;
	private InputAction _use1Action;
	private InputAction _use2Action;
	private InputAction _sprintAction;
	private InputAction _dropAction;
	private InputAction _timeSpeedAction;
	private InputAction _interactAction;
	private InputAction _closeAction;

	// Keeps track of input type
	[Header("Movement Settings")]
	[SerializeField]
	private bool analogMovement;

	// Lock mouse
	[Header("Mouse Cursor Settings")]
	[SerializeField]
	private bool cursorLocked = true;

	private PlayerInput _playerInput;

    private void Start()
    {
		// Find action
		_playerInput = GetComponent<PlayerInput>();

		_moveAction = _playerInput.actions["Move"];
		_lookAction = _playerInput.actions["Look"];
		_use1Action = _playerInput.actions["Use1"];
		_use2Action = _playerInput.actions["Use2"];
		_sprintAction = _playerInput.actions["Sprint"];
		_dropAction = _playerInput.actions["Drop"];
		_timeSpeedAction = _playerInput.actions["TimeSpeed"];
		_interactAction = _playerInput.actions["Interact"];
		_closeAction = _playerInput.actions["Close"];
	}

    private void Update()
    {
		// Getting action
		move = _moveAction.ReadValue<Vector2>();
		look = _lookAction.ReadValue<Vector2>();
		use1 = _use1Action.WasPressedThisFrame();
		use2 = _use2Action.WasPressedThisFrame();
		sprint = _sprintAction.IsPressed();
		drop = _dropAction.WasPressedThisFrame();
		timeSpeed = _timeSpeedAction.IsPressed();
		interact = _interactAction.IsPressed();
		close = _closeAction.IsPressed();
    }

	// Give varible vaule
	public bool IsAnalog()
    {
		return analogMovement;
    }

	// Sets cursor state
	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	// Locks or unlocks cursor state
	public void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
