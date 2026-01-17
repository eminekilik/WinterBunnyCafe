using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;

    public Transform leftSpawn;
    public Transform rightSpawn;

    float spawnInterval;

    void Start()
    {
        SetSpawnIntervalFromLevel();

        InvokeRepeating(nameof(SpawnCustomer), 1f, spawnInterval);
    }

    void SetSpawnIntervalFromLevel()
    {
        if (LevelLoader.SelectedLevel == null)
        {
            spawnInterval = 2.5f;
            return;
        }

        int gameSpeed = LevelLoader.SelectedLevel.gameSpeed;

        // hýz ? saniye dönüþümü
        switch (gameSpeed)
        {
            case 1: spawnInterval = 2.5f; break;
            case 2: spawnInterval = 2.0f; break;
            case 3: spawnInterval = 1.6f; break;
            case 4: spawnInterval = 1.3f; break;
            case 5: spawnInterval = 1.0f; break;
            case 6: spawnInterval = 0.8f; break;
            default: spawnInterval = 2.5f; break;
        }
    }

    void SpawnCustomer()
    {
        Transform spawnPoint = Random.value > 0.5f ? leftSpawn : rightSpawn;
        Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
