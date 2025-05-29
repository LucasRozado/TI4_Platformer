using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiritualObserver : MonoBehaviour
{
    public static SpiritualObserver instance;
    bool isSpiritualReality;
    List<SpiritualObject> spiritualObjects = new List<SpiritualObject>();

    private void Awake()
    {
        if(instance == null) 
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Subscribe(SpiritualObject spiritualObject)
    {
        spiritualObjects.Add(spiritualObject);
    }

    public void SwitchReality()
    {
        foreach (SpiritualObject obj in spiritualObjects)
        {
            obj.SwitchReality(isSpiritualReality);
        }
        isSpiritualReality = !isSpiritualReality;
    }
}
