using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class GameManager : MonoBehaviour
{
    ControllerManager controllerManager;
    InputHandler inputHandler;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        inputHandler.InputHandlerInit();

        controllerManager = GetComponent<ControllerManager>();
        controllerManager.ControllerInit();

        SpawnPoint spawnPoint = SpawnManager.instance.GetNextSpawnPoint();

        if(spawnPoint != null )
        {
            controllerManager.SpawnPlayer(spawnPoint);
        }
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        inputHandler.InputHandlerUpdate();

        if (controllerManager != null)
            controllerManager.ControllerUpdate(inputHandler.inputContainer, delta);
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (controllerManager != null)
            controllerManager.ControllerFixedUpdate(delta);
    }

    private void LateUpdate()
    {
        inputHandler.InputHandlerLateUpdate();
    }
}
