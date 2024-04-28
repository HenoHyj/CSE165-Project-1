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
    [SerializeField] XRNode leftHandNode; 
    //[SerializeField] XRNode leftHandNode;
    private List<InputDevice> devices = new List<InputDevice>();
    InputDevice leftControler;
    XRSimpleInteractable curInteractable;
    // Start is called before the first frame update

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!leftControler.isValid)
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
            curInteractable = rayHit.transform.GetComponent<XRSimpleInteractable>();
            lineRend.enabled = true;
            lineRend.SetPosition(0, this.transform.position);
            lineRend.SetPosition(1, rayHit.point);
            lineRend.startColor = Color.white;
            lineRend.endColor = Color.white;

            //Teleport when left trigger are pressed
            bool leftTriggerPress = false;
            if (leftControler.TryGetFeatureValue(CommonUsages.triggerButton, out leftTriggerPress) && leftTriggerPress)
            {

                //curInteractable.SelectObj();
                lineRend.startColor = Color.white;
                lineRend.endColor = Color.white;
                Debug.Log("trigger pulled");
            }
        }
        else
        {
            lineRend.startColor = Color.red;
            lineRend.endColor = Color.red;
        }

    }
}
