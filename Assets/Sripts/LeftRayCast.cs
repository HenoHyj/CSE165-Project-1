using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class LeftRayCast : MonoBehaviour
{
    Ray ray;
    RaycastHit rayHit;
    [SerializeField] float maxDist;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask buttonChair;
    [SerializeField] LayerMask buttonBed;
    [SerializeField] LayerMask buttonPackage;
    [SerializeField] XRNode leftHandNode;
    [SerializeField] GameObject player;
    [SerializeField] GameObject prefab1;
    [SerializeField] GameObject prefab2;
    [SerializeField] GameObject prefab3;
    //[SerializeField] XRNode leftHandNode;
    private List<InputDevice> devices = new List<InputDevice>();
    InputDevice leftControler;
    XRSimpleInteractable curInteractable;
    GameObject curPrefab;
    // Start is called before the first frame update
    bool ltriggerstatus;
    bool leftGripHolding;

    Vector3 targetposition;

    void getDevice()
    {
        InputDevices.GetDevicesAtXRNode(leftHandNode, devices);
        leftControler = devices.FirstOrDefault();
    }

    void Start()
    {
        //Debug.Log("initialized");
        if (!leftControler.isValid)
        {
            getDevice();
        }
        ltriggerstatus = false;
        targetposition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftControler.isValid)
        {
            getDevice();
        }

        ray = new Ray(transform.position, transform.forward);
        lineRend.SetVertexCount(2);
        lineRend.enabled = true;
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, transform.position + maxDist * transform.forward);


        if (Physics.Raycast(ray, out rayHit, maxDist, layerMask))
        {
            curInteractable = rayHit.transform.GetComponent<XRSimpleInteractable>();
            lineRend.SetPosition(1, rayHit.point);
            lineRend.startColor = Color.green;
            lineRend.endColor = Color.green;

            //Teleport when left trigger are pressed
            bool leftTriggerPress = false;
            if (leftControler.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPress) && leftTriggerPress && !ltriggerstatus)
            {
                //update trigger status to prevent teleport more than once
                ltriggerstatus = true;
                //curInteractable.SelectObj();
                lineRend.startColor = Color.blue;
                lineRend.endColor = Color.blue;

                // Change player's position

                targetposition.x = rayHit.point.x;
                targetposition.y = player.transform.position.y;
                targetposition.z = rayHit.point.z;

            }

            if (leftControler.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPress) && !leftTriggerPress && ltriggerstatus)
            {

                Vector3 targetRotation;
                targetRotation = transform.rotation.eulerAngles;
                targetRotation.x = 0;
                targetRotation.z = 0;
                player.transform.rotation = Quaternion.Euler(targetRotation);
                player.transform.position = targetposition;
                ltriggerstatus = false;
            }

            bool leftGripPress = false;
            //Spawning the Object
            if (leftControler.TryGetFeatureValue(CommonUsages.gripButton, out leftGripPress) && !leftGripPress && leftGripHolding)
            {
                
                Vector3 idealSpawnLocation;
                idealSpawnLocation.x = rayHit.point.x;
                idealSpawnLocation.y = player.transform.position.y;
                idealSpawnLocation.z = rayHit.point.z;

                Vector3 idealRotation;
                idealRotation = transform.rotation.eulerAngles;
                idealRotation.x = 0;
                idealRotation.z = 0;

                Instantiate(curPrefab, idealSpawnLocation, Quaternion.Euler(idealRotation));
                //Debug.Log("I get here");
                leftGripHolding = false;
                curPrefab = null;
            }
        }

        //Spawn the Fist Object
        else if (Physics.Raycast(ray, out rayHit, maxDist, buttonChair))
        {
            curInteractable = rayHit.transform.GetComponent<XRSimpleInteractable>();
            lineRend.SetPosition(1, rayHit.point);
            lineRend.startColor = Color.blue;
            lineRend.endColor = Color.blue;

            //Teleport when left trigger are pressed
            bool leftGripPress = false;
            if (leftControler.TryGetFeatureValue(CommonUsages.gripButton, out leftGripPress) && leftGripPress && !leftGripHolding)
            {
                leftGripHolding = true;
                curPrefab = prefab1;
            }
        }


        //Spawn Second Object
        else if (Physics.Raycast(ray, out rayHit, maxDist, buttonBed))
        {
            curInteractable = rayHit.transform.GetComponent<XRSimpleInteractable>();
            lineRend.SetPosition(1, rayHit.point);
            lineRend.startColor = Color.blue;
            lineRend.endColor = Color.blue;

            //Teleport when left trigger are pressed
            bool leftGripPress = false;
            if (leftControler.TryGetFeatureValue(CommonUsages.gripButton, out leftGripPress) && leftGripPress && !leftGripHolding)
            {
                leftGripHolding = true;
                curPrefab = prefab2;
            }
        }

        //Spawn Third Object
        else if (Physics.Raycast(ray, out rayHit, maxDist, buttonPackage))
        {
            curInteractable = rayHit.transform.GetComponent<XRSimpleInteractable>();
            lineRend.SetPosition(1, rayHit.point);
            lineRend.startColor = Color.blue;
            lineRend.endColor = Color.blue;

            //Teleport when left trigger are pressed
            bool leftGripPress = false;
            if (leftControler.TryGetFeatureValue(CommonUsages.gripButton, out leftGripPress) && leftGripPress && !leftGripHolding)
            {
                leftGripHolding = true;
                curPrefab = prefab3;
            }
        }

        else
        {
            lineRend.startColor = Color.red;
            lineRend.endColor = Color.red;
        }

    }
}
