using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    private Light pointLight;
    public InputActionReference switchAction;

    private int colorIndex = 0;

    void Start()
    {
        pointLight = GetComponent<Light>();

        switchAction.action.Enable();
        switchAction.action.performed += ChangeLight;
    }

    void ChangeLight(InputAction.CallbackContext ctx)
    {
        // 0 = white, 1 = blue, 2 = red
        if (colorIndex == 0)
        {
            pointLight.color = Color.blue;
            colorIndex = 1;
        }
        else if (colorIndex == 1)
        {
            pointLight.color = Color.red;
            colorIndex = 2;
        }
        else
        {
            pointLight.color = Color.white;
            colorIndex = 0;
        }
    }
}
