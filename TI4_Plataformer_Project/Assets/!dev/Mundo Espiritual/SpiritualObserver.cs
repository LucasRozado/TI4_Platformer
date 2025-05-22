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
            Debug.Log(instance.gameObject.name);
        }
        else
        {
            Debug.Log("no instance");
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
