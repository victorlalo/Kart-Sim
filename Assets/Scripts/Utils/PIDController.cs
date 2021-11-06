using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIDController : MonoBehaviour
{
    [SerializeField] SO_SteeringPID pidVals;

    float error_prev = 0f;
    float error_sum = 0f;

    public float GetFactor(float error)
    {
        float factor = 0f;

        // Proportional - fraction of correction to apply (Biggest factor)
        factor = pidVals.kP * error;


        // Integral - sum of all previous errors
        // Minimize built-in error
        error_sum += Time.fixedDeltaTime * error;

        // average of last 20 errors, not ALL errors
        error_sum = error_sum + ((error - error_sum) / 20f);
        factor += pidVals.kI * error_sum;


        // Derivative - Change since last time step (delta)
        // Dampens/ Smooths correction
        float error_deriv = (error - error_prev) / Time.fixedDeltaTime;
        factor += pidVals.kD * error_deriv;
        error_prev = error;


        return factor;

    }

    public void ClearErrorVariables()
    {
        error_prev = 0f;
        error_sum = 0f;
    }
}
