using MikroFramework.Architecture;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MushroomEntityManager : MonoBehaviour, ICanGetModel {
    //Might use this script to keep control of all the mushroom spawned via container
    //we shall see if this script is necessary later on. if not i will merge this with input manager or game manager

    public static MushroomEntityManager Instance;

    [SerializeField] private Button deleteModeButton;

    private InputManager inputManager;
    private bool deleteMode = false;
    private Mushroom selectedMushroom;
    private List<Mushroom> allMushrooms = new List<Mushroom>();

    public GameObject mushroomPrefab;

    public int mushroomsToSpawn = 3;
    public Vector2 rangeX = new Vector2(-9f, 9f);
    public Vector2 rangeY = new Vector2(-5f, 5f);

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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            RandomlySpawnMushrooms();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            this.GetModel<GameTimeModel>().Day.Value++;
        }
    }

    private void RandomlySpawnMushrooms() {
        for (int i = 0; i < mushroomsToSpawn; i++) {
            SpawnMushroom();
        }
    }

    private void SpawnMushroom() {
        Vector2 randomPosition = new Vector2(Random.Range(rangeX.x, rangeX.y), Random.Range(rangeY.x, rangeY.y));
        GameObject mushroomGO = MushroomGenerator.GenerateRandomMushroom(1, 1, randomPosition);
        mushroomGO.transform.SetParent(transform);
        Mushroom mushroom = mushroomGO.GetComponent<Mushroom>();
        mushroom.RegenerateCollider();
        allMushrooms.Add(mushroom);
    }

    private List<Mushroom> GetAllMushrooms() {
        return allMushrooms;
    }

    public List<Mushroom> GetMushroomsInRange(Vector2 position, float range) {
        List<Mushroom> results = new List<Mushroom>();
        foreach (Mushroom mushroom in allMushrooms) {
            if (Vector2.Distance(mushroom.transform.position, position) <= range) {
                results.Add(mushroom);
            }
        }
        return results;
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }

    private void CheckSelectMushroom() {
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
                allMushrooms.Remove(mushroom);
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
