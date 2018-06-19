using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KochLine : KochGenerator
{
    LineRenderer lineRenderer;

	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
	}
	
	void Update ()
    {
		
	}
}
