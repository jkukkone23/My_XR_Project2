using UnityEngine;

public class MagClassCameraVR : MonoBehaviour
{
    public Transform xrCamera;        // VR-lasit (HMD)
    public Transform magnifierLens;   // Suurennuslasiobjektin linssi
    public float zoomFactor = 4f;     // Zoom
    public Vector3 manualOffset = Vector3.zero; // Pieni offset linssin eteen

    private Camera magCamera;

    void Start()
    {
        magCamera = GetComponent<Camera>();
        if (magCamera == null)
            Debug.LogError("MagClassCameraVR: Kameraa ei löydy!");
    }

    void LateUpdate()
    {
        if (xrCamera == null || magnifierLens == null || magCamera == null)
            return;

        //  Lasketaan vektori HMD:n ja linssin sijainnin välillä
        Vector3 direction = (xrCamera.position - magnifierLens.position).normalized;

        //  Asetetaan kamera lasin eteen offsetilla
        float distance = Vector3.Distance(xrCamera.position, magnifierLens.position);
        transform.position = magnifierLens.position + manualOffset;

        //  Magnify out of same line as XR camera
        Vector3 targetPoint = magnifierLens.position - direction;
        transform.LookAt(targetPoint);

        //  Zoomataan FOV:lla
        magCamera.fieldOfView = Mathf.Clamp(60f / zoomFactor, 1f, 179f);
    }
}
