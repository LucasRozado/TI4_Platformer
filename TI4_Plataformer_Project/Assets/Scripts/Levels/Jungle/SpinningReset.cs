using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpinningReset : MonoBehaviour
{
    [SerializeField] GameObject[] platforms;
    [SerializeField] Quaternion[] rotations;
    Quaternion[] newRotations;
    [SerializeField] float duration = 2f;

    private void Start()
    {
        rotations = new Quaternion[platforms.Length];
        newRotations = new Quaternion[platforms.Length];
        for ( int i = 0; i < platforms.Length; i++ )
        {
            rotations[i] = transform.localRotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            for (int i = 0; i < platforms.Length; i++)
            {
                newRotations[i] = transform.localRotation;
            }
            StartCoroutine(CRResetRotation());
        }        
    }

    public IEnumerator CRResetRotation()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].transform.localRotation = Quaternion.Lerp(newRotations[i], rotations[i], t/duration);
            }            
            yield return null;
        }
        Debug.Log("Spin");
        yield return null;
    }
}
