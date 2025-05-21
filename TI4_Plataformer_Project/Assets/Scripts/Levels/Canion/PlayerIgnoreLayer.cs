using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerIgnoreLayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.IgnoreLayerCollision(7, 10); 
        Physics.IgnoreLayerCollision(10, 10);
    }
}
