using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab1; 
    public GameObject powerUpPrefab2;
    public GameObject powerUpPrefab3; 
    public Vector2 spawnAreaSize; 

    private GameObject currentPowerUp; 

    void Start()
    {
        InvokeRepeating("SpawnPowerUp", 5f, 5f);
    }

    void SpawnPowerUp()
    {

        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
        }

        int randomIndex = Random.Range(0, 3);
        GameObject powerUpPrefab;
        switch (randomIndex)
        {
            case 0:
                powerUpPrefab = powerUpPrefab1;
                break;
            case 1:
                powerUpPrefab = powerUpPrefab2;
                break;
            case 2:
                powerUpPrefab = powerUpPrefab3;
                break;
            default:
                powerUpPrefab = powerUpPrefab1;
                break;
        }

   
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            -5f,
            transform.position.z + Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
        );

 
        currentPowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        

        Invoke("DestroyPowerUp", 5f);
    }

    void DestroyPowerUp()
    {

        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
        }
    }
}
