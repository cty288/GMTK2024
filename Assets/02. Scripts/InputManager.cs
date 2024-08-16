using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
    public static InputManager Instance;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        if (Instance != this) {
            Destroy(this);
        }

        mainCamera = Camera.main;
    }

    /* General Data */
    private Camera mainCamera;
    private Vector3 mousePos;
    private Vector3 mousePosWorld;

    /* Camera Panning */
    public float panSpeed = 20f;   // Speed at which the camera will pan
    public Vector2 panLimitMin;    // Minimum limits for panning
    public Vector2 panLimitMax;    // Maximum limits for panning
    private Vector3 dragOrigin;    // Starting position of the mouse drag

    /* Input Actions */
    //public event Action OnMouseClick;
    public event Action OnEscape;

    public void Update() {
        UpdateMouse();
        HandleInput();
    }

    private void UpdateMouse() {
        mousePos = Input.mousePosition;
        mousePos.z = mainCamera.nearClipPlane + 1;
        mousePosWorld = mainCamera.ScreenToWorldPoint(mousePos);
        mousePosWorld.z = 0;
    }

    private void HandleInput() {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0)) {
            //OnMouseClick?.Invoke();

            //TODO: Make Clicking on Objects Eat Input

            // Panning: Record the initial position of the mouse when dragging starts
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            print(dragOrigin);
        }

        PanCamera();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEscape?.Invoke();
        }
    }

    void PanCamera() {
        if (Input.GetMouseButton(0)) {
            print("Panning");

            //var delta : Vector3 = Input.mousePosition - lastPosition;
            //transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            //lastPosition = Input.mousePosition;
        }
    }

    public Vector3 GetMouseWorldPosition() {
        return mousePosWorld;
    }
}
