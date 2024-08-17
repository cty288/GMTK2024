using UnityEngine;
using UnityEngine.UI;

public class MushroomEntityManager : MonoBehaviour {
    //Might use this script to keep control of all the mushroom spawned via container
    //we shall see if this script is necessary later on. if not i will merge this with input manager or game manager

    public static MushroomEntityManager Instance;

    [SerializeField] private Button deleteModeButton;

    private InputManager inputManager;
    private bool deleteMode = false;
    private Mushroom selectedMushroom;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        if (Instance != this) {
            Destroy(this);
        }
    }

    private void Start() {
        inputManager = InputManager.Instance;
        inputManager.OnMouseDown += CheckSelectMushroom;
    }

    public void CheckSelectMushroom() {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null) {
            Mushroom mushroom = hit.collider.GetComponent<Mushroom>();
            if (mushroom != null) {
                selectedMushroom = mushroom;
                selectedMushroom.Select();

                inputManager.OnMouseDown -= CheckSelectMushroom;
                inputManager.OnMouseUp += RemoveSelectedMushroom;
            }
        }
    }

    private void RemoveSelectedMushroom() {
        selectedMushroom.Deselect();
        inputManager.OnMouseDown += CheckSelectMushroom;
        inputManager.OnMouseUp -= RemoveSelectedMushroom;
    }

    public void CheckDestroyMushroom() {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null) {
            Mushroom mushroom = hit.collider.GetComponent<Mushroom>();
            if (mushroom != null) {
                mushroom.DestroySelf();
            }
        }
    }

    public void DeleteMode() {
        deleteMode = !deleteMode;
        deleteModeButton.targetGraphic.color = deleteMode ? deleteModeButton.colors.selectedColor : deleteModeButton.colors.normalColor;

        if (deleteMode) {
            inputManager.OnMouseDown -= CheckSelectMushroom;
            inputManager.OnMouseDown += CheckDestroyMushroom;
        } else {
            inputManager.OnMouseDown -= CheckDestroyMushroom;
            inputManager.OnMouseDown += CheckSelectMushroom;
        }
    }
}
