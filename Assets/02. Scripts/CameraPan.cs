using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPan : MonoBehaviour {
    private InputManager inputManager;
    private bool isDragging;
    private Vector3 dragOrigin;

    private void Start() {
        inputManager = InputManager.Instance;
        inputManager.OnMouseDown += StartPan;
    }

    public void StartPan() {
        RaycastHit2D hit = Physics2D.Raycast(inputManager.GetMouseWorldPosition(), Vector2.zero);
        if (hit.collider == null) {
            isDragging = true;
            dragOrigin = inputManager.GetMouseWorldPosition();
            inputManager.OnMouseUp += EndPan;
        }
    }

    public void EndPan() {
        isDragging = false;
        inputManager.OnMouseUp -= EndPan;
    }

    private void FixedUpdate() {
        if (isDragging) {
            PanCamera();
        }
    }

    void PanCamera() {
        if (Input.GetMouseButton(0)) {
            Vector3 difference = dragOrigin - inputManager.GetMouseWorldPosition();
            transform.position += difference;
        }
    }
}
