using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NonPlayableCharacterController : MonoBehaviour, IDamagable
{
    public enum npcStates { idle, patrol, damage, none };
    Dictionary<npcStates, NonPlayableCharacterState> states = new Dictionary<npcStates, NonPlayableCharacterState>();
    [SerializeField] npcStates defaultSate;
    npcStates currentState;
    NonPlayableCharacterCore core;

    private void Start()
    {
        GetAttachedStatesAndInit();
        currentState = defaultSate;
        core = new NonPlayableCharacterCore(transform, defaultSate);
        states[currentState].EnterState(core);
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        states[currentState].StateUpdate(core);
        CheckForStateChange();
        HandleAnimations(core, delta);
    }

    void GetAttachedStatesAndInit()
    {
        NonPlayableCharacterState[] allStates = GetComponents<NonPlayableCharacterState>();
        foreach (NonPlayableCharacterState state in allStates)
        {
            state.StateInit();
            states.Add(state.stateKey, state);
        }
    }

    void CheckForStateChange()
    {
        npcStates nextState = states[currentState].ChangeState(core);

        if (nextState != npcStates.none && states.ContainsKey(nextState))
            UpdateCurrentState(nextState);
        else
            return;
    }

    void UpdateCurrentState(npcStates nextState)
    {
        states[currentState].ExitState(core);
        currentState = nextState;
        states[currentState].EnterState(core);
    }

    void HandleAnimations(NonPlayableCharacterCore core, float delta)
    {
        core.animator.SetFloat("forward", core.forwardAmount, .25f, delta);
    }

    public void OnDamage(int damageAmount, Transform attackerTransform = null)
    {
        if (attackerTransform != null)
        {
            core.attackDir = (core.mTransform.position - attackerTransform.position).normalized;
            core.attackDir.y += 1f;
        }

        HandleDamage();
    }

    public void HandleDamage()
    {
        if(states.ContainsKey(npcStates.damage))
        {
            states[currentState].ExitState(core);
            currentState = npcStates.damage;
            states[currentState].EnterState(core);
        }
    }
}

public class NonPlayableCharacterCore 
{
    public Transform mTransform;
    public Animator animator;
    public AnimatorHook animatorHook;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public float forwardAmount = 0;
    public Vector3 attackDir = Vector3.zero;
    public NonPlayableCharacterController.npcStates defaultState;

    public NonPlayableCharacterCore(Transform mTransform, NonPlayableCharacterController.npcStates defaultState)
    {
        this.mTransform = mTransform;
        animator = mTransform.GetComponentInChildren<Animator>();
        animatorHook = mTransform.GetComponentInChildren<AnimatorHook>();
        agent = mTransform.GetComponent<NavMeshAgent>();
        rb = mTransform.GetComponent<Rigidbody>();
        this.defaultState = defaultState;
    }

    public float GetDistanceFromTarget(Transform target)
    {
        return Vector3.Distance(mTransform.position, target.position);
    }
}

