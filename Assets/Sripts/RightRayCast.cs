using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class RightRayCast : MonoBehaviour
{
    Ray ray;
    RaycastHit rayHit;
    [SerializeField] float maxDist;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] LayerMask layerMask;
    [SerializeField] XRNode rightHandNode; 
    //[SerializeField] XRNode leftHandNode;
    private List<InputDevice> devices = new List<InputDevice>();
    InputDevice rightControler;
    MyInteractable curInteractable;
    // Start is called before the first frame update
    bool righTriggerholding;
    void getDevice()
    {
        InputDevices.GetDevicesAtXRNode(rightHandNode, devices);
        rightControler = devices.FirstOrDefault();
        
    }

    void Start()
    {
        righTriggerholding = false;
        //Debug.Log("initialized");
        if (!rightControler.isValid)
        {
            getDevice();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!rightControler.isValid)
        {
            getDevice();
        }
        //Debug.Log("I get here");
        ray = new Ray(transform.position, transform.forward);
        lineRend.SetVertexCount(2);
        lineRend.enabled = true;
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, transform.position + maxDist * transform.forward);
        if (Physics.Raycast(ray, out rayHit, maxDist, layerMask))
        {
            curInteractable = rayHit.transform.GetComponent<MyInteractable>();
            lineRend.enabled = true;
            lineRend.SetPosition(0, this.transform.position);
            lineRend.SetPosition(1, rayHit.point);
            lineRend.startColor = Color.white;
            lineRend.endColor = Color.white;
            //select item
            bool rightTriggerPress = false;
            //Debug.Log("Ray has some hit");

            if (rightControler.TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerPress) && rightTriggerPress && !righTriggerholding)
            {
                curInteractable.SelectObj();
                righTriggerholding = true;
                Debug.Log("trigger pulled");
            }

            //bool rightGrabAction = false;
            //diselect item
            if (rightControler.TryGetFeatureValue(CommonUsages.triggerButton, out rightTriggerPress) && !rightTriggerPress && righTriggerholding)
            {
                curInteractable.DisSelectObj();
                Debug.Log("trigger released");
                righTriggerholding = false;
            }
        }
        else
        {
            lineRend.startColor = Color.red;
            lineRend.endColor = Color.red;
        }

    }
}
