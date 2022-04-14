using UnityEngine;
using System.Collections;

public class CopyLogicFromParent : MonoBehaviour {

    Rigidbody ourRig;
    Rigidbody parentRig;

	void Start () {

        ourRig = GetComponent<Rigidbody>();
        
        parentRig = transform.parent.GetComponent<Rigidbody>();

	}
	
	void Update()
    {
        ourRig.isKinematic = parentRig.isKinematic;
    }
}
