﻿using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class HermiteCurve : MonoBehaviour
{
    public GameObject start, startTangentPoint, end, endTangentPoint;

    public Color color = Color.white;
    public float width = 0.1f;
    public int numberOfPoints = 50;
    LineRenderer lineRenderer;
    float totalArcLenght;
    Vector3 previousPoint;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        totalArcLenght = 0;
        //curveLenght = Vector3.zero;
        DrawHermiteCurve();

    }

    void Update()
    {

    }

    void DrawHermiteCurve()
    {
        // check parameters and components
        if (null == lineRenderer || null == start || null == startTangentPoint
           || null == end || null == endTangentPoint)
        {
            return; // no points specified
        }

        // update line renderer
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        if (numberOfPoints > 0)
        {
            lineRenderer.positionCount = numberOfPoints;
        }

        // set points of Hermite curve
        Vector3 p0 = start.transform.position;
        Vector3 p1 = end.transform.position;
        Vector3 m0 = startTangentPoint.transform.position - start.transform.position;
        Vector3 m1 = endTangentPoint.transform.position - end.transform.position;
        float t;
        Vector3 position;

        for (int i = 0; i < numberOfPoints; i++)
        {
            t = i / (numberOfPoints - 1.0f);
            position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * p0
            + (t * t * t - 2.0f * t * t + t) * m0
            + (-2.0f * t * t * t + 3.0f * t * t) * p1
            + (t * t * t - t * t) * m1;
            //    Debug.Log(position);
            lineRenderer.SetPosition(i, position);

            if (i > 0)
            {
                totalArcLenght = totalArcLenght + Vector3.Magnitude(position - previousPoint);
                

            }
            previousPoint = position;
            
        }

        Debug.Log(totalArcLenght);
    }
    

        
}

