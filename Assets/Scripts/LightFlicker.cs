using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light lightSource;
    float baseIntensity;

    void Start()
    {
        lightSource = GetComponent<Light>();
        baseIntensity = lightSource.intensity;
    }

    void Update()
    {
        lightSource.intensity = baseIntensity + Random.Range(-0.2f, 0.2f);
    }
}
