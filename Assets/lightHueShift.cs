using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class lightHueShift : MonoBehaviour
{
    Light light;
    Color startColor;
    float hue;
    [SerializeField] float speed;

    private void Start()
    {
        light = GetComponent<Light>();
        startColor = light.color;
    }

    // Update is called once per frame
    void Update()
    {
        float h, s, v;
        Color.RGBToHSV(startColor,out h,out s,out v);
        h += Time.time*speed;
        h = h % 1;
        light.color = Color.HSVToRGB(h,s,v);
    }
}
