using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnPointID = "";
    public Vector3 position { get { return transform.position; } }
    public Quaternion rotation { get { return transform.rotation; } }
    public bool cameraInFront = true;
}
