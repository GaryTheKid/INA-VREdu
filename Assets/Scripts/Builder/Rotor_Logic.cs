using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Rotor_Logic : MonoBehaviour {
    
    Rigidbody rig;

    public float force; //The force we are going to apply

    public Transform visualModel; //The gameobject we are going to rotate to make the player think that the wheel/rotor is moving

    //Because we don't want to add a new axis for every button
    //We simply take to keys (that we can assign from the inspector)
    public KeyCode positiveKey;
    public KeyCode negativeKey;
    public Vector3 forceVector;
    public Transform forceOrigin;
    public Transform cubeTransform;

    XRIDefaultInputActions input;
    bool isFiring;
    float targetFloat;
    float curFloat;

    void Start()
    {
        forceOrigin = transform.GetChild(0).transform;
        cubeTransform = transform.GetChild(1).transform;
        rig = GetComponent<Rigidbody>();
        input = Game_Manager.singleton.inputActions;
        input.XRIRightHand.FireRotors.performed += ToggleRotor;
    }


    void FixedUpdate()
    {
        SimulateAxis(); //Simulate the axis

        //rotate the visual model based on our custom axis
        visualModel.Rotate(0, curFloat * 10, 0);

        //add force to the rigidbody based on our custom axis

        forceVector = (cubeTransform.position - forceOrigin.position).normalized;

        rig.AddForce(forceVector * force * curFloat);
    }

    void SimulateAxis()
    {
        if (isFiring)
        {
            //Then the target of our axis is 1
            targetFloat = 1;
        }
        else
        {
            targetFloat = 0;
        }

        //Interpolate the curfloat to the target float
        curFloat = Mathf.MoveTowards(curFloat, targetFloat, Time.deltaTime * 5);

        if (isFiring)
            isFiring = false;
    }

    void ToggleRotor(InputAction.CallbackContext context)
    {

        print(123);

        if (!isFiring)
            isFiring = true;
    }
}
