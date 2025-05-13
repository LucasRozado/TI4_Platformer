using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{
    [SerializeField] PowerUps powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.instance.AcquirePowerUp(powerUp);
            Destroy(gameObject);
        }
    }
}
