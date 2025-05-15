using UnityEngine;

public class CrystalLightPuzzle : MonoBehaviour
{
    bool isSolved = false;

    [SerializeField] private Activated onSolveActivate;

    private Crystal[] crystals;
    void Start()
    {
        crystals = GetComponentsInChildren<Crystal>();

        foreach (Crystal crystal in crystals)
        { crystal.onInteractionFinish.AddListener(Validate); }

        if (isSolved)
        { HandleSolve(); }
    }

    private void Validate()
    {
        // Nao foi resolvido se qualquer um dos cristais nao estiver ligado
        foreach (Crystal crystal in crystals)
        { if (!crystal.IsOn) return; }

        // Se todos estiverem, faz os procedimentos pos solucao
        HandleSolve();
    }

    private void HandleSolve()
    {
        isSolved = true;

        foreach (Crystal crystal in crystals)
        {
            crystal.onInteractionFinish.RemoveListener(Validate);
            crystal.DisableToggle();
        }

        onSolveActivate.Activate();
    }
}
