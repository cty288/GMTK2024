using System;
using MikroFramework.Architecture;
using System.Collections.Generic;
using MikroFramework.AudioKit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MushroomEntityManager : MonoBehaviour, ICanGetModel {
    //Might use this script to keep control of all the mushroom spawned via container
    //we shall see if this script is necessary later on. if not i will merge this with input manager or game manager

    public static MushroomEntityManager Instance;

    public PlayerCurrency currency;
    [SerializeField] private LargestSize sizeUI;

    [SerializeField] private Button sellModeButton;
    
    [SerializeField] private int lastDay = 100;

    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject nextDayButton;

    [SerializeField] private Camera captureCamera;
    public RawImage displayImage;
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
            mushroom.RegenerateCollider();
            /*
            Texture2D mushroomTexture = CaptureMushroomImage(mushroom.gameObject);
            if(mushroomTexture != null)
            {
                
                Debug.Log("success");
                displayImage.texture = mushroomTexture;

                // Get the RectTransform of the RawImage
                RectTransform imageRect = displayImage.rectTransform;

                // Calculate the aspect ratio of the texture
                float textureAspect = (float)mushroomTexture.width / mushroomTexture.height;

                // Calculate the aspect ratio of the RawImage
                float imageAspect = imageRect.rect.width / imageRect.rect.height;

                if (textureAspect > imageAspect)
                {
                    // Texture is wider, so we'll fit to width
                    displayImage.uvRect = new Rect(0, 0, 1, 1);
                    float newHeight = imageRect.rect.width / textureAspect;
                    imageRect.sizeDelta = new Vector2(imageRect.rect.width, newHeight);
                }
                else
                {
                    // Texture is taller, so we'll fit to height
                    displayImage.uvRect = new Rect(0, 0, 1, 1);
                    float newWidth = imageRect.rect.height * textureAspect;
                    imageRect.sizeDelta = new Vector2(newWidth, imageRect.rect.height);
                }

            }
            */
            largestMushroom = MushroomDataHelper.CopyMushroomData(mushroom.GetMushroomData());
            largestSize = largestMushroom.GetSize();
            dayLargest = this.GetModel<GameTimeModel>().Day.Value;
            sizeUI.Modify(largestSize);
        }
    }
    public Texture2D CaptureMushroomImage(GameObject mushroom)
    {
        
        CompositeCollider2D collider = mushroom.GetComponent<CompositeCollider2D>();
        if (collider == null)
        {
            
            return null;
        }

        Bounds mushroomBounds = collider.bounds;

       
        Vector3 originalPosition = captureCamera.transform.position;
        float originalOrthographicSize = captureCamera.orthographicSize;
        RenderTexture originalTargetTexture = captureCamera.targetTexture;

        // Position the camera to frame the mushroom
        captureCamera.transform.position = new Vector3(mushroomBounds.center.x, mushroomBounds.center.y, captureCamera.transform.position.z);

        // Calculate the required orthographic size to fit the mushroom
        float orthographicSize = Mathf.Max(mushroomBounds.extents.x * Screen.height / Screen.width, mushroomBounds.extents.y);
        captureCamera.orthographicSize = orthographicSize;

        // Create a render texture and set it as the camera's target
        RenderTexture renderTexture = new RenderTexture(512, 512, 24);
        captureCamera.targetTexture = renderTexture;

        captureCamera.gameObject.SetActive(true);
        captureCamera.Render();

       
        Texture2D capturedTexture = new Texture2D(512, 512, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        capturedTexture.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        capturedTexture.Apply();

        captureCamera.transform.position = originalPosition;
        captureCamera.orthographicSize = originalOrthographicSize;
        captureCamera.targetTexture = originalTargetTexture;

        // Clean up
        RenderTexture.active = null;
        Destroy(renderTexture);
        captureCamera.gameObject.SetActive(false);
        return capturedTexture;
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
            Camera.main.transform.position = new Vector3(0, 5, -10);
            restartButton.SetActive(true);
            nextDayButton.SetActive(false);
            
        }
    }

    public void RestartGame()
    {
        this.GetModel<GameTimeModel>().Day.Value = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
