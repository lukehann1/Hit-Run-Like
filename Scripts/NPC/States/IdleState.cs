using UnityEngine;

public class IdleState : NonPlayableCharacterState
{
    [SerializeField] float maximumDistanceFromTargetBeforePatrol;
    [SerializeField] bool canRotate;
    [SerializeField] float rotateSpeed;
    Transform player;

    public override void StateInit()
    {
        stateKey = NonPlayableCharacterController.npcStates.idle;
    }

    public override void EnterState(NonPlayableCharacterCore core)
    {
        player = ControllerManager.instance.playerController.transform;
        core.forwardAmount = 0;
        if(player != null ) 
            core.animatorHook.LookAtTarget(player);
    }

    public override void StateUpdate(NonPlayableCharacterCore core)
    {
        player = ControllerManager.instance.playerController.transform;

        if (canRotate)
            Rotate(core.mTransform, core.animator);
    }

    public override NonPlayableCharacterController.npcStates ChangeState(NonPlayableCharacterCore core)
    {
        if (player == null || core.GetDistanceFromTarget(player) >= maximumDistanceFromTargetBeforePatrol)
            return NonPlayableCharacterController.npcStates.patrol;
        return NonPlayableCharacterController.npcStates.none;
    }

    public override void ExitState(NonPlayableCharacterCore core)
    {
        core.animator.SetBool("isTurning", false);
        core.animatorHook.StopLookingAtTarget();
    }

   void Rotate(Transform mTransform, Animator animator)
    {
        Vector3 targetDir = player.position - mTransform.position;
        float angle = Vector3.Angle(mTransform.forward, targetDir);
        Vector3 crossProduct = Vector3.Cross(mTransform.forward, targetDir);
        bool isRight = crossProduct.y < 0;
        

        if(angle > 45f)
        {
            animator.SetBool("isTurning", true);
            float turningAmount = isRight? -1 : 1;
            animator.SetFloat("turningAmount", turningAmount, .25f, Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            targetRotation.x = 0;
            targetRotation.z = 0;
            mTransform.rotation = Quaternion.Slerp(mTransform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isTurning", false);
            animator.SetFloat("turningAmount", 0, .25f, Time.deltaTime);
        }
    }
}
