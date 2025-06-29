using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{
    [SerializeField] PowerUps powerUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.powerUp.AcquirePowerUp(powerUp);
            Destroy(gameObject, 2f); // Destroy the power-up after 2 seconds
            UIManager.instance.ShowText("You have gained the nimbleness of the Farus Monster. Jump to CLIMB", 15f);
        }
    }
}
