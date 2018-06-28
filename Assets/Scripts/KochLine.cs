using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator
{
    LineRenderer lineRenderer;
    [Range(0,1)]
    public float LerpAmount;
    Vector3[] lerpPosition;
    public float generateMultiplier;

	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
	}
	
	void Update ()
    {
        if(generationCount != 0)
        {
            for(int i = 0; i < position.Length; i++)
            {
                lerpPosition[i] = Vector3.Lerp(position[i], targetPosition[i], LerpAmount);
            }
            if(useBezierCurves)
            {
                bezierPosition = BezierCurve(lerpPosition, bezierVertexCount);
                lineRenderer.positionCount = bezierPosition.Length;
                lineRenderer.SetPositions(bezierPosition);
            }
            else
            {
                lineRenderer.positionCount = lerpPosition.Length;
                lineRenderer.SetPositions(lerpPosition);
            }
        }

		if(Input.GetKeyDown(KeyCode.O))
        {
            GenerateKoch(targetPosition, true, generateMultiplier);
            lerpPosition = new Vector3[position.Length];
            lineRenderer.positionCount = position.Length;
            lineRenderer.SetPositions(position);
            LerpAmount = 0;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GenerateKoch(targetPosition, false, generateMultiplier);
            lerpPosition = new Vector3[position.Length];
            lineRenderer.positionCount = position.Length;
            lineRenderer.SetPositions(position);
            LerpAmount = 0;
        }
    }
}