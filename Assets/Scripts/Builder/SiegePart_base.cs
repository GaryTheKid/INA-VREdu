using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SiegePart_base : MonoBehaviour {

    public Renderer rendererToFindEdges; //The render which would use to find the center and as a transform

    List<Vector3> directions = new List<Vector3>();//The list with all available positions we can sockets at
    List<Transform> Sockets = new List<Transform>(); //Our socket gameobjects
    List<Transform> disabledSockets = new List<Transform>(); //The disabled game objects
    Joint joint; //our reference to our joint


    //Every direction we can enable/disable
    //we do it this way so that we don't clutter up the inspector
    [SerializeField]
    SelectDirections selectDirections;

    [System.Serializable]
    public class SelectDirections
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool forward;
        public bool back;
    }

    //Debug stuff
    public bool debugPos;
    public GameObject debugPrefab;

	void Start () 
    {
        Transform trans = rendererToFindEdges.transform; //Get the transform from the renderer
        
        //If we have a Joint, then store it
        //This if statement is basically because the Base Cube doesn't have a Joint so it would give errors otherwise
        if(GetComponent<Joint>())
            joint = GetComponent<Joint>();

        //Now for every available position we can have a socket
	    if(selectDirections.up)
        {                
            //Find it based on the center of the mesh we want
            Vector3 addDirection = trans.up + new Vector3(0, rendererToFindEdges.bounds.extents.y - 1, 0);
            Vector3 finalPos = trans.position + addDirection;

            //And add it on a list
            directions.Add(finalPos);
        }

        if (selectDirections.down)
        {
            Vector3 addDirection = -trans.up + new Vector3(0, -rendererToFindEdges.bounds.extents.y + 1, 0);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.right)
        {
            Vector3 addDirection = trans.right + new Vector3(rendererToFindEdges.bounds.extents.x - 1, 0, 0);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.left)
        {
            Vector3 addDirection = -trans.right + new Vector3(-rendererToFindEdges.bounds.extents.x + 1, 0, 0);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.forward)
        {
            Vector3 addDirection = trans.forward + new Vector3(0, 0, rendererToFindEdges.bounds.extents.z - 1);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }

        if (selectDirections.back)
        {
            Vector3 addDirection = -trans.forward + new Vector3(0, 0, -rendererToFindEdges.bounds.extents.z + 1);
            Vector3 finalPos = trans.position + addDirection;

            directions.Add(finalPos);
        }        

        //For every direction
        for (int i = 0; i < directions.Count; i++)
        {
            GameObject obj;

            if (debugPos)
            {
                //either instatiate a game object since we are debuging, or..
                obj = (GameObject)Instantiate(debugPrefab, directions[i], Quaternion.identity);
            }
            else
            {
                //..create a new game object
                obj = new GameObject();
                obj.transform.position = directions[i];
            }

            obj.transform.parent = transform;
            Sockets.Add(obj.transform);//and add it to a list
        }

        //Alternatively, you can do all the above (or avoid it) by doing all this by hand from the inspector
        
	}

    //Find the closest socket
    public Transform ReturnClosestDirection(Vector3 pos)
    {
        Transform retVal = null;

        if (directions.Count > 0)
        {
            Transform closestTrans = Sockets[0];
            Vector3 closestPos = Sockets[0].position;
            float distance = Vector3.Distance(pos, closestPos);

            for (int i = 0; i < directions.Count; i++)
            {
                float tempDist = Vector3.Distance(pos, Sockets[i].position);

                if (tempDist < distance)
                {
                    closestPos = Sockets[i].position;
                    closestTrans = Sockets[i];
                    distance = tempDist;
                }
            }

            retVal = closestTrans;
        }

        return retVal;
        
    }

    //Disable a socket and put it on the disabled sockets list
    public void DisableSocket(Transform socket)
    {
        if (socket)
        {
            socket.gameObject.SetActive(false);

            if (Sockets.Contains(socket))
                Sockets.Remove(socket);

            disabledSockets.Add(socket);
        }
    }

    //Pass a transform to assign as a target to the join, Note: A transform with a rigidbody that is!
    public void AssignTargetToJoint(Transform target)
    {
            joint.connectedBody = target.GetComponent<Rigidbody>();
    }
}
