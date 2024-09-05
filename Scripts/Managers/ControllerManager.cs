using UnityEngine;
using System.Collections;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager instance;

    [Header("Player")]
    public PlayerController playerController;
    public GameObject playerPrefab;
    public ThirdPersonCamera playerCamera;

    [Header("Vehicle")]
    [SerializeField] VehicleController vehicleController;
    [SerializeField] VehicleCamera vehicleCamera;

    [Header("Transition")]
    [SerializeField] float transitionTime;
    Timer transitionTimer = new Timer();
    bool isDriving;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ControllerInit()
    {
        if(playerController != null)
            playerController.PlayerInit();

        if (playerCamera != null)
        {
            playerCamera.CameraInit();
            playerCamera.SetCameraPosition(playerController.transform, false);
        }

        if(vehicleCamera != null)
            vehicleCamera.CameraInit();

        if (vehicleController !=  null)
        {
            vehicleController.VehicleInit();
            
            vehicleCamera.SetCameraPosition(vehicleController);
            vehicleCamera.gameObject.SetActive(true);
            playerController.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(false);
            isDriving = true;
        }
        else
        {
            if(vehicleCamera != null)
                vehicleCamera.gameObject.SetActive(false);
            isDriving = false;
        }
    }

    public void ControllerUpdate(InputContainer inputContainer, float delta)
    {
        if (!isDriving)
        {
            if (playerController != null)
            {
                playerController.PlayerUpdate(inputContainer, delta);
                if (playerCamera != null)
                    playerCamera.CameraUpdate(inputContainer, delta);
            }
        }
        else
        {
            vehicleController.VehicleUpdate(inputContainer, delta);
        }
    }

    public void ControllerFixedUpdate(float delta)
    {
        if(isDriving)
            vehicleCamera.CameraUpdate(delta);
    }

    public void EnterVehicle(VehicleController vehicle)
    {
        StartCoroutine(EnterVehicleTransition(vehicle));
    }

    public void ExitVehcile(VehicleController vehicle)
    {
        StartCoroutine(ExitVehicleTransition(vehicle));
    }

    IEnumerator EnterVehicleTransition(VehicleController vehicle)
    {
        UiManager.instance.PlaySceneClose();
        transitionTimer.StartTimer(transitionTime);
        while (transitionTimer.IsGreaterThatZero())
        {
            transitionTimer.Tick(Time.deltaTime);
            yield return null;
        }

        vehicleController = vehicle;
        vehicleController.VehicleInit();
        vehicleCamera.gameObject.SetActive(true);
        vehicleCamera.SetCameraPosition(vehicleController);

        playerController.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(false);

        isDriving = true;

        UiManager.instance.PlaySceneOpen();
        transitionTimer.StartTimer(transitionTime);
        while (transitionTimer.IsGreaterThatZero())
        {
            transitionTimer.Tick(Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator ExitVehicleTransition(VehicleController vehicle)
    {
        UiManager.instance.PlaySceneClose();
        transitionTimer.StartTimer(transitionTime);
        while (transitionTimer.IsGreaterThatZero())
        {
            transitionTimer.Tick(Time.deltaTime);
            yield return null;
        }

        playerController.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(true);
        SpawnPlayer(vehicle.exitPoint);
        
        vehicleController = null;
        vehicleCamera.gameObject.SetActive(false);

        isDriving = false;

        UiManager.instance.PlaySceneOpen();
        transitionTimer.StartTimer(transitionTime);
        while (transitionTimer.IsGreaterThatZero())
        {
            transitionTimer.Tick(Time.deltaTime);
            yield return null;
        }
    }

    public void SpawnPlayer(SpawnPoint spawnPoint)
    {
        if (playerController != null)
        {
            Destroy(playerController.gameObject);
            playerController = null;
        }

        playerController = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<PlayerController>();
        playerController.PlayerInit();

        if (playerCamera != null) 
        {
            playerCamera.SetCameraPosition(playerController.transform, spawnPoint.cameraInFront);
        }
    }
}
