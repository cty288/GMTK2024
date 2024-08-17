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
    private Vector3 targetPosition;
    public float smoothness;
    public float inertia = 3;
    private bool isDragging;

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
            Debug.Log("clicked");
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            
        }
        PanCamera();


        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnEscape?.Invoke();
        }
    }

    void PanCamera()
    {
        if (Input.GetMouseButton(0) && isDragging)
        {
            inertia = 3;
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = Camera.main.transform.position + new Vector3(difference.x, difference.y, 0);

            // Smoothly move the camera towards the target position
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, smoothness * Time.deltaTime);
        }
        else if( (Vector3.Distance(targetPosition , Camera.main.transform.position) > 1) && isDragging)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, inertia * Time.deltaTime);
            inertia -= Time.deltaTime;
            if(inertia < 0)
            {
                isDragging = false;
            }
        }
    }

    public Vector3 GetMouseWorldPosition() {
        return mousePosWorld;
    }
}
