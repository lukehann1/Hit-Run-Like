using UnityEngine;

public class Wheel : MonoBehaviour
{
    [HideInInspector]
    public WheelCollider wheelCollider;
    [SerializeField] Transform modelTransform;
    public bool isSteerable;
    public bool isMotorized;

    Vector3 position;
    Quaternion rotation;

    public void WheelInit()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }

    public void WheelUpdate()
    {
        wheelCollider.GetWorldPose(out position, out rotation);
        modelTransform.position = position;
        modelTransform.rotation = rotation;
    }
}
