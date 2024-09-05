using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [HideInInspector]
    public string nextSpawnPointID = "";
    [SerializeField] string spawnTag;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public SpawnPoint GetNextSpawnPoint()
    {
        if (nextSpawnPointID == "")
            return null;

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(spawnTag);
        foreach(GameObject sp in spawnPoints)
        {
            if (sp.name == nextSpawnPointID)
            {
                nextSpawnPointID = "";
                return sp.GetComponent<SpawnPoint>();
            }
        }

        Debug.LogError("Could not find spawn point: " + nextSpawnPointID);
        nextSpawnPointID = "";
        return null;
    }
}
