using System.Collections;
using System.Collections.Generic;
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
        this.transform.SetParent(controller.transform);
    }

    public void DisSelectObj()
    {
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
        if (this.transform.parent != prevParent)
        {
            this.transform.SetParent(prevParent.transform);
        }
        
        //this.transform.SetParent(prevParent.transform);
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
