using UnityEngine;

public struct MushroomData {
    public float capHeight;
    public float capWidth;
    public float stemHeight;
    public float stemWidth;
    public Vector2 oscillation;
    public float oscillationSpeed;
    public Color capColor;
    public Color stemColor;
    public bool isPoisonous;
    public float sporeRange;
}

public static class MushroomDataHelper {
    public static MushroomData GetControlMushroomData() {
        return new MushroomData {
            capHeight = 1f,
            capWidth = 1f,
            stemHeight = 1f,
            stemWidth = 1f,
            oscillation = new Vector2(1.2f, 1.2f),
            oscillationSpeed = 1f,
            capColor = Color.red,
            stemColor = Color.white,
            isPoisonous = false,
            sporeRange = 1f
        };
    }

    public static MushroomData GetRandomMushroomData() {
        return new MushroomData {
            capHeight = Random.Range(0.3f, 1.8f),
            capWidth = Random.Range(0.3f, 1.8f),
            stemHeight = Random.Range(0.3f, 1.8f),
            stemWidth = Random.Range(0.3f, 1.8f),
            oscillation = new Vector2(Random.Range(0.8f, 1.3f), Random.Range(0.8f, 1.3f)),
            oscillationSpeed = Random.Range(0.3f, 0.9f),
            capColor = new Color(Random.value, Random.value, Random.value),
            stemColor = new Color(Random.value, Random.value, Random.value),
            isPoisonous = Random.value > 0.5f,
            sporeRange = Random.Range(0.8f, 1.6f)
        };
    }

    public static string ToString(MushroomData data) {
        return
                $"Cap Height: {data.capHeight}\n" +
                $"Cap Width: {data.capWidth}\n" +
                $"Stem Height: {data.stemHeight}\n" +
                $"Stem Width: {data.stemWidth}\n" +
                $"Oscillation: {data.oscillation}\n" +
                $"Oscillation Speed: {data.oscillationSpeed}\n" +
                $"Cap Color: {data.capColor}\n" +
                $"Stem Color: {data.stemColor}\n" +
                $"Is Poisonous: {data.isPoisonous}\n" +
                $"Spore Range: {data.sporeRange}";
    }
}