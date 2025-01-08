using System;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    // Entrada del jugador
    public float steerInput, accelInput;
    public float steerAngle, brakeForceValue;
    public bool isBraking;

    // Configuración del coche
    [SerializeField] private float enginePower, brakingPower, maxTurnAngle;
    [SerializeField] private float topSpeed = 75f;

    // Ruedas (Colliders)
    [SerializeField] private WheelCollider frontLeftCollider, frontRightCollider;
    [SerializeField] private WheelCollider rearLeftCollider, rearRightCollider;

    // Ruedas (Transformaciones)
    [SerializeField] private Transform frontLeftMesh, frontRightMesh;
    [SerializeField] private Transform rearLeftMesh, rearRightMesh;

    // Centro de masa
    [SerializeField] private Vector3 massCenterOffset;
    private Rigidbody rb;

    // Inicialización
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += massCenterOffset;
    }

    private void FixedUpdate()
    {
        ProcessInput();
        ControlAcceleration();
        ControlSteering();
        UpdateWheelMeshes();
    }

    // Procesar Entrada del Jugador
    private void ProcessInput()
    {
        steerInput = Input.GetAxis("Horizontal");
        accelInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    // Controlar Aceleración y Frenado
    private void ControlAcceleration()
    {
        if (rb.velocity.magnitude < topSpeed)
        {
            frontLeftCollider.motorTorque = accelInput * enginePower;
            frontRightCollider.motorTorque = accelInput * enginePower;
        }
        else
        {
            frontLeftCollider.motorTorque = 0f;
            frontRightCollider.motorTorque = 0f;
        }

        brakeForceValue = isBraking ? brakingPower : 0f;
        ApplyBrakes();
    }

    // Aplicar Frenado
    private void ApplyBrakes()
    {
        frontRightCollider.brakeTorque = brakeForceValue;
        frontLeftCollider.brakeTorque = brakeForceValue;
        rearLeftCollider.brakeTorque = brakeForceValue;
        rearRightCollider.brakeTorque = brakeForceValue;
    }

    // Controlar Dirección
    private void ControlSteering()
    {
        steerAngle = maxTurnAngle * steerInput;
        frontLeftCollider.steerAngle = steerAngle;
        frontRightCollider.steerAngle = steerAngle;
    }

    // Actualizar las Mallas de las Ruedas
    private void UpdateWheelMeshes()
    {
        UpdateWheelPose(frontLeftCollider, frontLeftMesh);
        UpdateWheelPose(frontRightCollider, frontRightMesh);
        UpdateWheelPose(rearRightCollider, rearRightMesh);
        UpdateWheelPose(rearLeftCollider, rearLeftMesh);
    }

    // 🟡 Actualizar una Rueda Individual
    private void UpdateWheelPose(WheelCollider wheelCol, Transform wheelMesh)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCol.GetWorldPose(out pos, out rot);
        wheelMesh.rotation = rot;
        wheelMesh.position = pos;
    }
}
