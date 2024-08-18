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

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dataText;

    private void Start() {
        this.GetModel<GameTimeModel>().Day.RegisterOnValueChanged(UpdateDayText);
    }

    private void UpdateDayText(int oldDay, int newDay) {
        dayText.text = $"Day: {newDay}";
    }

    public void SetPanelDisplay(MushroomData data) {
        nameText.text = $"Mushroom: Day {data.GrowthDay.Value}";
        dataText.text = MushroomDataHelper.ToString(data);
        dataText.text += "\nTraits: \n";
        foreach (var trait in data.GetTraits()) {
            dataText.text += trait.ToString() + "\n";
        }
    }

    public void ResetPanelDisplay() {
        nameText.text = "No MusHRoom sEleCTED";
        dataText.text = "data here lmao";
    }

    public IArchitecture GetArchitecture() {
        return MainGame.Interface;
    }
}