using UnityEngine;

namespace RPG.Core {
    public class PlayerCamera : MonoBehaviour {
        /// <summary>
        /// Control how Player's camera behavior.
        /// </summary>
        Camera cam;

        [SerializeField] Transform target;      // Reference to the object this camera is following.
        [SerializeField] Vector3 offset;

        [SerializeField] float cameraDistance = 8f;
        [SerializeField] float minDistance = 3f;
        [SerializeField] float maxDistance = 10f;
        [SerializeField] float zoomSpeed = 4f;

        void Start() {
            cam = GetComponentInChildren<Camera>();
        }

        private void Update() {
            // Zoom the camera.
            cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);
        }

        private void LateUpdate() {
            // Move camera forward Player.
            transform.position = target.position + new Vector3(0, 1.5f, 0);
            cam.transform.position = transform.position - offset * cameraDistance;
            //cam.transform.LookAt(transform.position);
        }
    }
}