using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochGenerator : MonoBehaviour {

    protected enum _axis
    {
        XAxis,
        YAxis,
        ZAxis
    };

    protected enum _initiator
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon
    };

    public struct LineSegment
    {
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 Direction { get; set; }
        public float Length { get; set; }
    }

    [SerializeField]
    protected _initiator initiator = new _initiator();

    [SerializeField]
    protected AnimationCurve generator;
    protected Keyframe[] keys;

    [System.Serializable]
    public struct StartGen
    {
        public bool outwards;
        public float scale;
    }

    public StartGen[] startGen;

    protected int generationCount;

    [SerializeField]
    protected _axis axis = new _axis();

    protected int initiatorPointAmount;
    private Vector3[] initiatorPoint;
    private Vector3 rotateVector;
    private float initialRotation;
    [SerializeField]
    protected float initiatorSize;
    private Vector3 rotateAxis;

    protected Vector3[] position;
    protected Vector3[] targetPosition;
    protected Vector3[] bezierPosition;
    private List<LineSegment> lineSegment;

    [SerializeField]
    protected bool useBezierCurves;
    [SerializeField]
    [Range(8,24)]
    protected int bezierVertexCount;

    protected Vector3[] BezierCurve(Vector3[] points, int vertexCount)
    {
        var pointList = new List<Vector3>();
        for (int i = 0; i < points.Length; i += 2)
        {
            if(i+2 <= points.Length - 1)
            {
                for(float ratio = 0f; ratio <= 1f; ratio += 1.0f / vertexCount)
                {
                    var tangentLineVertex1 = Vector3.Lerp(points[i], points[i + 1], ratio);
                    var tangentLineVertex2 = Vector3.Lerp(points[i+1], points[i + 2], ratio);
                    var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                    pointList.Add(bezierPoint);
                }
            }
        }

        return pointList.ToArray();
    }

    private void Awake()
    {
        GetInitiatorPoins();

        position = new Vector3[initiatorPointAmount + 1];
        targetPosition = new Vector3[initiatorPointAmount + 1];
        keys = generator.keys;

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            position[i] = rotateVector * initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }
        position[initiatorPointAmount] = position[0];
        targetPosition = position;

        for (int i = 0; i < startGen.Length; i++)
        {
            GenerateKoch(targetPosition, startGen[i].outwards, startGen[i].scale);
        }
    }

    protected void GenerateKoch(Vector3[] positions, bool outwards, float generatorMultiplier)
    {
        lineSegment = new List<LineSegment>();
        lineSegment.Clear();
        for (int i = 0; i < positions.Length - 1; i++)
        {
            LineSegment line = new LineSegment();
            line.StartPosition = positions[i];
            if(i==positions.Length-1)
                line.EndPosition = positions[0];
            else
                line.EndPosition = positions[i + 1];

            line.Direction = (line.EndPosition - line.StartPosition).normalized;
            line.Length = Vector3.Distance(line.EndPosition, line.StartPosition);
            lineSegment.Add(line);
        }

        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();

        for (int i = 0; i < lineSegment.Count; i++)
        {
            newPos.Add(lineSegment[i].StartPosition);
            targetPos.Add(lineSegment[i].StartPosition);

            for (int j = 1; j < keys.Length - 1; j++)
            {
                float moveAmount = lineSegment[i].Length * keys[j].time;
                float heightAmount = (lineSegment[i].Length * keys[j].value) * generatorMultiplier;
                Vector3 movePos = lineSegment[i].StartPosition + (lineSegment[i].Direction * moveAmount);
                Vector3 direction;
                if(outwards)
                    direction = Quaternion.AngleAxis(-90, rotateAxis) * lineSegment[i].Direction;
                else
                    direction = Quaternion.AngleAxis(90, rotateAxis) * lineSegment[i].Direction;
                newPos.Add(movePos);
                targetPos.Add(movePos + (direction * heightAmount));
            }
        }

        newPos.Add(lineSegment[0].StartPosition);
        targetPos.Add(lineSegment[0].StartPosition);

        position = new Vector3[newPos.Count];
        targetPosition = new Vector3[targetPos.Count];
        position = newPos.ToArray();
        targetPosition = targetPos.ToArray();
        bezierPosition = BezierCurve(targetPosition, bezierVertexCount);

        generationCount++;
    }

    private void GetInitiatorPoins()
    {
        switch(initiator)
        {
            case _initiator.Triangle:
                initiatorPointAmount = 3;
                initialRotation = 0;
                break;
            case _initiator.Square:
                initiatorPointAmount = 4;
                initialRotation = 45;
                break;
            case _initiator.Pentagon:
                initiatorPointAmount = 5;
                initialRotation = 36;
                break;
            case _initiator.Hexagon:
                initiatorPointAmount = 6;
                initialRotation = 30;
                break;
            case _initiator.Heptagon:
                initiatorPointAmount = 7;
                initialRotation = 25.71428f;
                break;
            case _initiator.Octagon:
                initiatorPointAmount = 8;
                initialRotation = 22.5f;
                break;
            default:
                initiatorPointAmount = 3;
                initialRotation = 0;
                break;
        }

        switch(axis)
        {
            case _axis.XAxis:
            rotateVector = new Vector3(1, 0, 0);
            rotateAxis = new Vector3(0, 0, 1);
                break;
            case _axis.YAxis:
            rotateVector = new Vector3(0, 1, 0);
            rotateAxis = new Vector3(1, 0, 0);
                break;
            case _axis.ZAxis:
            rotateVector = new Vector3(0, 0, 1);
            rotateAxis = new Vector3(0, 1, 0);
                break;
            default:
            rotateVector = new Vector3(0, 1, 0);
            rotateAxis = new Vector3(1, 0, 0);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        GetInitiatorPoins();

        initiatorPoint = new Vector3[initiatorPointAmount];
        lineSegment = new List<LineSegment>();

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            initiatorPoint[i] = rotateVector * initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            Gizmos.color = Color.white;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            if (i < initiatorPointAmount - 1)
            {
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[0]);
            }
        }
    }
}
