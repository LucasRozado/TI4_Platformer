using System.Linq;
using UnityEngine;

public class MammothWaypoint : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform finalWaypoint;
    [SerializeField] Transform substituteFinal;


    public Vector3 GetRandomWaypoint(Vector3 currentWaypoint, bool finalWaypoint)
    {
        if (finalWaypoint)
        {
            return GetFinalWaypoint();
        }
        int random;
        do
        {
            random = Random.Range(0, waypoints.Length);
        }
        while (waypoints[random].position == currentWaypoint);
        return waypoints[random].position;
    }

    public Vector3 GetFinalWaypoint()
    {
        if (waypoints.Contains(finalWaypoint))
        {
            return finalWaypoint.position;
        }
        return substituteFinal.position;
    }

    private void OnDrawGizmos()
    {
        foreach (Transform t in waypoints)
        {
            Gizmos.DrawLine(transform.position, t.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            if (other.TryGetComponent<MammothMaze>(out MammothMaze mammoth))
            {
                mammoth.ChangeWaypoint(GetRandomWaypoint(mammoth.GetLastWaypoint(), mammoth.isFinal));
            }
            else
            {
                Debug.Log("Mammoth nao encontrado");
            }
        }
    }
}
