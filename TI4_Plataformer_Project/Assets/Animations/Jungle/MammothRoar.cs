using UnityEngine;

public class MammothRoar : MonoBehaviour
{
    [SerializeField] MammothMaze mammoth;
    public void BeginCharge()
    {
        mammoth.BeginCharge();
    }
}
