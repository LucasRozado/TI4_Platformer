using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{
    [SerializeField] PowerUps powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.powerUp.AcquirePowerUp(powerUp);
            Destroy(gameObject);
        }
    }
}
