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
	public bool sneak;
	public bool drop;

	private InputAction _moveAction;
	private InputAction _lookAction;
	private InputAction _use1Action;
	private InputAction _use2Action;
	private InputAction _sprintAction;
	private InputAction _sneakAction;
	private InputAction _dropAction;

	[Header("Movement Settings")]
	[SerializeField]
	private bool analogMovement;

	[Header("Mouse Cursor Settings")]
	[SerializeField]
	private bool cursorLocked = true;

	private PlayerInput _playerInput;

    private void Start()
    {
		_playerInput = GetComponent<PlayerInput>();

		_moveAction = _playerInput.actions["Move"];
		_lookAction = _playerInput.actions["Look"];
		_use1Action = _playerInput.actions["Use1"];
		_use2Action = _playerInput.actions["Use2"];
		_sprintAction = _playerInput.actions["Sprint"];
		_sneakAction = _playerInput.actions["Sneak"];
		_dropAction = _playerInput.actions["Drop"];
	}

    private void Update()
    {
		move = _moveAction.ReadValue<Vector2>();
		look = _lookAction.ReadValue<Vector2>();
		use1 = _use1Action.WasPressedThisFrame();
		use2 = _use2Action.WasPressedThisFrame();
		sprint = _sprintAction.IsPressed();
		sneak = _sneakAction.IsPressed();
		drop = _dropAction.WasPressedThisFrame();
    }

	public bool IsAnalog()
    {
		return analogMovement;
    }

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	public void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
