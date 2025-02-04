using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SparklingLight))]
[CanEditMultipleObjects]
public class SparklingEditor : Editor
{
    SparklingLight Target => target as SparklingLight;

    float _rdDelay;
    float _rdDuration;
    float _rdMinIntensity;
    float _rdMaxIntensity;
    double _startTime;
    double _nextActionAt;

    bool _previewState = true;

    void OnEnable()
    {
        _previewState = true;
    }

    void OnDisable()
    {
        StopPreview();
    }

    void StartPreview()
    {
        Target.GetIntensityLight();
        SetInitialvalue();
        EditorApplication.update += SparklingDelay;
    }

    void StopPreview()
    {
        EditorApplication.update -= SparklingDelay;
        ResetLights();
    }

    void SparklingDelay()
    {
        if (EditorApplication.timeSinceStartup >= _nextActionAt)
        {
            if (Target.SparklingFade(Target.LightGroup.Lights, _rdMinIntensity, _rdMaxIntensity, _rdDuration, (float)_nextActionAt, (float)EditorApplication.timeSinceStartup) == true)
            {
                SetNewLightValue();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginChangeCheck();

        EditorGUIUtility.labelWidth = 1f;

        EditorGUILayout.LabelField(new GUIContent("Delay", "Time to wait between each blinking"));
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Min : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(28));
            Target.LightGroup.Delay.min = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.Delay.min);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Max : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(31));
            Target.LightGroup.Delay.max = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.Delay.max);
        }

        GUILayout.Space(30);

        EditorGUILayout.LabelField(new GUIContent("Duration", "Blinking duration"));
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Min : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(28));
            Target.LightGroup.Duration.min = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.Duration.min);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Max : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(31));
            Target.LightGroup.Duration.max = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.Duration.max);
        }

        GUILayout.Space(30);

        EditorGUILayout.LabelField(new GUIContent("DefaultIntensity", "The initial intensity multiplier that persists when there's no blinking"));
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Min : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(28));
            Target.LightGroup.minIntensity.min = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.minIntensity.min);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Max : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(31));
            Target.LightGroup.minIntensity.max = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.minIntensity.max);
        }
        EditorGUILayout.MinMaxSlider(ref Target.LightGroup.minIntensity.min, ref Target.LightGroup.minIntensity.max, 0f, 2f);

        GUILayout.Space(30);

        EditorGUILayout.LabelField(new GUIContent("PeakIntensity", "The peak intensity multiplier that is reached in the middle of the blink animation"));

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Min : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(28));
            Target.LightGroup.maxIntensity.min = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.maxIntensity.min);
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Max : ", GUILayout.ExpandWidth(false), GUILayout.MaxWidth(31));
            Target.LightGroup.maxIntensity.max = EditorGUILayout.FloatField(GUIContent.none, Target.LightGroup.maxIntensity.max);
        }
        EditorGUILayout.MinMaxSlider(ref Target.LightGroup.maxIntensity.min, ref Target.LightGroup.maxIntensity.max, 0f, 2f);

        GUILayout.Space(50);

        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();

            if (_previewState)
            {
                if (GUILayout.Button("Start Preview"))
                {
                    StartPreview();
                    _previewState = false;
                }
            }
            else
            {
                if (GUILayout.Button("Stop Preview"))
                {
                    StopPreview();
                    _previewState = true;
                }
            }

            GUILayout.FlexibleSpace();

            GUILayout.Space(30);
        }
    }

    void ResetLights()
    {
        if (target == null) return;
        if (Target.LightsList == null) return;

        foreach (var light in Target.LightsList)
        {
            light.SetLight(light.Intensity);
        }
    }

    void SetInitialvalue()
    {
        _rdMinIntensity = Random.Range(Target.LightGroup.minIntensity.min, Target.LightGroup.minIntensity.max);
        _rdMaxIntensity = Random.Range(Target.LightGroup.maxIntensity.min, Target.LightGroup.maxIntensity.max);
        _rdDuration = Random.Range(Target.LightGroup.Duration.min, Target.LightGroup.Duration.max);
        _rdDelay = Random.Range(Target.LightGroup.Delay.min, Target.LightGroup.Delay.max);
        _startTime = EditorApplication.timeSinceStartup;
    }

    void SetNewLightValue()
    {
        _rdMinIntensity = Random.Range(Target.LightGroup.minIntensity.min, Target.LightGroup.minIntensity.max);
        _rdMaxIntensity = Random.Range(Target.LightGroup.maxIntensity.min, Target.LightGroup.maxIntensity.max);
        _rdDuration = Random.Range(Target.LightGroup.Duration.min, Target.LightGroup.Duration.max);
        _rdDelay = Random.Range(Target.LightGroup.Delay.min, Target.LightGroup.Delay.max);
        _startTime = EditorApplication.timeSinceStartup;
        _nextActionAt = _startTime + _rdDelay;
    }

}
