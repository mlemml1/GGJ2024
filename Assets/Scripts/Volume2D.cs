using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume2D : MonoBehaviour
{
    public Transform listenerTransform;
    public AudioSource audioSource;
    public float minDist = 1;
    public float maxDist = 400;
    public float volumeScale = 1.0f;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, listenerTransform.position);

        if (dist < minDist)
        {
            audioSource.volume = 1 * volumeScale;
        }
        else if (dist > maxDist)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = (1 - ((dist - minDist) / (maxDist - minDist))) * volumeScale;
        }
    }
}