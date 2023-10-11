using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Gun gunShootHandler;

    Player playerInput;

	Vector2 _movementInput;
	Vector2 MovementOut;
	Rigidbody rb;
	[SerializeField]
	GameObject cameraPoint;

	float MoveSpeed = 4500f;
	float Multiplier = 1;
	float MultiplierForward = 1;
	float MaxSpeed = 3;
	float _counterMovement = 0.175f;
	Vector2 mag;

	bool _isGrounded;
	private float characterLength = 2f;
	private float padding = 0.01f;
	private float maxDistanceToGround = 0.01f;
	private float _threshold = 0.01f;
	public LayerMask _whatIsGround;
	public Camera playerCam;
	private float _xRotation;
	[SerializeField]
	private float _sensitivity = 50f;
	private float _sensMultiplier = 1f;

	bool IsJumpPressed;
	float _jumpPressTimestamp;
	private float _jumpCooldown = 0.25f;
	[SerializeField]
	private float _jumpForce = 550f;
	public bool isHolding;
	public RaftUpgradePickUp raftUpgrade;
	PickUpItems PickUpItems;

	// Start is called before the first frame update
	void Awake()
    {
		PickUpItems = GetComponent<PickUpItems>();
		 rb = GetComponent<Rigidbody>();
		_jumpPressTimestamp = Time.time;

		playerInput = new Player();
		playerInput.PlayerActions.Movement.performed += ctx =>
		{
			_movementInput = ctx.ReadValue<Vector2>();
		};

		playerInput.PlayerActions.MouseMovement.performed += ctx =>
		{
				Vector2 vector = ctx.ReadValue<Vector2>() * _sensitivity * Time.fixedDeltaTime * _sensMultiplier;
				float y = playerCam.transform.localRotation.eulerAngles.y + vector.x;
				_xRotation -= vector.y;
				_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
				playerCam.transform.localRotation = Quaternion.Euler(_xRotation, y, 0f);
				transform.localRotation = Quaternion.Euler(0f, y, 0f);
		};

		playerInput.PlayerActions.Jump.performed += ctx =>
		{
			IsJumpPressed = ctx.ReadValueAsButton();
			_jumpPressTimestamp = Time.time;
		};
		playerInput.PlayerActions.Jump.canceled += ctx =>
		{
			//IsJumpPressed = false;
		};

		playerInput.PlayerActions.Run.performed += ctx =>
		{
			MaxSpeed = 7;
		};
		playerInput.PlayerActions.Run.canceled += ctx =>
		{
			MaxSpeed = 3;
		};

		playerInput.PlayerActions.Shoot.performed += ctx =>
		{
			gunShootHandler.DoTriggerPulled();
		};

		playerInput.PlayerActions.Shoot.canceled += ctx =>
		{
			gunShootHandler.DoTriggerReleased();
		};

		playerInput.PlayerActions.Reload.performed += ctx =>
		{
			gunShootHandler.DoReloadGun();
		};
		playerInput.PlayerActions.Swap.performed += ctx =>
		{
			gunShootHandler.GunSwapped();
		};

		playerInput.PlayerActions.Interact.performed += ctx =>
		{
			PickUpItems.OnInteract();
		};

		playerInput.PlayerActions.Interact.canceled += ctx =>
		{
			PickUpItems.OnStopInteract();
		};

        playerInput.Enable();
		SetUpGun();

	}

	void SetUpGun()
    {
		gunShootHandler.cam = playerCam;
		gunShootHandler.rb = rb;
		gunShootHandler.owner = this;
	}

	// Update is called once per frame
	void Update()
    {
		Debug.DrawRay(transform.position, transform.forward*3, Color.red);
    }
	private void FixedUpdate()
	{
		if (IsJumpPressed && _isGrounded && _jumpPressTimestamp + _jumpCooldown <= Time.time)
		{
			IsJumpPressed = false;
			HandleJump();
			_isGrounded = false;
		}
		if (_isGrounded)
		{
			DoWalking();
			CounterMovement(_movementInput.x, _movementInput.y, mag);
		}
		CheckGrounded();
		mag = FindVelRelativeToLook();
		playerCam.transform.position = cameraPoint.transform.position;
	}


	private void DoWalking()
	{
		float x = _movementInput.x;
		float y = _movementInput.y;
		if (_movementInput.x > 0f && mag.x > MaxSpeed)
		{
			x = 0f;
		}
		if (_movementInput.x < 0f && mag.x < -MaxSpeed)
		{
			x = 0f;
		}
		if (_movementInput.y > 0f && mag.y > MaxSpeed)
		{
			y = 0f;
		}
		if (_movementInput.y < 0f && mag.y < -MaxSpeed)
		{
			y = 0f;
		}
		MovementOut = new Vector2(x, y);
		rb.AddForce(transform.right * MovementOut.x * MoveSpeed * Time.fixedDeltaTime * Multiplier);
		rb.AddForce(transform.forward * MovementOut.y * MoveSpeed * Time.fixedDeltaTime * Multiplier * MultiplierForward);
	}


	public Vector2 FindVelRelativeToLook()
	{
		float y = transform.eulerAngles.y;
		float target = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 57.29578f;
		float num = Mathf.DeltaAngle(y, target);
		float num2 = 90f - num;
		float magnitude = rb.velocity.magnitude;
		float x = magnitude * Mathf.Cos(num2 * (Mathf.PI / 180f));
		float y2 = magnitude * Mathf.Cos(num * (Mathf.PI / 180f));
		return new Vector2(x, y2);
	}

	private void CheckGrounded()
	{
		float num = characterLength / 4f;
		Vector3 position = base.transform.position;
		position.y -= num;
		if (Physics.SphereCast(position, num - padding, -base.transform.up, out var _, padding + maxDistanceToGround, _whatIsGround))
		{
			_isGrounded = true;
		}
		else
		{
			_isGrounded = false;
		}
	}


	private void HandleJump()
	{
		rb.AddForce(Vector2.up * _jumpForce * 1.5f);
		Vector3 velocity = rb.velocity;
		if (rb.velocity.y < 0.5f)
		{
			rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
		}
		else if (rb.velocity.y > 0f)
		{
			rb.velocity = new Vector3(velocity.x, velocity.y / 2f, velocity.z);
		}
		//JumpsLeft--;
	}
	private void CounterMovement(float x, float y, Vector2 mag)
	{
		if ((Mathf.Abs(mag.x) > _threshold && Mathf.Abs(x) < 0.05f) || (mag.x < 0f - _threshold && x > 0f) || (mag.x > _threshold && x < 0f))
		{
			rb.AddForce(MoveSpeed * transform.right * Time.fixedDeltaTime * (0f - mag.x) * _counterMovement);
		}
		if ((Mathf.Abs(mag.y) > _threshold && Mathf.Abs(y) < 0.05f) || (mag.y < 0f - _threshold && y > 0f) || (mag.y > _threshold && y < 0f))
		{
			rb.AddForce(MoveSpeed * transform.forward * Time.fixedDeltaTime * (0f - mag.y) * _counterMovement);
		}
		if (Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2f) + Mathf.Pow(rb.velocity.z, 2f)) > MaxSpeed)
		{
			float y2 = rb.velocity.y;
			Vector3 vector = rb.velocity.normalized * MaxSpeed;
			rb.velocity = new Vector3(vector.x, y2, vector.z);
		}
	}


}
