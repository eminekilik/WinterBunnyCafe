using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;

    public Transform leftSpawn;
    public Transform rightSpawn;

    public float spawnInterval = 4f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnCustomer), 1f, spawnInterval);
    }

    void SpawnCustomer()
    {
        Transform spawnPoint = Random.value > 0.5f ? leftSpawn : rightSpawn;
        Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
