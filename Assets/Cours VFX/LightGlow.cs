using System.Collections.Generic;
using UnityEngine;

public class LightGlow : MonoBehaviour
{
    [SerializeField] private List<Light> _pointLightList = new List<Light>();
    [SerializeField] private Color _color;
    [SerializeField] private float _intensityMin;
    [SerializeField] private float _intensityMax;

    private void Update()
    {
        foreach (Light light in _pointLightList)
        {
            light.intensity = Random.Range(_intensityMin, _intensityMax);
            light.color = _color;
        }
    }
}
