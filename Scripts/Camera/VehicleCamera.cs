using Cinemachine;
using UnityEngine;

public class VehicleCamera : MonoBehaviour
{
    Transform mTransform;
    Transform cameraTransform;

    [SerializeField] Transform followTarget;
    [SerializeField] float rotateSpeed;

    public void CameraInit()
    {
        mTransform = transform;
        cameraTransform = GetComponentInChildren<CinemachineVirtualCamera>().transform;
    }

    public void CameraUpdate(float delta)
    {
        mTransform.position = followTarget.position;
        Quaternion targetRotation = Quaternion.Slerp(mTransform.rotation, followTarget.rotation, rotateSpeed * delta);
        targetRotation.x = 0;
        targetRotation.z = 0;
        mTransform.rotation = targetRotation;
    }

    public void SetCameraPosition(VehicleController vehicle)
    {
        followTarget = vehicle.transform;
        mTransform.position = followTarget.position;
        Vector3 cameraPosition = new Vector3();
        cameraPosition.x = 0;
        cameraPosition.y += vehicle.cameraHeight;
        cameraPosition.z -= vehicle.cameraDistance;
        cameraTransform.localPosition = cameraPosition;
    }
}
