/**
 * Alexandre Ouellet - 9 octobre 2022
 */
using UnityEngine;

/// <summary>
/// Calculation regarding the Bezier' curves
/// </summary>
public static class Bezier 
{
    /// <summary>
    /// Strategy for numeric integral approximation using trapeze.
    /// </summary>
    public enum IntegrationStrategy
    {
        // Value are the number of steps in the approximation computation
        FAST = 20,
        BALANCED = 50,
        PRECISE = 100
    }

    /// <summary>
    /// Compute a position on a three dimentionnal Bezier's curve.
    /// </summary>
    /// <param name="p0">First anchor point of the Beizer's curve.</param>
    /// <param name="p1">Control point associated to the first anchor point of the Beizer's curve.</param>
    /// <param name="p2">Control point associated to the second anchor point of the Beizer's curve.</param>
    /// <param name="p3">Second anchor point of the Beizer's curve.</param>
    /// <param name="t">Progression along the curve between 0 and 1.</param>
    /// <returns>The position at parametrized position t.</returns>
    public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (1.0f - t) * (1.0f - t) * (1.0f - t) * p0 + 3.0f * (1.0f - t) * (1.0f - t) * t * p1 + 3.0f * (1 - t) * t * t * p2 + t * t * t * p3;
    }

    /// <summary>
    /// Compute a position on the derivative function of a three dimentionnal Bezier's curve.
    /// </summary>
    /// <param name="p0">First anchor point of the Beizer's curve.</param>
    /// <param name="p1">Control point associated to the first anchor point of the Beizer's curve.</param>
    /// <param name="p2">Control point associated to the second anchor point of the Beizer's curve.</param>
    /// <param name="p3">Second anchor point of the Beizer's curve.</param>
    /// <param name="t">Progression along the curve between 0 and 1.</param>
    /// <returns>The position at parametrized position t on the derivate curve.</returns>
    public static Vector3 DerivativeCubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 3.0f * (1.0f - t) * (1.0f - t) * (p1 - p0) + 6.0f * (1.0f - t) * t * (p2 - p1) + 3.0f * t * t * (p3 - p2);
    }

    /// <summary>
    /// Compute the length of a Bezier's curve using numeric integral.
    /// </summary>
    /// <param name="p0">First anchor point of the Beizer's curve.</param>
    /// <param name="p1">Control point associated to the first anchor point of the Beizer's curve.</param>
    /// <param name="p2">Control point associated to the second anchor point of the Beizer's curve.</param>
    /// <param name="p3">Second anchor point of the Beizer's curve.</param>
    /// <param name="integrationStrategy">Integration strategy used to obtained the value. More precise calculation require
    /// more time.</param>
    /// <returns>The length of the curve.</returns>
    public static float CubicBezierCurveLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, IntegrationStrategy integrationStrategy = IntegrationStrategy.PRECISE)
    {
        int integrationStepCount = (int)integrationStrategy;

        float area = 0f;
        float step = 1.0f / integrationStepCount;
        float start = 0f;
        float end = step;

        for(int i = 0; i < integrationStepCount; i++)
        {
            area += (DerivativeCubicBezier(p0, p1, p2, p3, start).magnitude + DerivativeCubicBezier(p0, p1, p2, p3, end).magnitude) * step * 0.5f;
        }

        return area;
    }
}
