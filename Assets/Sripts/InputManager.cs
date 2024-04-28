using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;


public class InputManager : MonoBehaviour
{
    [SerializeField] private XRNode lefthandNode;
    [SerializeField] private XRNode righthandNode;
    [SerializeField] float updaterate;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice LControler;
    private InputDevice RControler;

    public bool rightTrigger;
    public bool leftTrigger;
    public bool bothTrigger;
    public bool rightGrip;
    public bool leftGrip;
    public bool bothGrip;

    bool ltriggerButtonAction = false;
    bool rtriggerButtonAction = false;
    bool rGripButtonAction = false;
    bool lGripButtonAction = false;
    void getDevice()
    {
        //get left controller
        InputDevices.GetDevicesAtXRNode(lefthandNode, devices);
        LControler = devices.FirstOrDefault();
        InputDevices.GetDevicesAtXRNode(righthandNode, devices);
        RControler = devices.FirstOrDefault();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (!RControler.isValid || !LControler.isValid)
        {
            getDevice();
        }
        ltriggerButtonAction = false;
        rtriggerButtonAction = false;
        rGripButtonAction = false;
        lGripButtonAction = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (LControler.TryGetFeatureValue(CommonUsages.triggerButton, out ltriggerButtonAction) && ltriggerButtonAction)
        {
            if (RControler.TryGetFeatureValue(CommonUsages.triggerButton, out rtriggerButtonAction) && rtriggerButtonAction)
            {
                bothTrigger = true;
            }
            else if(!bothTrigger)
            {
                leftTrigger = true;
            }
            else if(!leftTrigger)
            {
                leftTrigger = true;
            }
        }

        if(!ltriggerButtonAction &&!rtriggerButtonAction && !rGripButtonAction  && !lGripButtonAction)
        {
            StartCoroutine(stateUpdate());
        }

    }

    IEnumerator stateUpdate()
    {
        yield return new WaitForSeconds(updaterate);
        ltriggerButtonAction = false;
        rtriggerButtonAction = false;
        rGripButtonAction = false;
        lGripButtonAction = false;
    }
}
