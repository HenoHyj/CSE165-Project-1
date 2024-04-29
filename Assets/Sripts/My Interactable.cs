using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MyInteractable : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject prevParent;
    [SerializeField] GameObject controller;

    private Color originalColor;
    private Renderer renderer;

    public void SelectObj()
    {
        /*
        if (this.transform.parent != controller.transform) {
            this.transform.SetParent(controller.transform);
        }
        */
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.SetParent(controller.transform);
    }

    public void DisSelectObj()
    {
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
        this.GetComponent<Rigidbody>().isKinematic = false;
        if (this.transform.parent != prevParent)
        {
            this.transform.SetParent(prevParent.transform);
        }
    }

    public void GrabObj()
    {
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
        this.transform.SetParent(controller.transform);
        this.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 targetposition;
        targetposition.x = this.transform.parent.position.x;
        targetposition.y = this.transform.parent.position.y;
        targetposition.z = this.transform.parent.position.z;
        targetposition += 0.1f * this.transform.parent.forward;
        this.transform.position = targetposition;

    }

    public void ReleaseGrabObj()
    {
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
        this.GetComponent<Rigidbody>().isKinematic = false;
        if (this.transform.parent != prevParent)
        {
            this.transform.SetParent(prevParent.transform);
        }
        
    }

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color; // Store the original color
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
