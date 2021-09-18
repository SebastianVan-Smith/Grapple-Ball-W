using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMotor))] //add the player motor which adds the rigidbody
[RequireComponent(typeof(Animator))]

public class PlayerController : NetworkBehaviour 
{
	//Speed variables
	public float speed = 10.0f;

	private float jumpForce = 20000f;
	public int jumpCount = 0;

	//Bool collision variables
	public bool isInAir = false;
	public bool onGround = false;

	//Look Sesitivity
	public float lookSensitivity = 10f;

	//Component variables
	private HealthSystem health;
	private PlayerMotor motor;
	private Rigidbody rb;
	private Animator anim;                          // a reference to the animator on the character
	[SerializeField]
	private Camera cam;
	private WallRun wall;
	public CapsuleCollider col;
	public LayerMask groundLayers;


	void Start ()
	{
		//Gets the component with the name in the <>
		motor = GetComponent<PlayerMotor>();
		health = GetComponent<HealthSystem>();
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		wall = GetComponent<WallRun>();
		

		//Sets the mouse visibility and movement
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void movement ()
	{
		//Calculate movement velocity as a 3D vector
		//Gets horizontal movement, GetAxisRaw gives us complete control or processing, goes between -1 and 1
		float _xMov = Input.GetAxisRaw("Horizontal");

		float _zMov = Input.GetAxisRaw("Vertical");
		
		//local transformation take into account our current rotation, not relative to the world
		//(0, 0, 0) The last value is the only one that changes, posative its forwards negative is backwards, same for top but with first value
		Vector3 _movHorizontal = transform.right * _xMov;  
		Vector3 _movVertical = transform.forward * _zMov;

		anim.SetFloat("Speed", _zMov);

		//If sprint is true run sprint calculation else run normal speed calculation
		if (wall.onWall == false)
		{
			
			//Sprinting speed calculation
			Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed * 1.5f;
			//Apply movement
			motor.Move(_velocity);
			
		}
		//Calculate rotation as a 3D vector: only affects turning around
		//y rotation, left and right only,, tilting up and down we only want to effect the camera
		float _yRot = Input.GetAxisRaw("Mouse X"); 

		//Rotation Calculation X
		Vector3 _rotation = new Vector3(0f, _yRot, 0f ) * lookSensitivity;

		//Apply rotation
		motor.Rotate(_rotation);

		//Calculate camera rotation as a 3D vector: only affects turning around
		float _xRot = Input.GetAxisRaw("Mouse Y"); //y rotation, left and right only,, tilting up and down we only want to effect the camera

		//Rotation calculation Y
		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply rotation
		motor.RotateCamera(_cameraRotationX);
	}
	//Calculate Jump based on player input, this is the jump when on the GROUND
	public void jump()
	{
		//Sets vector equal to 0
		Vector3 _jumpForce = Vector3.zero;

		//if space is pressed and the player is not in the air
		if (Input.GetButton("Jump") && onGround == true)
		{
			//Ensures that the constraints are set correctly
			rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

			//Jump calculation
			_jumpForce = Vector3.up * jumpForce;

			//Apply Jump
			motor.ApplyJump(_jumpForce);

			//Add one to the jump counter
			jumpCount++;
		}
	}
	private void airCheck()
	{
		if (Physics.CheckCapsule(col.bounds.center,new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * 0.9f,groundLayers))
		{
			onGround = true;
		}
		else
		{
			onGround = false;
		}

	}
	private void FixedUpdate()
	{
		if (base.hasAuthority)
		{
			movement();
			airCheck();
			jump();
		}
		
	}
}
