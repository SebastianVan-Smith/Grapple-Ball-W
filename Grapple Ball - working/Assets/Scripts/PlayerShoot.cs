using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class PlayerShoot :NetworkBehaviour {


	//Setting Up Variables
	public GameObject grapple;
	public GameObject stunGun;
	public Material crosshair;

	//Cooldown UI elements NOT YET IMPLEMENTED
	//public UnityEngine.UI.Image grappleCD;
	//public UnityEngine.UI.Image stunGunCD;

	//Creating Component and raycast variables
	private PlayerController playerC;
	private WallRun wallrun;
	[SerializeField]
	private Camera cam;
	[SerializeField]
	private LayerMask mask;
	private RaycastHit hit;
	private RaycastHit crossHit;
	private Rigidbody rb;


	//Grapple Variables

	public bool attached = false;
	public bool canGrapple = false;
	private Vector3 grappleVector = Vector3.zero;
	private float grappleForce = 0f;
	private float grapplepullspeed = 0.02f;
	//Stun Gun Variables
	public GameObject bullet;
	public float stunRange = 100f;
	private float stunTimer = 0f;
	public float stunCoolD = 1.5f;

	//cam variables
	private PlayerMotor playerMotor;
	public GameObject line;
	[SerializeField]
	private LayerMask grappleable;
	//public Mesh linemesh;
	private Vector3[] verts = new Vector3[4];
	void Start()
	{
		// linemesh = line.GetComponent<MeshFilter>().mesh;
		//verts = linemesh.vertices;
		
		playerMotor = GetComponent<PlayerMotor>();
		//Testing Purposes
		//if (cam == null)
		//{
		//	this.enabled = false;
		//}

		//Disabling Grapple and disabling its use of world space
		

		
		//Setting Up References
		rb = GetComponent<Rigidbody>();
		playerC = GetComponent<PlayerController>();
		wallrun = GetComponent<WallRun>();

	}

	void Updateing()
	{

		//Cooldown timer for both abilities, fixed delta time allows floats 
		stunTimer = stunTimer + UnityEngine.Time.fixedDeltaTime;

		//Caps the timer to a defined number
		if (stunTimer >= stunCoolD)
		{
			stunTimer = stunCoolD;
		}

		crosshair.color = new Color(0, 0, 0, 1);

		//Crosshair Color Change using raycast for positions
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out crossHit, 50, grappleable))
		{
				crosshair.color = new Color(0, 1, 0, 1);
		}
		//Leave the crosshair black on default
		else
		{
			crosshair.color = new Color(0, 0, 0, 1);
		}

		//Sets the lines position to where the player is, this needs to be updated as the player moves 
		
		//Debug.DrawLine(playerMotor.cam_pos, playerMotor.cam_pos + (playerMotor.cam_forward * 50));
		//Debug.Log(canGrapple);
		//If left click is true and the player can grapple
		if (Input.GetButtonDown("Fire1") && canGrapple == true)
		{
			//Grapple graphic is true, stun graphic is false
			grapple.SetActive(true);
			stunGun.SetActive(false);

			//Run grapple hook function, set can grapple to false
			
			GrappleHook();
			canGrapple = false;
		}

		//When left click is released, attached equals false and canGrapple is true
		if (Input.GetButtonUp("Fire1"))
		{
			attached = false;
			canGrapple = true;
		}


		//STUN GUN
		//if right click is true
	
	}
	[Command]
	void CmdSpawnBullet(Quaternion rott,Vector3 pos,Vector3 forward)
	{
		//Debug.Log(cam.transform.forward);
		GameObject stunBullet = Instantiate(bullet, pos + forward,rott) as GameObject;
		NetworkServer.Spawn(stunBullet);
	}

	//Stun shot function, creates an instance of the bullet and gives it a position to spawn
	void StunShot()
	{
		CmdSpawnBullet(playerMotor.cam_quarternion, playerMotor.cam_pos, playerMotor.cam_forward);
	}

	//Grapplehook funciton
	void GrappleHook()
	{

		//Raycast with range of 50, set it to come out of the camera,   can grapple mus also be true&& canGrapple == true
		if (Physics.Raycast(playerMotor.cam_pos + playerMotor.cam_forward * 2, playerMotor.cam_forward, out hit, 50, grappleable) )
		{
			//if run attached equals true
			attached = true;

			//sets other point in grapple line to hit point
			
			//Debug.Log(hit.point);
			
		}
	}
	private void drawgrapple(bool ye)
	{
		if (ye)
		{
			line.transform.localScale=new Vector3(0.1f,0.1f, (hit.point - transform.position).magnitude);
			line.transform.forward = (hit.point - transform.position);
			line.transform.position = (transform.position + hit.point) / 2;
			
			//Vector3 pos = Quaternion.Inverse(transform.rotation) * (hit.point - transform.position);
			//verts[0] = pos + new Vector3(0.1f, 0, 0);
			//verts[1] = pos + new Vector3(-0.1f, 0, 0);
			//verts[2] = new Vector3(0.1f, 0, 0);
			//verts[3] = new Vector3(-0.1f, 0, 0);
			//linemesh.vertices = verts;
			//linemesh.RecalculateBounds();
		}
		else
		{
			line.transform.localScale = new Vector3(0.1f, 0.1f,0.1f);
			//line.transform.forward = Vector3.zero;
			line.transform.position = transform.position ;
			//line.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
			//verts[0] = new Vector3(0.1f, 0.1f, 0);
			//verts[1] = new Vector3(-0.1f, 0.1f, 0);
			//verts[2] = new Vector3(0.1f, 0, 0);
			//verts[3] = new Vector3(-0.1f, 0, 0);
			//linemesh.vertices = verts;
			//linemesh.RecalculateBounds();
		}
	}

	//Grapple movement calculation, this is done to fit in with the unified movement system
	void Movement(Vector3 _placetogo)
	{
		grappleVector = (_placetogo - transform.position).normalized*grappleForce;
		rb.AddForce(grappleVector, ForceMode.VelocityChange);

	}

	void FixedUpdate()
	{
		if (base.hasAuthority)
		{
			
			
			
			Updateing();
			//If attached is true, grappleline is visible movement is calculated, wall running is false and grapple timer is 0
			if (attached)
			{
				grappleForce += grapplepullspeed;

				drawgrapple(true);
				Movement(hit.point);
				wallrun.canWallRun = false;
			}
			else
			{
				//else grapple line is invisible
				drawgrapple(false);
			}
			grappleForce *= 0.8f;
		}
		

	}
}
