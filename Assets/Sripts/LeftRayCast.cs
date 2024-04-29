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
    [SerializeField] XRNode leftHandNode;
    [SerializeField] GameObject player;
    [SerializeField] GameObject chairPrefab;
    [SerializeField] GameObject bedPrefab;
    //[SerializeField] XRNode leftHandNode;
    private List<InputDevice> devices = new List<InputDevice>();
    InputDevice leftControler;
    XRSimpleInteractable curInteractable;
    // Start is called before the first frame update
    bool ltriggerstatus;

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
        }

        //Spawn the Fist Object
        else if (Physics.Raycast(ray, out rayHit, maxDist, buttonChair))
        {
            curInteractable = rayHit.transform.GetComponent<XRSimpleInteractable>();
            lineRend.SetPosition(1, rayHit.point);
            lineRend.startColor = Color.blue;
            lineRend.endColor = Color.blue;

            //Teleport when left trigger are pressed
            bool leftTriggerPress = false;
            if (leftControler.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPress) && leftTriggerPress && !ltriggerstatus)
            {
                ltriggerstatus = true;

                // Instantiate corresponding object (sample here)!
                Vector3 idealSpawnLocation = transform.position;
                idealSpawnLocation.x += transform.forward.x + 0.2f;
                idealSpawnLocation.z += transform.forward.z + 0.2f;
                Instantiate(chairPrefab, idealSpawnLocation, Quaternion.identity);

                //Debug.Log("trigger pulled");
            }

            if (leftControler.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPress) && !leftTriggerPress && ltriggerstatus)
            {
                ltriggerstatus = false;
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
            bool leftTriggerPress = false;
            if (leftControler.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPress) && leftTriggerPress && !ltriggerstatus)
            {
                ltriggerstatus = true;

                // Instantiate corresponding object (sample here)!
                Vector3 idealSpawnLocation = transform.position;
                idealSpawnLocation.x += transform.forward.x + 0.2f;
                idealSpawnLocation.z += transform.forward.z + 0.2f;
                Instantiate(bedPrefab, idealSpawnLocation, Quaternion.identity);

                Debug.Log("trigger pulled");
            }

            if (leftControler.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPress) && !leftTriggerPress && ltriggerstatus)
            {

                ltriggerstatus = false;
            }
        }

        else
        {
            lineRend.startColor = Color.red;
            lineRend.endColor = Color.red;
        }

    }
}
