using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffectsManager : MonoBehaviour
{
    ParticleSystem[] particleSystems;
    // TODO: Add rubber burning trails (trail renderer)

    CarController car;
    bool particlesEnabled = false;

    void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        car = GetComponentInParent<CarController>();

        EnableParticleEmissions(false);
    }

    void Update()
    {
        if (!particlesEnabled && Mathf.Abs(car.lateralSpeed) > 5f)
        {
            particlesEnabled = true;
            EnableParticleEmissions(true);
        }
        else if (particlesEnabled && Mathf.Abs(car.lateralSpeed) <= 5f)
        {
            particlesEnabled = false;
            EnableParticleEmissions(false);
        }

    }

    void EnableParticleEmissions(bool enabled)
    {
        foreach (ParticleSystem p in particleSystems)
        {
            ParticleSystem.EmissionModule emission = p.emission;
            emission.enabled = enabled;
        }
    }
}
