using UnityEngine;

public abstract class NonPlayableCharacterState : MonoBehaviour
{
    [HideInInspector]
    public NonPlayableCharacterController.npcStates stateKey;

    public abstract void StateInit();
    public abstract void EnterState(NonPlayableCharacterCore core);
    public abstract void StateUpdate(NonPlayableCharacterCore core);
    public abstract NonPlayableCharacterController.npcStates ChangeState(NonPlayableCharacterCore core);
    public abstract void ExitState(NonPlayableCharacterCore core);
}
