using UnityEngine;

public class PatrolState : NonPlayableCharacterState
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float minimumDistanceFromWaypointTarget;
    [SerializeField] float minimumDistanceFromTargetBeforeIdle;
    Transform currentWaypoint;
    int waypointCounter;
    Transform player;

    public override void StateInit()
    {
        stateKey = NonPlayableCharacterController.npcStates.patrol;
    }

    public override void EnterState(NonPlayableCharacterCore core)
    {
        core.agent.enabled = true;
        if(currentWaypoint == null)
            currentWaypoint = waypoints[0];
        core.agent.SetDestination(currentWaypoint.position);
        core.forwardAmount = .5f;
    }

    public override void StateUpdate(NonPlayableCharacterCore core)
    {
        player = ControllerManager.instance.playerController.transform;
        float distanceFromTarget = Vector3.Distance(core.mTransform.position, currentWaypoint.position);
        if (distanceFromTarget <= minimumDistanceFromWaypointTarget)
        {
            waypointCounter++;
            if(waypointCounter > waypoints.Length - 1)
                waypointCounter = 0;
            currentWaypoint = waypoints[waypointCounter];
            core.agent.SetDestination(currentWaypoint.position);
        }       
    }

    public override NonPlayableCharacterController.npcStates ChangeState(NonPlayableCharacterCore core)
    {
        if (player != null && core.GetDistanceFromTarget(player) <= minimumDistanceFromTargetBeforeIdle)
            return NonPlayableCharacterController.npcStates.idle;
        return NonPlayableCharacterController.npcStates.none;
    }

    public override void ExitState(NonPlayableCharacterCore core)
    {
        core.agent.enabled = false;
        core.forwardAmount = 0;
    } 
}
