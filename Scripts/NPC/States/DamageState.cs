using UnityEngine;

public class DamageState : NonPlayableCharacterState
{
    bool isOnGround;
    bool isGettingUp;
    [SerializeField] float forceAmount;
    [SerializeField] float timeBeforeGetUp;
    Timer isOnGroundTimer = new Timer();
    [SerializeField] LayerMask groundLayers;
    [SerializeField] Transform midCheck;

    public override void StateInit()
    {
        stateKey = NonPlayableCharacterController.npcStates.damage;
    }

    public override void EnterState(NonPlayableCharacterCore core)
    {
        core.rb.isKinematic = false;
        core.rb.AddForce(core.attackDir * forceAmount);
        isOnGround = true;
        isGettingUp = false;
        isOnGroundTimer.StartTimer(timeBeforeGetUp);
        core.animator.SetBool("isOnGround", true);
        core.animator.CrossFadeInFixedTime("Falling", .25f);
    }

    public override void StateUpdate(NonPlayableCharacterCore core)
    {
        isOnGround = core.animator.GetBool("isOnGround");
        float delta = Time.deltaTime;
        isOnGroundTimer.Tick(delta);

        if (!isOnGroundTimer.IsGreaterThatZero() && !isGettingUp)
            GetUp(core);
    }

    void GetUp(NonPlayableCharacterCore core)
    {
        isGettingUp = true;
        Quaternion targetRot = Quaternion.LookRotation(core.mTransform.forward);
        targetRot.x = 0;
        targetRot.z = 0;
        core.mTransform.rotation = targetRot;
        Vector3 targetPos = GetMidPointPosition();
        targetPos = targetPos != Vector3.zero ? targetPos : core.mTransform.forward;
        core.mTransform.position = targetPos;
        core.animator.Play("Getting Up");
        core.rb.isKinematic = true;
    }

    Vector3 GetMidPointPosition()
    {
        RaycastHit hit;
        if(Physics.Raycast(midCheck.position, Vector3.down, out hit, 3,  groundLayers))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public override NonPlayableCharacterController.npcStates ChangeState(NonPlayableCharacterCore core)
    {
        if(!isOnGround)
            return core.defaultState;
        return NonPlayableCharacterController.npcStates.none;
    }

    public override void ExitState(NonPlayableCharacterCore core)
    {
        core.rb.isKinematic = true;
    }
}
