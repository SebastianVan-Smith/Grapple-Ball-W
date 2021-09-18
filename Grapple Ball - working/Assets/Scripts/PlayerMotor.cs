using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
[RequireComponent(typeof(Rigidbody))] //allows access to the rigidbody component

public class PlayerMotor : NetworkBehaviour 
{

	//Vector 3 Variables default to 0, and damping force for speed
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 jumpForce = Vector3.zero;
	private float dampingForce = 0.995f;
	private PlayerController playerC;
	//Camera rotation variables
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	[SerializeField]
	private float cameraRotationLimit = 85f;

	//Component variables
	private Rigidbody rb;
	[SerializeField]
	private SpeedBar speedBar;
	private CustomGravity gravity;
	[SerializeField]
	private Camera cam;

	//Velocity Variables
	private float yVel = 0f;
	private Vector3 velTemp = Vector3.zero;
	private float velSpeedTemp = 0f;
	private Vector3 velocitytemp = Vector3.zero;

	//camera quarternion
	public Quaternion cam_quarternion = Quaternion.identity;
	public Vector3 cam_pos = Vector3.zero;
	public Vector3 cam_forward = Vector3.zero;

	void Start()
	{
		//Gets the component with the name in the <>
		rb = GetComponent<Rigidbody>();

		//Finds a component with the tag "PlayerSpeedBar" and gets the SpeedBar script  
		//speedBar = GetComponent<SpeedBar>();
		playerC = GetComponent<PlayerController>();
		//Part of old system for testing purposes
		//gravity = GetComponent<CustomGravity>();
		
	}


	//Gets movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
	}

	//Gets rotational vector
	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}

	//Gets rotational vector for camera
	public void RotateCamera(float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

	//Get a force vector for the jump
	public void ApplyJump(Vector3 _jumpForce)
	{
		jumpForce = _jumpForce;
	}

	//Run every physics iteration,
	void FixedUpdate ()
	{
		PerformMovement();
		PerformRotation();
		
	}

	//Perform movement based on velocity variable
	void PerformMovement ()
	{
		//Velocity variable with damping force applied
		velocitytemp = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		
		rb.AddForce(velocitytemp * (1 - dampingForce) * -1, ForceMode.VelocityChange);

		

		//if not equal to zero, begin velocity calculation
		if (velocity != Vector3.zero)
		{
			
			rb.AddForce (velocity * UnityEngine.Time.fixedDeltaTime, ForceMode.VelocityChange);
		}

		//if not equal to zero, begin Jump force calculation based on time
		if (jumpForce != Vector3.zero)
		{
			rb.AddForce(jumpForce * UnityEngine.Time.fixedDeltaTime);
			jumpForce = Vector3.zero;
		}

		//Calculations for clamping the player jumps
		yVel = Mathf.Clamp (rb.velocity.y, -100f, 80f);

		//Calculation to get the overall player movement for unification and to implement speed bar
		rb.velocity = new Vector3(rb.velocity.x, yVel, rb.velocity.z);
		velTemp = velocity + rb.velocity;

		//Old movement calculation
		///rb.MovePosition(rb.position + rb.velocity);
		
		//Gets global player movement and creates a precentage to be used for the speed bar
		float speedPct = (velTemp.magnitude / 50);
		speedBar.HandleSpeedChanged(speedPct);
	}

	//Perform Rotation
	void PerformRotation()
	{
		//Lets unity handle rotation calculations
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation)); 

		//if camera is not null run this code block
		if (cam != null)
		{

			//Old Code needed for referencing, camera rotation calculation
			//cam.transform.Rotate(-cameraRotation); //minus needed to inverse rotation

			//New Rotation Calculation, adds clamp to limit camera movement
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			//apply rotation to camera transform
			cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
			cam_quarternion = cam.transform.rotation;
			cam_pos = cam.transform.position;
			cam_forward = cam.transform.forward;
		}
	}

}
