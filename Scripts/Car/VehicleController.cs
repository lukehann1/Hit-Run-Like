using UnityEngine;

public class VehicleController : MonoBehaviour
{
    Rigidbody rb;
    Transform mTransform;
    [SerializeField] float motorTorque = 2000;
    [SerializeField] float brakeTorque = 2000;
    [SerializeField] float maxForwardSpeed = 20;
    [SerializeField] float maxReverseSpeed = 10;
    [SerializeField] float steeringRange = 30;
    [SerializeField] float steeringRangeAtMaxSpeed = 10;
    [SerializeField] Wheel[] weels;
    [SerializeField] float centerOfGravityOffset = -1f;
    [SerializeField] float stoppingSpeed = 3f;
    public float cameraHeight;
    public float cameraDistance;
    public SpawnPoint exitPoint;

    public void VehicleInit()
    {
        rb = GetComponent<Rigidbody>();
        mTransform = transform;
        rb.centerOfMass += Vector3.up * centerOfGravityOffset;
        foreach (Wheel wheel in weels) 
            wheel.WheelInit();
    }

    public void VehicleUpdate(InputContainer inputContainer, float delta)
    {
        HandleDrive(inputContainer, delta);

        if(inputContainer.y_input.pressed)
            ExitVehcile();
    }

    void HandleDrive(InputContainer inputContainer, float delta)
    {
        int moveDirection;

        if (inputContainer.b_input.held)
            moveDirection = -1;
        else if (inputContainer.a_input.held)
            moveDirection = 1;
        else
            moveDirection = 0;

        float forwardSpeed = Vector3.Dot(mTransform.forward, rb.velocity);
        float maxSpeed = moveDirection > 0 ? maxForwardSpeed : maxReverseSpeed;
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        foreach (Wheel wheel in weels)
        {
            if (wheel.isSteerable)
                wheel.wheelCollider.steerAngle = inputContainer.leftStick.x * currentSteerRange;

            switch (moveDirection)
            {
                case 0:
                    wheel.wheelCollider.motorTorque = 0;
                    wheel.wheelCollider.brakeTorque = Mathf.Lerp(wheel.wheelCollider.brakeTorque, brakeTorque, delta * stoppingSpeed);
                    break;
                case 1:
                    if (forwardSpeed < 0)
                    {
                        if (wheel.isMotorized)
                            wheel.wheelCollider.motorTorque = 0;
                        wheel.wheelCollider.brakeTorque = brakeTorque;
                    }
                    else
                    {
                        if (wheel.isMotorized)
                            wheel.wheelCollider.motorTorque = currentMotorTorque;
                        wheel.wheelCollider.brakeTorque = 0;
                    }
                    break;
                case -1:
                    if(forwardSpeed > 0)
                    {
                        if(wheel.isMotorized)
                            wheel.wheelCollider.motorTorque = 0;
                        wheel.wheelCollider.brakeTorque = brakeTorque;
                    }
                    else
                    {
                        if (wheel.isMotorized)
                            wheel.wheelCollider.motorTorque = -currentMotorTorque;
                        wheel.wheelCollider.brakeTorque = 0;
                    }
                    break;
                default: 
                    break;
            }

            wheel.WheelUpdate();
        }
    }   

    void ExitVehcile()
    {
        ControllerManager.instance.ExitVehcile(this);
    }
}
