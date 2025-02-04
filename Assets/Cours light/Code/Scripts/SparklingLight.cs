using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SparklingLight : MonoBehaviour
{
    [System.Serializable] public struct MinMax { public float min; public float max; }

    [System.Serializable]
    public struct GroupLight
    {
        [HideInInspector]
        public MinMax Delay;
        [HideInInspector]
        public MinMax Duration;
        [HideInInspector]
        public MinMax minIntensity;
        [HideInInspector]
        public MinMax maxIntensity;

        public List<GameObject> Lights;
    }

    public GroupLight LightGroup;

    public enum LightType
    {
        Light, OcclusionHalo, Item25D, Item3D
    }

    public class LightObject
    {
        public Light Light = null;

        public float Intensity;
        public LightType Type;

        public LightObject(LightType type, float intensity, Light light = null)
        {
            Type = type;
            Intensity = intensity;
            Light = light;
        }

        public void SetLight(float intensity)
        {
            switch (Type)
            {
                case LightType.Light:
                    Light.intensity = intensity;
                    break;
                default:
                    break;
            }
        }
    }

    public List<LightObject> LightsList
    {
        protected set;
        get;
    }

    void Start()
    {
        GetIntensityLight();
        StartCoroutine(SparklingDelay());
    }

    public IEnumerator SparklingDelay()
    {
        while (true)
        {
            var minIntensity = Random.Range(LightGroup.minIntensity.min, LightGroup.minIntensity.max);
            var maxInstensity = Random.Range(LightGroup.maxIntensity.min, LightGroup.maxIntensity.max);
            var duration = Random.Range(LightGroup.Duration.min, LightGroup.Duration.max);
            var startTime = Time.time;
            yield return new WaitWhile(() => SparklingFade(LightGroup.Lights, minIntensity, maxInstensity, duration, startTime, Time.time) == false);
            yield return new WaitForSeconds(Random.Range(LightGroup.Delay.min, LightGroup.Delay.max));
        }
    }

    public bool SparklingFade(List<GameObject> lights, float minIntensity, float maxIntensity, float duration, float startTime, float currentTime)
    {
        if (currentTime - startTime >= duration)
        {
            return true;
        }
        else
        {
            var firstMuliplier = (minIntensity + (((duration - (currentTime - startTime)) * (maxIntensity - minIntensity)) / (duration / 2)));
            var secondMultiplier = (minIntensity + (((currentTime - startTime) * (maxIntensity - minIntensity)) / (duration / 2)));
            foreach (var light in LightsList)
            {
                if (currentTime - startTime >= duration / 2)
                {
                    light.SetLight(light.Intensity * firstMuliplier);
                }
                else
                {
                    light.SetLight(light.Intensity * secondMultiplier);
                }
            }
            return false;
        }
    }

    public void GetIntensityLight()
    {
        if (LightGroup.Lights == null) return;

        LightsList = new List<LightObject>();

        var lights = new List<Light>();

        foreach (var light in LightGroup.Lights)
        {
            foreach (var lightComponent in light.GetComponentsInChildren<Light>())
            {
                LightsList.Add(new LightObject(LightType.Light, lightComponent.intensity, lightComponent));
            }
        }
    }
}