using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class scr_Wheel : MonoBehaviour
{
    [Header("Wheels")]
    public float wheelRadius;
    public float steerAngle;
    public bool wheelFL;
    public bool wheelFR;
    public bool wheelBL;
    public bool wheelBR;

    [Header("Suspension Settings")]
    public float suspensionStiffness;
    public float suspensionRestLength;
    public float maxSpringTravel;
    public float damperStiffness;

    private float minSuspensionLength;
    private float maxSuspensionLength;
    private float suspensionLength;
    private float lastSuspensionLength;
    private float springVelocity;

    private Vector3 damperForce;
    private Vector3 suspensionForce;

    Rigidbody rb;
    RaycastHit hit;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        minSuspensionLength = wheelRadius - maxSpringTravel;
        maxSuspensionLength = wheelRadius + maxSpringTravel;
    }

    void Update() 
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + steerAngle, transform.localRotation.z);
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, -transform.up * suspensionLength, Color.green);
        Debug.DrawRay(transform.position, -transform.up * maxSuspensionLength, Color.red);
        Debug.DrawRay(transform.position, -transform.up * minSuspensionLength, Color.blue);

        if (Physics.Raycast(transform.position, -transform.up, out hit, maxSuspensionLength + wheelRadius))
        {
            lastSuspensionLength = suspensionLength;
            suspensionLength = hit.distance - wheelRadius;
            suspensionLength = Mathf.Clamp(suspensionLength, minSuspensionLength, maxSuspensionLength);

            springVelocity = (lastSuspensionLength - suspensionLength) / Time.fixedDeltaTime;
            damperForce = damperStiffness * springVelocity * transform.up;

            suspensionForce = suspensionStiffness * (suspensionRestLength - suspensionLength) * transform.up;

            rb.AddForceAtPosition(suspensionForce + damperForce, hit.point);
        }
    }
}
