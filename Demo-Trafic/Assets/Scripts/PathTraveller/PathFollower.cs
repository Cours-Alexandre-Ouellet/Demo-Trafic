/**
 * Alexandre Ouellet - 9 octobre 2022
 */
using UnityEngine;

/// <summary>
/// Follows the path defined by a Bezier's curve
/// </summary>
public class PathFollower : MonoBehaviour
{
    public Path Path { get; private set; }      // Path followed
                     
    public float Speed { get; set; }            // Speed at which to move

    private int currentSegmentIndex;            // Index of the path segment the object is currently on

    private float progression;                  // Progression on the current path segement

    private Quaternion forward;                 // Local forward vector

    /// <summary>
    /// Indicates if the follower has reached the end of the path. If the path is closed, 
    /// end of path will never be reached.
    /// </summary>
    public bool ReachEndOfPath { get; private set; }

    /// <summary>
    /// Sets the base forward orientation of the object
    /// </summary>
    /// <param name="forward">The vector of the forward orientation</param>
    public void SetForward(Vector3 forward)
    {
        this.forward = Quaternion.LookRotation(forward);
    }

    /// <summary>
    /// Changes the path of the follower. If the path is dirty, then it is recalculated.
    /// </summary>
    /// <param name="path">The path to be followed by the object.</param>
    public void SetPath(Path path)
    {
        this.Path = path;
        if (path.IsDirty)
        {
            path.ComputePathLength();
        }
    }

    /// <summary>
    /// Resets the path following attribute (such as ReachEndOfPath)
    /// </summary>
    public void ResetPathFollowingAttribute()
    {
        ReachEndOfPath = false;
    }

    /// <summary>
    /// Calculates the position of the object if it moves forward for a frame.
    /// </summary>
    /// <returns>The position of the object after having moved forward for a frame.</returns>
    public (Vector3, Quaternion) MoveForward()
    {
        float displacement = Time.deltaTime * Speed;
        float t = 2.0f;

        while (displacement > 0.0f && !ReachEndOfPath && t > 1.0f)
        {
            float segmentLength = Path.SegmentsLength[currentSegmentIndex];
            t = displacement / segmentLength + progression;
            if (t > 1.0f)
            {
                displacement -= (1.0f - progression) * segmentLength;
                progression = 0.0f;
                if (currentSegmentIndex < Path.NumSegments - 1 || Path.IsClosed)
                {
                    currentSegmentIndex = (currentSegmentIndex + 1) % Path.NumSegments;
                }
                else
                {
                    ReachEndOfPath = true;
                    progression = 1.0f;
                }
            }
        }
        Vector3[] points = Path.GetPointsInSegment(currentSegmentIndex);

        progression = t;
        return (Bezier.CubicBezier(points[0], points[1], points[2], points[3], t), 
            Quaternion.LookRotation(Bezier.DerivativeCubicBezier(points[0], points[1], points[2], points[3], t), Vector3.up) * forward);
    }

    /// <summary>
    /// Calculates the position of the object if it moves backward for a frame.
    /// </summary>
    /// <returns>The position of the object after having moved backward for a frame.</returns>
    public (Vector3, Quaternion) MoveBackward()
    {
        float displacement = Time.deltaTime * Speed;
        float t = -2.0f;

        while (displacement > 0.0f && !ReachEndOfPath && t < 0.0f)
        {
            float segmentLength = Path.SegmentsLength[currentSegmentIndex];
            t = progression - displacement / segmentLength;
            if (t < 0.0f)
            {
                displacement -= progression * segmentLength;
                progression = 1.0f;
                if (currentSegmentIndex > 0 || Path.IsClosed)
                {
                    currentSegmentIndex = (currentSegmentIndex - 1 + Path.NumSegments) % Path.NumSegments;
                }
                else
                {
                    ReachEndOfPath = true;
                    progression = 0.0f;
                }
            }
        }

        Vector3[] points = Path.GetPointsInSegment(currentSegmentIndex);
        progression = t;
        return (Bezier.CubicBezier(points[0], points[1], points[2], points[3], t),
            Quaternion.LookRotation(Bezier.DerivativeCubicBezier(points[0], points[1], points[2], points[3], t), Vector3.up) * forward);
    }

    /// <summary>
    /// Returns the progression over the total length of the path as a ratio.
    /// </summary>
    /// <param name="forward">If the follower is travelling forward.</param>
    /// <returns>The percentage of the path that has been traveled.</returns>
    public float GetMilestone(bool forward)
    {
        float travaledDistance = 0.0f;

        if (forward)
        {
            for (int i = 0; i < currentSegmentIndex; i++)
            {
                travaledDistance += Path.SegmentsLength[i];
            }
            travaledDistance += Path.SegmentsLength[currentSegmentIndex] * progression;
        }
        else
        {
            for (int i = Path.NumSegments - 1; i > currentSegmentIndex; i--)
            {
                travaledDistance += Path.SegmentsLength[i];
            }
            travaledDistance += Path.SegmentsLength[currentSegmentIndex] * (1.0f - progression);
        }

        return travaledDistance / Path.PathLength;
    }
}
