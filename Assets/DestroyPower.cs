using UnityEngine;

public class DestroyPower : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ball"))
        {

            Destroy(gameObject);
        }
    }
}