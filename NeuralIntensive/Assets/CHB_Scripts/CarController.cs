using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    #region Variables
    public Rigidbody rb;
    public Transform centerOfMass;

    public Transform wheelFrontLeftModel, wheelFrontRightModel, wheelRearLeftModel, wheelRearRightModel;
    public WheelCollider wheelFrontLeftCollider, wheelFrontRightCollider, wheelRearLeftCollider, wheelRearRightCollider;

    public float horizontalInput;
    public float verticalInput;

    public float maxSteerAngle = 42;
    public float motorForce = 500;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass = centerOfMass.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    void Steer()
    {
        wheelFrontLeftCollider.steerAngle = horizontalInput * maxSteerAngle;
        wheelFrontRightCollider.steerAngle = horizontalInput * maxSteerAngle;
    }

    void Accelerate()
    {
        wheelRearLeftCollider.motorTorque = verticalInput * motorForce;
        wheelRearRightCollider.motorTorque = verticalInput * motorForce;
    }

    void UpdateWheelPoses()
    {
        UpdateWheelPose(wheelFrontLeftCollider, wheelFrontLeftModel);
        UpdateWheelPose(wheelFrontRightCollider, wheelRearRightModel);
        UpdateWheelPose(wheelRearLeftCollider, wheelRearLeftModel);
        UpdateWheelPose(wheelRearRightCollider, wheelRearRightModel);
    }

    Vector3 pos;
    Quaternion quat;
    void UpdateWheelPose(WheelCollider _wCol, Transform _tr)
    {
        pos = _tr.position;
        quat = _tr.rotation;

        _wCol.GetWorldPose(out pos, out quat);

        _tr.position = pos;
        _tr.rotation = quat;
    }

    public void Reset()
    {
        horizontalInput = 0;
        verticalInput = 0;
    }
}
