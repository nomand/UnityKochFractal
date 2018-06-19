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

    [SerializeField]
    protected _initiator initiator = new _initiator();
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

    private void OnDrawGizmos()
    {
        GetInitiatorPoins();
        initiatorPoint = new Vector3[initiatorPointAmount];

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
            if(i < initiatorPointAmount - 1)
            {
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(initiatorPoint[i], initiatorPoint[0]);
            }
        }
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

    private void Awake()
    {
        GetInitiatorPoins();
        position = new Vector3[initiatorPointAmount + 1];

        rotateVector = Quaternion.AngleAxis(initialRotation, rotateAxis) * rotateVector;

        for (int i = 0; i < initiatorPointAmount; i++)
        {
            position[i] = rotateVector * initiatorSize;
            rotateVector = Quaternion.AngleAxis(360 / initiatorPointAmount, rotateAxis) * rotateVector;
        }
        position[initiatorPointAmount] = position[0];
    }

    void Start ()
    {
		
	}
	
	void Update () {
		
	}
}
