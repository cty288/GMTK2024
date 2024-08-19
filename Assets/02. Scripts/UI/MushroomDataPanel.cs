using MikroFramework.Architecture;
using TMPro;
using UnityEngine;

public class MushroomDataPanel : MonoBehaviour, ICanGetModel {
    public static MushroomDataPanel Instance;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        if (Instance != this) {
            Destroy(this);
        }
    }

    [SerializeField] private GameObject UIGroup;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI[] traitTexts;
    [SerializeField] private GameObject[] mushroomLives;
    [SerializeField] private TextMeshProUGUI dataText;

    private void Start() {
        this.GetModel<GameTimeModel>().Day.RegisterOnValueChanged(UpdateDayText);
    }

    private void UpdateDayText(int oldDay, int newDay) {
        dayText.text = $"Day: {newDay}";
    }

    public void SetPanelDisplay(MushroomData data) {
        //TODO: handle naming mushrooms
        nameText.text = $"Mushroom: Day {data.GrowthDay.Value}";

        dataText.text = MushroomDataHelper.ToString(data);

        for (int i = 0; i < mushroomLives.Length; i++) {
            mushroomLives[i].SetActive(i < data.GrowthDay.Value);
        }

        for (int i = 0; i < traitTexts.Length; i++) {
            traitTexts[i].text = "--";
        }

        var count = 0;
        foreach (var trait in data.GetTraits()) {
            traitTexts[count].text = trait.ToString();
            count++;
        }
    }

    public void ResetPanelDisplay() {
        nameText.text = "No MusHRoom sEleCTED";
        dataText.text = "data here lmao";
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }

    public void TurnOnPanel() {
        UIGroup.SetActive(true);
    }
    public void TurnOffPanel() {
        UIGroup.SetActive(false);
    }
}