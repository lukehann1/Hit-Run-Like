using Cinemachine;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    Transform mTransform;

    public Transform followTarget;
    [SerializeField] Transform pivot;
    [SerializeField] float followSpeed;
    [SerializeField] float rotateSpeed;
    float lookAngle = 0;
    float pivotAngle = 0;
    [SerializeField] float minPivotAngle = -35;
    [SerializeField] float maxPivotAngle = 35;
    CinemachineVirtualCamera vCam;

    public void CameraInit()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();  
        mTransform = transform; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CameraUpdate(InputContainer inputContainer, float delta)
    {
        FollowTarget(delta);
        HandleRotation(inputContainer, delta);

        if (inputContainer.ra_input.pressed)
            CameraReset();
    }

    void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.Lerp(mTransform.position, followTarget.position, followSpeed * delta);
        mTransform.position = targetPosition;
    }

    void HandleRotation(InputContainer inputContainer, float delta)
    {
        if (lookAngle >= 360 || lookAngle <= -360)
            lookAngle = 0;
        lookAngle += (inputContainer.rightStick.x * rotateSpeed) * delta;
        pivotAngle -= (inputContainer.rightStick.y * rotateSpeed) * delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        Vector3 euler = Vector3.zero;
        euler.y = lookAngle;
        euler.x = pivotAngle;

        Quaternion targetRotation = Quaternion.Euler(euler);
        mTransform.rotation = targetRotation;
    }

    void CameraReset()
    {
        Debug.Log("Camera Reset");
        lookAngle = followTarget.transform.localRotation.eulerAngles.y;
        pivotAngle = 0;
    }

    public void SetCameraPosition(Transform target, bool lookAtFront)
    {
        followTarget = target;
        vCam.LookAt = followTarget;
        mTransform.position = followTarget.position;

        if (!lookAtFront)
            lookAngle = followTarget.transform.rotation.eulerAngles.y;
        else
            lookAngle = -followTarget.transform.rotation.eulerAngles.y;
    }
}
