using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class Game_Manager : MonoBehaviour {

    public static Game_Manager singleton;

    private bool playMode; //If it's play mode, then it's not build mode :P

    //A list that hold every part we have attached to our Siege equipment(?)
    private List<Transform> PlayerParts = new List<Transform>();

    [SerializeField] private Transform baseCube; //The starter cube
    private Vector3 initPos;
    private Quaternion initRot;

    [SerializeField] private GameObject partPrefab; //The prefab we want to instantiate
    private GameObject part; //The actual instantiated game object
    private Transform socket; //The socket we are going to place it to
    private Vector3 placePos; //Where we are going to place it
    private bool isReadyForInstantiate;

    // user input
    [SerializeField] private Transform rightHandTransform;
    private Vector3 rightHandPos;
    public XRIDefaultInputActions inputActions;

    // tutorial completion markers
    [SerializeField] private List<Collider> markers;
    [SerializeField] private MeshRenderer completionMarker;
    [SerializeField] private Timer timer;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        inputActions = new XRIDefaultInputActions();
        inputActions.XRIRightHand.InstantiatePart.Enable();
        inputActions.XRIRightHand.Move.Enable();
        inputActions.XRIRightHand.FireRotors.Enable();
    }

    private void Start()
    {
        PlayerParts.Add(baseCube);
        initPos = baseCube.position;
        initRot = baseCube.rotation;
        inputActions.XRIRightHand.InstantiatePart.performed += InstantiatePart;
    }

    void Update()
    {
        if (!playMode)
        {
            //This is basically the build mode
            rightHandPos = rightHandTransform.position;
            ReadyInstantiatePart();
        }
        else
        { //If we are at play mode
            //and we had a part we where suppose to place
            if (part)
            {
                //destroy it
                Destroy(part);
                part = null;
            }
        }
    }

    void ReadyInstantiatePart()
    {
        if (!part) //If we don't have a part to place
        {
            if (partPrefab) //but we have a prefab for it
            {
                //instantiate the part on a position way out of sight from the camera
                part = Instantiate(partPrefab, -Vector3.up * 2000, Quaternion.identity) as GameObject;
            }
            //if we don't have a prefab, then the player is either sleeping or haven't decided what to place
            isReadyForInstantiate = false;
        }
        else
        {
            //If we have a part to place
            //Make a raycast
            RaycastHit hit;

            if (Physics.Raycast(rightHandPos, rightHandTransform.forward, out hit, Mathf.Infinity))
            {
                //Check what we hit
                CheckHit(hit);
            }

            //Update the part's position
            part.transform.position = placePos;

            //And if we click
            if (socket != null)
            {
                isReadyForInstantiate = true;
            }
        }
    }

    void InstantiatePart(InputAction.CallbackContext context)
    {
        if (isReadyForInstantiate)
        {
            //...store the Siege part base of the "part to place"
            SiegePart_base placeBase = part.GetComponent<SiegePart_base>();

            //add it to the list
            PlayerParts.Add(part.transform);
            //enable it's collider
            part.GetComponentInChildren<Collider>().enabled = true;

            //assign a target to the joint
            placeBase.AssignTargetToJoint(socket.parent);

            //disable the socket
            placeBase.DisableSocket(socket);

            //update the position once more
            part.transform.position = placePos;

            part = null;

            isReadyForInstantiate = false;
        }
    }

    void CheckHit(RaycastHit hit)
    {
        //If we hit a gameobject that has the SiegePart_base script then we hit a part of our siege engine
        if(hit.transform.GetComponent<SiegePart_base>())
        {
            //Store it
            SiegePart_base partBase = hit.transform.GetComponent<SiegePart_base>();
                      
            //Find the closest socket to our hit.point
            socket = partBase.ReturnClosestDirection(hit.point);

            //if we have a socket, then update the place pos
            if (socket) //The socket might return null if the part doesn't have any sockets, this avoids errors from that
            {
                placePos = socket.position;

                //We want to look at the center of the other parts mesh
                //Since the transform.position gives the pivot of a gameobject,
                //it might not always be the correct position we want our part to look
                //thus we simply find the center of that other parts mesh

                Vector3 dir = partBase.rendererToFindEdges.bounds.center - socket.position;

                dir.Normalize();

                if (dir == Vector3.zero)
                    dir = -socket.forward;

                Quaternion rot = Quaternion.LookRotation(dir);
                part.transform.rotation = rot;
                //partToPlace.transform.LookAt(partBase.rendererToFindEdges.bounds.center);  
            }
        }
        else
        {
            //If we din't hit an object, hide the part then
            placePos = new Vector3(0, -2000, 0);
        }
    }

    //Drop this script into a UI button's event system and assign the prefab from there
    public void PassNewPrefabToInstantiate (GameObject prefab)
    {
        if (part) //If we already had a part
        {
            //remove it from the list
            if (PlayerParts.Contains(part.transform))
                PlayerParts.Remove(part.transform);
          
            //and anihilate it
            Destroy(part);
        }

        //Pass the new prefab as the part to place prefab
        partPrefab = prefab;
    }

    public void EnablePlayMode()
    {
        //Enable the play mode,
        //basically find every rigid body in our list and make it to not be kinematic
        for(int i = 0; i < PlayerParts.Count; i++)
        {
            if(PlayerParts[i] != null)
                PlayerParts[i].GetComponent<Rigidbody>().isKinematic = false;
        }

        playMode = true;

        //Another way you can do this, is instead of the isKinematic true/false
        //Would be by using the Time.scale and changing it from 0 to 1 etc.
        //Of course for functions that are using the Time.delta time (camera scripts etc.)
        //you should do either another timer or simply avoid using the deltaTime.
    }

    public void ResetGame()
    {
        // Disable play mode
        playMode = false;

        // clear all parts except the base cube
        for (int i = 0; i < PlayerParts.Count; i++)
        {
            if (PlayerParts[i] != baseCube)
            {
                Destroy(PlayerParts[i]);
            }
        }
        PlayerParts.Clear();
        PlayerParts.Add(baseCube);

        // reset base cube
        baseCube.GetComponent<SiegePart_base>().InitializeDirections();
        baseCube.GetComponent<SiegePart_base>().InitializeSockets();
        baseCube.GetComponent<Rigidbody>().isKinematic = true;
        baseCube.position = initPos;
        baseCube.rotation = initRot;
    }

    public void CompleteMarker(Collider collider)
    {
        markers.Remove(collider);
        if (markers.Count == 0)
        {
            completionMarker.material.SetColor("_EmissionColor", Color.green * 1.2f);
            timer.Stop();
        }
    }

    IEnumerator Co_ColorChange()
    {
        var time = 0f;
        while (time < 2f)
        {
            Color emissionCol = completionMarker.material.color;
            
            time += Time.deltaTime;
        }
        yield return null;
    }

    public void StartTimer()
    {
        timer.Activate();
    }
}
