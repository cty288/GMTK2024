using System;
using MikroFramework.Architecture;
using System.Collections.Generic;
using MikroFramework.AudioKit;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MushroomEntityManager : MonoBehaviour, ICanGetModel {
    //Might use this script to keep control of all the mushroom spawned via container
    //we shall see if this script is necessary later on. if not i will merge this with input manager or game manager

    public static MushroomEntityManager Instance;

    public PlayerCurrency currency;
    [SerializeField] private LargestSize sizeUI;

    [SerializeField] private Button sellModeButton;
    
    [SerializeField] private int lastDay = 100;

    private InputManager inputManager;
    private bool sellMode = false;
    private Mushroom selectedMushroom;
    private List<Mushroom> allMushrooms = new List<Mushroom>();

    public int mushroomsToSpawn = 3;
    public Vector2 rangeX = new Vector2(-9f, 9f);
    public Vector2 rangeY = new Vector2(-5f, 5f);

    [SerializeField] private Texture2D cursor;

    private int debugCount = 0;

    public Action OnEndGame;

    private MushroomData largestMushroom;
    private int dayLargest;
    [SerializeField] private float largestSize = 0;

    public void Awake() {
        AudioSystem.Singleton.Initialize(null);
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
        RandomlySpawnMushrooms();
    }

    private void Update() {
        /*if (Input.GetKeyDown(KeyCode.Space)) {
            RandomlySpawnMushrooms();
        }*/

        //if (Input.GetKeyDown(KeyCode.P)) {
        //    IncrementDay();
        //}
    }

    public void IncrementDay() {
        this.GetModel<GameTimeModel>().Day.Value++;

        foreach (var mushroom in GetAllMushrooms())
        {
            CheckLargestMushroom(mushroom);
        }
        
        EndGame();
        //UpdateMushroomNeighbors();
    }

    private void RandomlySpawnMushrooms() {
        for (int i = 0; i < mushroomsToSpawn; i++) {
            Vector2 randomPosition = new Vector2(Random.Range(rangeX.x, rangeX.y), Random.Range(rangeY.x, rangeY.y));
            Mushroom shroom = SpawnMushroom(randomPosition, 1, 1, Random.Range(1, 5));
            shroom.GetMushroomData().OnPlantToFarm();
        }
    }

    public Mushroom SpawnMushroom(Vector2 position, int minTrait, int maxTrait, int growthDay = 1) {
        GameObject mushroomGO = MushroomGenerator.GenerateRandomMushroom(minTrait, maxTrait, position, growthDay);

        return RegisterMushroom(mushroomGO); ;
    }

    public Mushroom SpawnMushroom(MushroomData data, Vector2 position) {
        GameObject mushroomGO = MushroomGenerator.GenerateCustomMushroom(data, position);

        return RegisterMushroom(mushroomGO); ;
    }

    private Mushroom RegisterMushroom(GameObject mushroomGO) {
        mushroomGO.name = mushroomGO.name + "_" + debugCount++;
        mushroomGO.transform.SetParent(transform);
        Mushroom mushroom = mushroomGO.GetComponent<Mushroom>();
        mushroom.RegenerateCollider();
        mushroom.OnMushroomDestroyed += OnMushroomDestroyed;
        allMushrooms.Add(mushroom);

        return mushroom;
    }

    public List<Mushroom> GetAllMushrooms() {
        return allMushrooms;
    }

    public void CheckLargestMushroom(Mushroom mushroom)
    {
        if(mushroom.GetMushroomData().GetStage() == 1) return;
        if (mushroom.GetMushroomData().GetSize() > largestSize)
        {
            largestMushroom = MushroomDataHelper.CopyMushroomData(mushroom.GetMushroomData());
            largestSize = largestMushroom.GetSize();
            dayLargest = this.GetModel<GameTimeModel>().Day.Value;
            sizeUI.Modify(largestSize);
        }
    }

    public void EndGame()
    {
        if (this.GetModel<GameTimeModel>().Day.Value >= lastDay)
        {
            // Delete all other mushrooms
            foreach (var mushroom in GetAllMushrooms())
            {
                mushroom.DestroySelf();
            }
            // Center the camera
            Camera.main.transform.position = new Vector3(0, 0, -10);
            
            // Spawn the copy of the largest mushroom
            var largest = MushroomGenerator.GenerateCustomMushroom(largestMushroom, Vector3.zero);
            OnEndGame.Invoke();
            
            // Lock data panel and set day to day the mushroom was recorded.
            MushroomDataPanel.Instance.TurnOnPanel();
            MushroomDataPanel.Instance.SetPanelDisplay(largestMushroom);
            MushroomDataPanel.Instance.UpdateDayText(lastDay, dayLargest);
            MushroomDataPanel.Instance.ToggleLock(true);
        }
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }

    private void CheckSelectMushroom() {
        if (inputManager.IsMouseOverUI()) return;

        RaycastHit2D hit = Physics2D.Raycast(inputManager.GetMouseWorldPosition(), Vector2.zero);
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
                hit.collider.enabled = false;
                currency.Modify(mushroom.GetMushroomData().GetSellPrice());
                mushroom.DestroySelf();
            }
        }
    }

    public void OnMushroomDestroyed(Mushroom mushroom) {
        allMushrooms.Remove(mushroom);
    }

    public void SellModeToggle() {
        sellMode = !sellMode;
        sellModeButton.targetGraphic.color = sellMode ? Color.red : Color.white;

        if (sellMode) {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);

            inputManager.OnMouseDown -= CheckSelectMushroom;
            inputManager.OnMouseDown += CheckDestroyMushroom;
        } else {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            inputManager.OnMouseDown -= CheckDestroyMushroom;
            inputManager.OnMouseDown += CheckSelectMushroom;
        }
    }
}
