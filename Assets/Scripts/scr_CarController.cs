using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CarController : MonoBehaviour
{
    public scr_Wheel[] wheels;

    [Header("Car Specs")]
    public float WheelBase; // in m
    public float rearTrack; // in m
    public float turningRadius; // in m
    public Transform centreOfMass;

    [Header("Inputs")]
    public float steerInput;

    public float ackermannAngleLeft;
    public float ackermannAngleRight;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();        
        rb.centerOfMass = centreOfMass.transform.localPosition;
    }

    void Update()
    {
        steerInput = Input.GetAxis("Horizontal");

        if (steerInput > 0) // turning right 
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(WheelBase / (turningRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(WheelBase / (turningRadius - (rearTrack / 2))) * steerInput;
        }
        else if (steerInput < 0) // turning left
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(WheelBase / (turningRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(WheelBase / (turningRadius + (rearTrack / 2))) * steerInput;
        }
        else // not turning
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        foreach (scr_Wheel wheel in wheels)
        {
            if(wheel.wheelFL)
                wheel.steerAngle = ackermannAngleLeft;
            if(wheel.wheelFR)
                wheel.steerAngle = ackermannAngleRight;
        }
    }
}
