using System.Collections;
using UnityEngine;

public class DoorRotationLite : MonoBehaviour
{
    public float InitialAngle = 0.0F;
    public float RotationAngle = 90.0F;
    public enum SideOfRotation { Left, Right }
    public SideOfRotation RotationSide;
    [StayPositive]
    public float Speed = 3F;
    public int TimesMoveable = 0;

    private GameObject hinge;
    public enum PositionOfHinge { Left, Right }
    public PositionOfHinge HingePosition;

    public enum ScaleOfDoor { Unity3DUnits, Other }
    public ScaleOfDoor DoorScale;
    public enum PositionOfPivot { Centered, CorrectlyPositioned }
    public PositionOfPivot PivotPosition;

    Quaternion FinalRot, InitialRot;
    int State;
    int TimesRotated = 0;
    [HideInInspector] public bool RotationPending = false;

    public bool VisualizeHinge = false;
    public Color HingeColor = Color.yellow;

    Quaternion RotationOffset;

    public float DetectionRadius = 3f;
    private Collider[] hitColliders = new Collider[10];

    void Start()
    {
        gameObject.tag = "Door";
        RotationOffset = transform.rotation;

        SetInitialFinalRotations();

        if (DoorScale != ScaleOfDoor.Unity3DUnits || PivotPosition != PositionOfPivot.Centered) return;

        hinge = new GameObject("hinge");
        float cosDeg = Mathf.Cos((transform.eulerAngles.y * Mathf.PI) / 180);
        float sinDeg = Mathf.Sin((transform.eulerAngles.y * Mathf.PI) / 180);

        Vector3 HingePosCopy = hinge.transform.position;
        Vector3 HingeRotCopy = hinge.transform.localEulerAngles;

        if (HingePosition == PositionOfHinge.Left)
        {
            if (transform.localScale.x > transform.localScale.z)
            {
                HingePosCopy.x = (transform.position.x - (transform.localScale.x / 2 * cosDeg));
                HingePosCopy.z = (transform.position.z + (transform.localScale.x / 2 * sinDeg));
            }
            else
            {
                HingePosCopy.x = (transform.position.x + (transform.localScale.z / 2 * sinDeg));
                HingePosCopy.z = (transform.position.z + (transform.localScale.z / 2 * cosDeg));
            }
        }
        else if (HingePosition == PositionOfHinge.Right)
        {
            if (transform.localScale.x > transform.localScale.z)
            {
                HingePosCopy.x = (transform.position.x + (transform.localScale.x / 2 * cosDeg));
                HingePosCopy.z = (transform.position.z - (transform.localScale.x / 2 * sinDeg));
            }
            else
            {
                HingePosCopy.x = (transform.position.x - (transform.localScale.z / 2 * sinDeg));
                HingePosCopy.z = (transform.position.z - (transform.localScale.z / 2 * cosDeg));
            }
        }

        hinge.transform.position = HingePosCopy;
        transform.parent = hinge.transform;
        hinge.transform.localEulerAngles = HingeRotCopy;

        if (VisualizeHinge)
        {
            #if UNITY_EDITOR
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = HingePosCopy;
            cube.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            cube.GetComponent<Renderer>().material.color = HingeColor;
            #endif
        }
    }

    void Update()
    {
        if (RotationPending) return;

        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, DetectionRadius, hitColliders);
        for (int i = 0; i < hitCount; i++)
        {
            if (hitColliders[i].CompareTag("NPC"))
            {
                StartCoroutine(Move());
                break;
            }
        }
    }

    void SetInitialFinalRotations()
    {
        if (RotationSide == SideOfRotation.Left)
        {
            InitialRot = Quaternion.Euler(0, -InitialAngle, 0);
            FinalRot = Quaternion.Euler(0, -InitialAngle - RotationAngle, 0);
        }
        else if (RotationSide == SideOfRotation.Right)
        {
            InitialRot = Quaternion.Euler(0, -InitialAngle, 0);
            FinalRot = Quaternion.Euler(0, -InitialAngle + RotationAngle, 0);
        }
    }

    public IEnumerator Move()
    {
        RotationPending = true;
        AnimationCurve rotationcurve = AnimationCurve.EaseInOut(0, 0, 1f, 1f);
        float TimeProgression = 0f;

        Transform t = (DoorScale == ScaleOfDoor.Unity3DUnits && PivotPosition == PositionOfPivot.Centered) ? hinge.transform : transform;

        if (TimesRotated < TimesMoveable || TimesMoveable == 0)
        {
            if (t.rotation == (State == 0 ? FinalRot * RotationOffset : InitialRot * RotationOffset)) State ^= 1;

            Quaternion FinalRotation = (State == 0 ? FinalRot * RotationOffset : InitialRot * RotationOffset);

            while (TimeProgression <= (1 / Speed))
            {
                TimeProgression += Time.deltaTime;
                float RotationProgression = Mathf.Clamp01(TimeProgression / (1 / Speed));
                float RotationCurveValue = rotationcurve.Evaluate(RotationProgression);

                t.rotation = Quaternion.Lerp(t.rotation, FinalRotation, RotationCurveValue);

                yield return null;
            }

            TimesRotated = TimesMoveable == 0 ? 0 : TimesRotated + 1;
        }

        RotationPending = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
    }
}
