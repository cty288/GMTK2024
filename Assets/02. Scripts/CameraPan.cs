using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraPan : MonoBehaviour {
    private InputManager inputManager;
    private bool isDragging;
    private Vector3 dragOrigin;
    private bool canPan = true;
    private void Start() {
        inputManager = InputManager.Instance;
        inputManager.OnMouseDown += StartPan;
        inputManager.OnScrollUp += ZoomIn;
        inputManager.OnScrollDown += ZoomOut;
        MushroomEntityManager.Instance.OnEndGame += (() => { canPan = false; });
    }

    public void StartPan() {
        if (inputManager.IsMouseOverUI()) return;

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

    public void ZoomIn() {
        Camera.main.orthographicSize -= 0.2f;
    }

    public void ZoomOut() {
        Camera.main.orthographicSize += 0.2f;
    }

    private void FixedUpdate() {
        if (isDragging) {
            PanCamera();
        }
    }

    void PanCamera() {
        if (Input.GetMouseButton(0) && canPan) {
            Vector3 difference = dragOrigin - inputManager.GetMouseWorldPosition();
            transform.position += difference;
        }
    }
}
