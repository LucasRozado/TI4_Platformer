using System.Linq;
using UnityEngine;

public class MazeWaypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            if (other.TryGetComponent<MammothMaze>(out MammothMaze mammoth))
            {
                mammoth.ChangeWaypoint();
            }
            else
            {
                Debug.Log("Mammoth nao encontrado");
            }
        }
    }
}
