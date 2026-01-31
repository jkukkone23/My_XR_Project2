using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // HOX!! This doesnt work well with one controller grab, it doesn't rotate around the controller
    // It works somewhat okay with two controllers

    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;

    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    // Dynaamic doublerotation action (A button)
    public InputActionReference doubleRotationAction;

    public bool doubleRotation = false; // EXTRA CREDIT toggle

    bool grabbing = false;

    Vector3 prevPos;
    Quaternion prevRot;

    void Start()
    {
        action.action.Enable();
        if (doubleRotationAction != null)
            doubleRotationAction.action.Enable();

        prevPos = transform.position;
        prevRot = transform.rotation;

        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        grabbing = action.action.IsPressed();

        // Grab nearby object or the object in the other hand
        if (grabbing)
        {
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;

            if (grabbedObject)
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                ApplyDeltaManipulation();
        }
        else
        {
            grabbedObject = null;
        }

        // Save transforms for next frame
        prevPos = transform.position;
        prevRot = transform.rotation;
    }


    void ApplyDeltaManipulation()
    {
        Vector3 totalDeltaPos = Vector3.zero;
        Quaternion totalDeltaRot = Quaternion.identity;

        // tämä käsi
        Vector3 dp = transform.position - prevPos;
        Quaternion dq = transform.rotation * Quaternion.Inverse(prevRot);

        totalDeltaPos += dp;
        totalDeltaRot *= dq;

        // toinen käsi
        if (otherHand && otherHand.grabbing)
        {
            Vector3 dpOther = otherHand.transform.position - otherHand.prevPos;
            Quaternion dqOther =
                otherHand.transform.rotation * Quaternion.Inverse(otherHand.prevRot);

            totalDeltaPos += dpOther;
            totalDeltaRot *= dqOther;
        }

        // EXTRA CREDIT
        // --- Dynaamic doublerotation with A button pressed ---
        if (doubleRotationAction != null && doubleRotationAction.action.IsPressed())
        {
            totalDeltaRot.ToAngleAxis(out float angle, out Vector3 axis);
            totalDeltaRot = Quaternion.AngleAxis(angle * 2f, axis);
        }

        // OIKEA pivot: käsien keskipiste (tai yhden käden tapauksessa sen käden prevPos)
        Vector3 pivot;

        if (otherHand && otherHand.grabbing)
            pivot = (prevPos + otherHand.prevPos) * 0.5f;
        else
            pivot = prevPos;

        Vector3 r = grabbedObject.position - pivot;
        Vector3 rRot = totalDeltaRot * r;

        grabbedObject.position = pivot + rRot + totalDeltaPos;
        grabbedObject.rotation = totalDeltaRot * grabbedObject.rotation;
    }



    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}
