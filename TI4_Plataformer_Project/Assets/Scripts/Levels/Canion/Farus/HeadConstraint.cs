using UnityEngine;

public class HeadConstraint : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Player.instance.transform.position - transform.position) * Quaternion.Euler(70,0,0);
    }
}
