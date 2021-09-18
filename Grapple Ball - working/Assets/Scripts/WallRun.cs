using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    //Creating Component and raycast variables
    public float viewAngel = 0.4f;//0 means they can wall run at when looking at any angle to the wall even perpendicular 1 means they cant wall run at all 
    private float wallRoatationTemp = 0f;
    private float maxWallRotation = 10f;
    public bool canWallRun = false;
    public bool onWall = false;
    private Rigidbody rb;
    private PlayerController playerC;
    private PlayerMotor motor;
    private float wallside = 0f;
    private float Sprint = 1.5f;

    //Jump variables
    private float jumpForceW = 100000f;
    private int jumpCount = 0;
    public SphereCollider col;
    public LayerMask wallLayers;
    public float wallRunDamp = 0.14f;
    private RaycastHit leftRay;
    private RaycastHit rightRay;
    private RaycastHit backRay;
    private RaycastHit frontRay;
    private Vector3 templeftray = Vector3.zero;
    private Vector3 temprightray = Vector3.zero;
    private Vector3 tempbackray = Vector3.zero;
    private Vector3 tempfrontray = Vector3.zero;
    private float temp;
    private bool initaldamp = false;
    [SerializeField]
    private Camera cam;
    private bool facingwall=false;
    private Vector3 jumpForce = Vector3.zero;
    private Vector3 leftvector = Vector3.zero;
    private Vector3 parrallel = Vector3.zero;
    private Vector3 flatvector = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the component with the name in the <>
        rb = GetComponent<Rigidbody>();
        motor = GetComponent<PlayerMotor>();
        playerC = GetComponent<PlayerController>();
        col = GetComponent<SphereCollider>();
    }





    //Wall running function
    void wallVerticalMovement()
    {
        if (onWall == true )
        {
            if (rb.velocity.y < -5 && (initaldamp==false))
            {
                rb.AddForce(Vector3.up * (wallRunDamp+rb.velocity.y*-0.01f), ForceMode.Impulse);
            }
            else
            {
                initaldamp = true;
            }
            rb.AddForce(Vector3.up*wallRunDamp , ForceMode.Impulse);

        }

    }

    //Calculate Jump based on player input
    public void wallJump()
    {
        //Sets vector equal to 0
        

        //if space is pressed
        if (Input.GetButtonDown("Jump") && onWall)

        {
            //Ensures that the constraints are set correctly

 


            directionjump(1f);
            //Add one to the jump counter
            jumpCount++;

            //Makes playerC jumpcount equal 2
            playerC.jumpCount = 2;
        }
    }
    private void wallCheck()
    {
        if (Physics.CheckSphere(col.bounds.center, col.radius *1.1f, wallLayers) && playerC.onGround==false)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }
    }
    private void wallHorizontalMovement(){


        Physics.Raycast(transform.position,Vector3.left,out leftRay, 1000f);
        Physics.Raycast(transform.position, Vector3.right, out rightRay, 1000f);
        Physics.Raycast(transform.position, -1f*Vector3.forward, out backRay, 1000f);
        Physics.Raycast(transform.position,  Vector3.forward, out frontRay, 1000f);
        if (leftRay.distance < 0.2)
        {
            leftRay.distance = 1000000000;
        }
        if (rightRay.distance < 0.2)
        {
            rightRay.distance = 1000000000;
        }
        if (frontRay.distance < 0.2)
        {
            frontRay.distance = 1000000000;
        }
        if (backRay.distance < 0.2)
        {
            backRay.distance = 1000000000;
        }
        temp = Mathf.Min(new float[] { leftRay.distance, rightRay.distance, backRay.distance, frontRay.distance });
        if (temp > 0.2 && temp < 1)
        {
            facingwall = true;
            if (wallRoatationTemp < maxWallRotation)
            {
                wallRoatationTemp += 1f;
            }

            if (temp == leftRay.distance)
            {

                templeftray = Vector3.Cross(leftRay.normal.normalized, Vector3.up).normalized;
                if (Vector3.Dot(templeftray, cam.transform.forward.normalized) > 0.4f)
                {
                    parrallel = Vector3.Cross(leftRay.normal.normalized, Vector3.up).normalized;
                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed*Sprint);
                    wallside = -1;
                }
                else if (Vector3.Dot(templeftray, cam.transform.forward.normalized) < -0.4f)
                {
                    parrallel = Vector3.Cross(leftRay.normal.normalized, Vector3.up).normalized*1f;
                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed * -1 * Sprint);
                    wallside = 1;
                }
                else
                {
 
                    facingwall = false;
                }
                
            }
            if (temp == rightRay.distance)
            {
                
                temprightray = Vector3.Cross(rightRay.normal.normalized, Vector3.up).normalized;
                if (Vector3.Dot(temprightray, cam.transform.forward.normalized) > viewAngel)
                {
                    parrallel = Vector3.Cross(rightRay.normal.normalized, Vector3.up).normalized;
                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed * Sprint);
                    wallside = -1;
                }
                else if (Vector3.Dot(temprightray, cam.transform.forward.normalized) < -1*viewAngel)
                {
                    parrallel = Vector3.Cross(rightRay.normal.normalized, Vector3.up).normalized*1f;
                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed*-1 * Sprint);
                    wallside = 1;
                }
                else
                {
                    facingwall = false;
                }
            }
            if (temp == backRay.distance)
            {

                tempbackray = Vector3.Cross(backRay.normal.normalized, Vector3.up).normalized;
                if (Vector3.Dot(tempbackray, cam.transform.forward.normalized) > viewAngel)
                {
                    parrallel = Vector3.Cross(backRay.normal.normalized, Vector3.up).normalized;
                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed * Sprint);
                    wallside = -1;
                }
                else if (Vector3.Dot(tempbackray, cam.transform.forward.normalized) < -1 * viewAngel)
                {
                    parrallel = Vector3.Cross(backRay.normal.normalized, Vector3.up).normalized*1f;

                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed*-1 * Sprint);
                    wallside = 1;
                }
                else
                {
                    facingwall = false;
                }
            }
            if (temp == frontRay.distance)
            {
                tempfrontray = Vector3.Cross(frontRay.normal.normalized, Vector3.up).normalized;
                if (Vector3.Dot(tempfrontray, cam.transform.forward.normalized) > viewAngel)
                {
                    parrallel = Vector3.Cross(frontRay.normal.normalized, Vector3.up).normalized;
                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed * Sprint);
                    wallside = -1;
                }
                else if (Vector3.Dot(tempfrontray, cam.transform.forward.normalized) < -1 * viewAngel)
                {
                    parrallel = Vector3.Cross(frontRay.normal.normalized, Vector3.up).normalized*1f;
                    motor.Move(parrallel * Input.GetAxisRaw("Vertical") * playerC.speed*-1 * Sprint);
                    wallside = 1;
                }
                else
                {
                    facingwall = false;
                }
            }
        }


    }
    private void intialdampening()
    {
        if (rb.velocity.y > 0){
            initaldamp = false;
        }
    }
    private void wallcamera()
    {

        if (facingwall)
        {

            cam.transform.eulerAngles += new Vector3(0, 0, wallside * wallRoatationTemp);
        }
        else
        {

            if (wallRoatationTemp > 0)
            {

                wallRoatationTemp += -2f;
                cam.transform.eulerAngles += new Vector3(0, 0, wallside * wallRoatationTemp);//wallside * wallRoatationTemp
            }
            else
            {
                wallRoatationTemp = 0;
                wallside = 0;
            }
        }
    }
    private void directionjump(float direction)
    {
        flatvector = new Vector3(parrallel.x,0f, parrallel.z).normalized;
        leftvector = Vector3.Cross(Vector3.up, flatvector);

        
        jumpForce = Vector3.up * jumpForceW+leftvector.normalized*direction* jumpForceW*2;
        //Apply Jump
        motor.ApplyJump(jumpForce);
    }

    private void FixedUpdate()
    {
       
        wallCheck();
        intialdampening();
        if (onWall){

            
            wallVerticalMovement();
            wallHorizontalMovement();
            
        }
        else
        {
            facingwall = false;
        }
        wallcamera();
        wallJump();
        


    }
}
