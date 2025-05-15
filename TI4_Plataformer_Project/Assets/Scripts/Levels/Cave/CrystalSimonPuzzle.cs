using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalSimonPuzzle : MonoBehaviour
{
    bool isSolved = false;

    [SerializeField] private Activated onSolveActivate;

    private int currentStep = 0;
    private int currentInteraction = 0;

    private Crystal[] crystals;
    private int[] order;
    void Start()
    {
        crystals = GetComponentsInChildren<Crystal>();

        order = new int[crystals.Length];

        List<int> indexes = new();
        indexes.AddRange(Enumerable.Range(0, crystals.Length));
        indexes.AddRange(Enumerable.Range(0, crystals.Length));
        for (int i = 0; i < order.Length; i++)
        {
            int random = Random.Range(0, indexes.Count);
            order[i] = indexes[random];
            indexes.RemoveAt(random);
        }

        for (int i = 0; i < crystals.Length; i++)
        {
            Crystal crystal = crystals[i];
            crystal.onInteractionFinish.AddListener(Validate);
        }

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

    private void Glow(Crystal crystal)
    {
        StartCoroutine(Glow_Coroutine(crystal));
    }
    private IEnumerator Glow_Coroutine(Crystal crystal)
    {
        crystal.TurnOn();
        yield return new WaitForSeconds(1f);
        crystal.TurnOff();
    }

    private void Play()
    {
        StartCoroutine(Play_Coroutine());
    }
    private IEnumerator Play_Coroutine()
    {
        foreach (Crystal crystal in crystals)
        { crystal.DisableToggle();}

        foreach (int current in order)
        {
            Crystal crystal = crystals[current];
            yield return Glow_Coroutine(crystal);
            yield return new WaitForSeconds(0.5f);
        }

        foreach (Crystal crystal in crystals)
        { crystal.EnableToggle(); }
    }
}
