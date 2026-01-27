using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeedY = 30f; // degrees per second for y
    public float rotationSpeedX = 15f; // degrees per second for x

    void Update()
    {
        // Pyöritetään Y-akselin ympäri
        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, 0f);
    }
}

