// CarController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public bool alive = true;// is car crashed for flipped

    public float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // Rigidbody for weight transfer
    private Rigidbody rb;

    // Static instance for easy access
    public static CarController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!alive) return;

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();


        // Check if the car is flipped
        if (Vector3.Dot(transform.up, Vector3.up) < 0.1f)
        {
            Die();
        }
    }

    private void GetInput()
    {

        verticalInput = 1.5f; // Always move forward at a constant acceleration

        // Braking Input
        isBreaking = Input.GetKey(KeyCode.Space);


        if (horizontalInput >= 0.2f || verticalInput <= -0.2f)
        {
            // Debugging information
            Debug.Log("Horizontal Input: " + horizontalInput);
            Debug.Log("Vertical Input: " + verticalInput);
        }

        if (transform.position.y < -5)
        {
            Die();
        }
    }
    //joystick
    public void SetInput(float horizontal, float vertical)
    {
        horizontalInput = horizontal;
        this.verticalInput = vertical;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();


        float weightTransfer = verticalInput > 0 ? -verticalInput * 0.2f : 0f;
        rb.AddForce(Vector3.down * weightTransfer);
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void Die()
    {
        alive = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

}

