/**
 * Aadpté de 
 * LAGUE S, [Unity] 2D Curve Editor (E01 : introduction and concepts), télévsersée le 21 janvier 2018 (https://www.youtube.com/watch?v=RF04Fi9OCPc)
 * LAGUE S, [Unity] 2D Curve Editor (E02 : adding and moving points), télévsersée le 25 janvier 2018 (https://www.youtube.com/watch?v=n_RHttAaRCk) 
 * LAGUE S, [Unity] 2D Curve Editor (E03 : closed path and auto-controls), télévsersée le 21 janvier 2018 (https://www.youtube.com/watch?v=nNmFLWup4_k) 
 * 
 * Alexandre Ouellet - 9 octobre 2022
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data about a path composed of multiple cubic Bezier's curves. Path is serializable, so it can be setup in editor 
/// and value will be maintained for runtime. Controls points are defined in a way that the path is continuous and it doesn't
/// present spike points (e.g. derivative function exists everywhere).
/// </summary>
[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    private List<Vector3> points;                   // List of points, multiple of 3 are anchors

    public bool IsClosed => isClosed;               // Accessor of isClosed

    [SerializeField, HideInInspector]
    private bool isClosed;                          // Whether the path isLooping around (closed) or not

    public float[] SegmentsLength { get; private set; }     // Length of each segments of the path

    public float PathLength { get; private set; }           // Total length of the path

    [SerializeField, HideInInspector]
    private bool isDirty;                           // Indicates if the length value are correct or if they need to be recalculated

    public bool IsDirty => isDirty;                 // Accessor of isDirty

    public int NumSegments => points.Count / 3;     // Number of segments in the path

    public int NumPoints => points.Count;           // Number of points in the path

    public Vector3 End => points[isClosed ? NumPoints - 3 : NumPoints - 1];    // Last point of the path

    public Vector3 Start => points[0];              // First point of the path

    [SerializeField, HideInInspector]
    private string name;                             // Unique name for the path

    public string Name { get => name; set => name = value; }    // Accessor and mutator for name

    [SerializeField, HideInInspector]
    private bool autoSetControlPoints;              // Wheter the control points are automatically setup

    public bool AutoSetControlPoints                // Accessor and mutator for autoSetControlPoints 
    {
        get => autoSetControlPoints;
        set
        {
            // Recalculate control points only if needed
            if (value != autoSetControlPoints)
            {
                autoSetControlPoints = value;
                AutoSetAllControlPoints();
            }
        }
    }

    /// <summary>
    /// Create a path at the specified position
    /// </summary>
    /// <param name="center">Center point for the origin of the path. The path is ensured to passed through this point.</param>
    /// <param name="name">The uynique name of the path.</param>
    public Path(Vector3 center, string name)
    {
        points = new List<Vector3>()
        {
            center + Vector3.left,
            center + (Vector3.left + Vector3.forward) * 0.5f,
            center + (Vector3.right + Vector3.back) * 0.5f,
            center + Vector3.right
        };

        isClosed = false;
        isDirty = true;
        this.name = name;
    }

    /// <summary>
    /// Indexor to access point based on integer index. 
    /// </summary>
    /// <param name="i">The index of the points.</param>
    /// <returns>The point at index i.</returns>
    public Vector3 this[int i]
    {
        get
        {
            return points[i];
        }
        set
        {
            points[i] = value;
        }
    }

    /// <summary>
    /// Computes the segment length array and the total path lenght. Method is not efficient as all
    /// length are computed. Must be call at the end of all modifications.
    /// </summary>
    public void ComputePathLength()
    {
        isDirty = false;

        SegmentsLength = new float[NumSegments];
        PathLength = 0.0f;

        for (int i = 0; i < NumSegments; i++)
        {
            Vector3[] points = GetPointsInSegment(i);
            float segmentLength = Bezier.CubicBezierCurveLength(points[0], points[1], points[2], points[3]);

            SegmentsLength[i] = segmentLength;
            PathLength += segmentLength;
        }
    }

    /// <summary>
    /// Adds a new segment to the curve by adding a new anchor point.
    /// </summary>
    /// <param name="anchorPos">The new anchor point to append to the path.</param>
    public void AddSegment(Vector3 anchorPos)
    {
        int pointCount = points.Count;
        points.Add(points[pointCount - 1] * 2 - points[pointCount - 2]);
        // length of array increased by 1
        points.Add((points[pointCount] + anchorPos) * 0.5f);
        points.Add(anchorPos);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectdControlPoints(pointCount - 1);
        }

        isDirty = true;
    }

    /// <summary>
    /// Returns points in the segment specified by the index. Method does not verified if segmentIndex is in range.
    /// </summary>
    /// <param name="segmentIndex">The index of the segment from which get the points.</param>
    /// <returns>Array of points from the segment.</returns>
    public Vector3[] GetPointsInSegment(int segmentIndex)
    {
        return new Vector3[] { points[segmentIndex * 3], points[segmentIndex * 3 + 1], points[segmentIndex * 3 + 2], points[LoopIndex(segmentIndex * 3 + 3)] };
    }

    /// <summary>
    /// Moves a the point at the given index to the specified position.
    /// </summary>
    /// <param name="index">Index of the point to move.</param>
    /// <param name="position">New position of the point.</param>
    public void MovePoint(int index, Vector3 position)
    {
        Vector3 deltaPosition = position - points[index];

        // If auto set, then only anchors can be moved
        if (index % 3 == 0 || !autoSetControlPoints)
        {
            points[index] = position;
            isDirty = true;

            if (autoSetControlPoints)
            {
                AutoSetAllAffectdControlPoints(index);
            }
            else
            {
                if (index % 3 == 0)
                {
                    MoveAdjacentControlPoints(index, deltaPosition);
                }
                else
                {
                    MoveCorrespondingAnchorPoint(index, position);
                }
            }
        }
    }

    /// <summary>
    /// Moves the control points adjance to the anchor point whose index is given.
    /// </summary>
    /// <param name="index">The index of the moved anchor point.</param>
    /// <param name="displacement">The displacement of the control point.</param>
    private void MoveAdjacentControlPoints(int index, Vector3 displacement)
    {
        if (index - 1 >= 0 || isClosed)
        {
            points[LoopIndex(index - 1)] += displacement;
        }
        if (index + 1 < NumPoints || isClosed)
        {
            points[LoopIndex(index + 1)] += displacement;
        }
    }

    /// <summary>
    /// Moves the anchor point corresponding to control whose index is given.
    /// </summary>
    /// <param name="index">The index of the moved control point.</param>
    /// <param name="position">The new position of the control point.</param>
    private void MoveCorrespondingAnchorPoint(int index, Vector3 position)
    {
        bool nextIsAnchor = (index + 1) % 3 == 0;
        int correspondingControlIndex = nextIsAnchor ? index + 2 : index - 1;
        int anchorIndex = nextIsAnchor ? index + 1 : index - 1;

        if (correspondingControlIndex >= 0 && correspondingControlIndex < NumPoints || isClosed)
        {
            float distance = Vector3.Distance(points[LoopIndex(anchorIndex)], points[LoopIndex(correspondingControlIndex)]);
            Vector3 direction = (points[LoopIndex(anchorIndex)] - position).normalized;
            points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + direction * distance;
        }
    }

    /// <summary>
    /// Toggles whether the path is closed or not. If the path is closed by the toggling, then
    /// control points are immediatly updated.
    /// </summary>
    public void ToggleClosed()
    {
        isClosed = !isClosed;
        isDirty = true;

        if (isClosed)
        {
            // Adds two control points
            points.Add(points[NumPoints - 1] * 2 - points[NumPoints - 2]);
            points.Add((points[0] * 2 - points[1]) * 0.5f);
            if (autoSetControlPoints)
            {
                AutoSetAnchorControlPoint(0);
                AutoSetAnchorControlPoint(NumPoints - 3);
            }
        }
        else
        {
            // Removes the two extra control points
            points.RemoveRange(NumPoints - 2, 2);
            if (autoSetControlPoints)
            {
                AutoSetStartAndEndControls();
            }
        }
    }

    /// <summary>
    /// Partly updated the control points. The ones updated are those of anchor immediatly before and after 
    /// the update anchor point.
    /// </summary>
    /// <param name="updatedAnchorIndex">Index of the moved anchor point</param>
    private void AutoSetAllAffectdControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i < updatedAnchorIndex + 3; i += 3)
        {
            if (i >= 0 && i < NumPoints || isClosed)
            {
                AutoSetAnchorControlPoint(LoopIndex(i));
            }
        }
        AutoSetStartAndEndControls();
    }

    /// <summary>
    /// Calculate control points automatically for all anchor points
    /// </summary>
    private void AutoSetAllControlPoints()
    {
        for (int i = 0; i < NumPoints; i += 3)
        {
            AutoSetAnchorControlPoint(i);
        }
        AutoSetStartAndEndControls();
    }

    /// <summary>
    /// Calculate the control points for an anchor point based on the position
    /// of the previous and the next anchor point.
    /// </summary>
    /// <param name="anchorIndex">The index of the anchor point for which to calculate the control points.</param>
    private void AutoSetAnchorControlPoint(int anchorIndex)
    {
        Vector3 anchorPosition = points[anchorIndex];
        Vector3 direction = Vector3.zero;
        float[] neighboursDistance = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector3 offset = points[LoopIndex(anchorIndex - 3)] - anchorPosition;
            direction += offset.normalized;
            neighboursDistance[0] = offset.magnitude;
        }
        if (anchorIndex + 3 < NumPoints || isClosed)
        {
            Vector3 offset = points[LoopIndex(anchorIndex + 3)] - anchorPosition;
            direction -= offset.normalized;
            neighboursDistance[1] = -1.0f * offset.magnitude;
        }

        direction.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPosition + direction * neighboursDistance[i] * 0.5f;
            }
        }

        isDirty = true;
    }

    /// <summary>
    /// If the path is closed, calculates the first and last control points accordingly 
    /// </summary>
    private void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) * 0.5f;
            points[NumPoints - 2] = (points[NumPoints - 1] + points[NumPoints - 3]) * 0.5f;
            isDirty = true;
        }
    }

    /// <summary>
    /// Return the index of the point in a way that the last anchor point connects to the first if the path is closed.
    /// A negative index (more than the negative number of points), go back in the list of points (e.g. -1 gives the control point
    /// before the first anchor).     
    /// </summary>
    /// <param name="index">The index of the point. If their is n points, than the index must be at least -n.</param>
    /// <returns>The correct index to loop in the path.</returns>
    private int LoopIndex(int index)
    {
        return (index + NumPoints) % NumPoints;
    }
}
