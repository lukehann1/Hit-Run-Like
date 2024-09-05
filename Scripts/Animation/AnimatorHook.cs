using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorHook : MonoBehaviour
{
    Animator animator;

    bool lookingAtTarget = false;
    Transform lookAtTargetTransform;
    [SerializeField] float yLookAtOffset;

    bool _damageColliderStatus;
    public bool damageColliderStatus {  get { return _damageColliderStatus; } }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(lookAtTargetTransform != null)
        {
            Vector3 lookAtPos = lookAtTargetTransform.position;
            lookAtPos.y += yLookAtOffset;
            animator.SetLookAtPosition(lookAtPos);
            lookingAtTarget = true;

            float headWeight = lookingAtTarget ? 1 : 0;
            animator.SetLookAtWeight(1, 0, headWeight, 0, .75f);
        }   
    }

    public void LookAtTarget(Transform target)
    {
        lookAtTargetTransform = target;
        lookingAtTarget = true;
    }

    public void StopLookingAtTarget()
    {
        lookingAtTarget = false;
    }

    public void OpenDamageCollider()
    {
        _damageColliderStatus = true;
    }

    public void CloseDamageCollider()
    {
        _damageColliderStatus = false;
    }
}
