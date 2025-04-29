using UnityEngine;

public class TemporyPlatform : MonoBehaviour
{
    [SerializeField] Collider platformCollider;
    public void DisablePlatform()
    {
        platformCollider.enabled = false;
    }

    public void EnablePlatform()
    {
        platformCollider.enabled = true;
    }
}
