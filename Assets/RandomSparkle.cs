using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSparkle : MonoBehaviour
{
    Light light;
    [SerializeField]AnimationCurve curve;
    [SerializeField]float minDelay, maxDelay, minDuration, MaxDuration,minIntensity,MaxIntensity;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(c());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator c()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay,maxDelay));


            float duration = Random.Range(minDuration, MaxDuration);
            float i = Random.Range(minIntensity, MaxIntensity);
            float StartTime = Time.time;
            float EndTime = Time.time + duration;
            light.intensity = curve.Evaluate(0);
            while (Time.time < EndTime)
            {
                float alpha = Mathf.InverseLerp(StartTime, EndTime, Time.time);
                light.intensity = curve.Evaluate(alpha)* i;
                yield return null;
            }
            light.intensity = curve.Evaluate(1);
        }
        
    }
}
