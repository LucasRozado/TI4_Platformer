using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableList", menuName = "Scriptable Objects/CollectableList")]
public class CollectableList : ScriptableObject
{
    [SerializeField] CollectableType type;

    [SerializeField] List<Collectable> list;
}
