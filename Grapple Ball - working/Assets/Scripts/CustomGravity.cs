using UnityEngine;

//Requires object to havea Rigidbody
[RequireComponent(typeof(Rigidbody))]

public class CustomGravity : MonoBehaviour
{
    // Gravity Scale editable on the inspector
    // providing a gravity scale per object
    public float gravityScale = 1.0f;

    // Global Gravity doesn't appear in the inspector. Modify it here in the code
    // (or via scripting) to define a different default gravity for all objects.
    public static float globalGravity = -9.81f;

    //Creates rb variable in the script
    Rigidbody rb;

    //When gravity is enabled it makes the rigidbodys gravity false and uses the defined one
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    //Gravity calculation and application
    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}
