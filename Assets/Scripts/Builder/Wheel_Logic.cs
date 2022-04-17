using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Wheel_Logic : MonoBehaviour {

    //See rotor logic script first for the uncommented parts

    private WheelCollider wheelCollider;
    private Rigidbody wheelRigid;

    public Transform visualModel;
    public float force;

    XRIDefaultInputActions input;
    float curFloat;

	void Start () 
    {
        wheelCollider = GetComponent<WheelCollider>();
        wheelRigid = GetComponent<Rigidbody>();
        input = Game_Manager.singleton.inputActions;
    }
	
	void LateUpdate () 
    {
        SimulateAxis();
        visualModel.Rotate(-wheelRigid.angularVelocity.y * 5, 0, 0);

        //apply the motor torque to the wheel collider
        wheelCollider.motorTorque = force * curFloat * Time.deltaTime;
    }

    void SimulateAxis()
    {
        curFloat = input.XRIRightHand.Move.ReadValue<Vector2>().y;
    }

  
}
