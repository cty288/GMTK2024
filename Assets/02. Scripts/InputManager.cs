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

    /* Input Actions */
    public event Action OnMouseDown;
    public event Action OnMouseUp;
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
            OnMouseDown?.Invoke();
        }

        if (Input.GetMouseButtonUp(0)) {
            OnMouseUp?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEscape?.Invoke();
        }
    }

    public Vector3 GetMouseWorldPosition() {
        return mousePosWorld;
    }
}
