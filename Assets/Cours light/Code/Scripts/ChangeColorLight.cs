using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorLight : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 0.5f; // Valeur minimale d'intensit�
    public float maxIntensity = 2.0f; // Valeur maximale d'intensit�

    void Start()
    {
        StartCoroutine(ChangeLightColor());
        StartCoroutine(ChangeIntensity());
    }

    IEnumerator ChangeLightColor()
    {
        while (true)
        {
            lightSource.color = new Color(Random.value, Random.value, Random.value);
            yield return new WaitForSeconds(0.8f);
        }
    }

    IEnumerator ChangeIntensity()
    {
        while (true)
        {
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
