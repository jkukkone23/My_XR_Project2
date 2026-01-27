using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitcher : MonoBehaviour
{
    public Transform roomPosition;         
    public Transform externalPosition;     
    public InputActionReference switchAction; // Button handler

    private bool atExternal = false;       // True if outside of the room

    void Start()
    {
        transform.position = roomPosition.position;
        transform.rotation = roomPosition.rotation;

        switchAction.action.Enable();
        switchAction.action.performed += SwitchPosition;
    }

    void SwitchPosition(InputAction.CallbackContext ctx)
    {
        if (atExternal)
        {
            // We go in to the room
            transform.position = roomPosition.position;
            transform.rotation = roomPosition.rotation;
            atExternal = false;
        }
        else
        {
            // We move outside of the room
            transform.position = externalPosition.position;
            transform.rotation = externalPosition.rotation;
            atExternal = true;
        }
    }
}
