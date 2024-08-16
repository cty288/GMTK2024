using TMPro;
using UnityEngine;

public class MushroomDataPanel : MonoBehaviour {
    public static MushroomDataPanel Instance;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        if (Instance != this) {
            Destroy(this);
        }
    }

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dataText;

    public void SetPanelDisplay(MushroomData data) {
        nameText.text = "CuRRENTLY DISPLAYING MUSHROOM";
        dataText.text = MushroomDataHelper.ToString(data);
    }

    public void ResetPanelDisplay() {
        nameText.text = "No MusHRoom sEleCTED";
        dataText.text = "data here lmao";
    }
}