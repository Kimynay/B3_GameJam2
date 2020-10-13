using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BezierCurve : MonoBehaviour
{
    public Transform startingPoint;
    public Transform startingTangent;
    public Transform endingTangent;
    public Transform endingPoint;

    public bool DebugCurve = true;

    #region Properties
    public bool AreReferencesFilled
    {
        get
        {
            return startingPoint != null && startingTangent != null && endingTangent != null && endingPoint != null;
        }
        set
        {

        }
    }
    public bool HasChanged
    {
        get
        {
            return startingPoint.hasChanged || startingTangent.hasChanged || endingTangent.hasChanged || endingPoint.hasChanged;
        }
    }
    #endregion
    public Vector3 GetPosition(float ratio)
    {
        if (!AreReferencesFilled) // make a early return if all the references are not set
        {
            Debug.LogError("ATTENTION : All the references needed to Compute the curve are not set on " + gameObject.name, this);
            return Vector3.zero;
        }

        ratio = Mathf.Clamp01(ratio); // s'assure que le ratio ne dépasse pas 0 et 1

        Vector3 lerpStartPtAndStartTg = Vector3.Lerp(startingPoint.position, startingTangent.position, ratio);
        Vector3 lerpStartTgAndEndTg = Vector3.Lerp(startingTangent.position, endingTangent.position, ratio);
        Vector3 lerpEndTgAndEndPt = Vector3.Lerp(endingTangent.position, endingPoint.position, ratio);

        Vector3 entryCurve = Vector3.Lerp(lerpStartPtAndStartTg, lerpStartTgAndEndTg, ratio);
        Vector3 exitCurve = Vector3.Lerp(lerpStartTgAndEndTg, lerpEndTgAndEndPt, ratio);

        Vector3 bezierCurve = Vector3.Lerp(entryCurve, exitCurve, ratio);

        Vector3 position = bezierCurve;
        Quaternion rotation = GetRotation(ratio);
        Vector3 scale = Vector3.one;
        Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);

        Vector3 origin = Vector3.zero; // center of our matrix is the local position 0,0,0
        return matrix.MultiplyPoint(origin);
    }
    public Vector3 GetVelocity(float ratio)
    {
        if (!AreReferencesFilled) // make a early return if all the references are not set
        {
            Debug.LogError("ATTENTION : All the references needed to Compute the curve are not set on " + gameObject.name, this);
            return Vector3.zero;
        }

        ratio = Mathf.Clamp01(ratio); // s'assure que le ratio ne dépasse pas 0 et 1

        float inverseRatio = 1.0f - ratio;
        Vector3 velocity = 3.0f * inverseRatio * inverseRatio * (startingTangent.position - startingPoint.position)
            + 6.0f * inverseRatio * ratio * (endingTangent.position - startingTangent.position)
            + 3.0f * ratio * ratio * (endingPoint.position - endingTangent.position);

        return velocity;
    }
    public Quaternion GetRotation(float ratio)
    {
        Vector3 velocity = GetVelocity(ratio);
        Vector3 orientation = velocity.normalized;
        Quaternion rotation = Quaternion.LookRotation(orientation);

        return rotation;
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (AreReferencesFilled)
        {
            Handles.DrawBezier(startingPoint.position, endingPoint.position, startingTangent.position, endingTangent.position, Color.cyan, null, 2);
        }
    }   
    #endif
}