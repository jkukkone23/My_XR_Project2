using UnityEngine;

public class MagClassCameraVR : MonoBehaviour
{
    // this doesnt work right but it has to do for now
    // this only works if magGlass is facing upright

    public Transform xrCamera;        // XR headset camera
    public Transform magnifierLens;   // Magnifying glass lens
    public float zoomFactor = 4f;     // Zoomfactor 4 meaning 4 times magnified
    public Vector3 manualOffset = Vector3.zero; // Small offset if needed, this works funny so maybe dont use it?

    private Camera magCamera;

    void Start()
    {
        magCamera = GetComponent<Camera>();
        if (magCamera == null)
            Debug.LogError("MagClassCameraVR: Camera not found!");
    }

    void LateUpdate()
    {
        if (xrCamera == null || magnifierLens == null || magCamera == null)
            return;

        //  Calculate vector between HMD ja lens 
        Vector3 direction = (xrCamera.position - magnifierLens.position).normalized;

        //  Set camera at the position OFFSET optional
        transform.position = magnifierLens.position + manualOffset;

        //  Magnify to the same line as XR camera
        Vector3 targetPoint = magnifierLens.position - direction;
        transform.LookAt(targetPoint, Vector3.up);

        //  Zoom with field of view to given zoom factor
        magCamera.fieldOfView = Mathf.Clamp(60f / zoomFactor, 1f, 179f);
    }
}