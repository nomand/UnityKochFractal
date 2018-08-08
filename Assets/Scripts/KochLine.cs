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


    [Tooltip("Download https://github.com/nomand/UnityAudioVisualization")]
    [Header("Audio")]
    public bool UseAudio;
    public Runningtap.AnalyzeAudio audioRead;
    public int[] audioBand;

    private float[] lerpAudio;

    void Start ()
    {
        lerpAudio = new float[initiatorPointAmount];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = position.Length;
        lineRenderer.SetPositions(position);
        lerpPosition = new Vector3[position.Length];
    }
	
	void Update ()
    {
        if(generationCount != 0)
        {
            int count = 0;
            for(int i = 0; i < initiatorPointAmount; i++)
            {
                lerpAudio[i] = audioRead.AudioBandBuffer[audioBand[i]];

                for (int j = 0; j < (position.Length -1) / initiatorPointAmount; j++)
                {
                    lerpPosition[count] = Vector3.Lerp(position[count], targetPosition[count], UseAudio ? lerpAudio[i] : LerpAmount);
                    count++;
                }
            }

            lerpPosition[count] = Vector3.Lerp(position[count], targetPosition[count], UseAudio ? lerpAudio[initiatorPointAmount-1] : LerpAmount);

            if (useBezierCurves)
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
    }
}