using UnityEngine;

public class MushroomSpawner : MonoBehaviour {
    public GameObject mushroomPrefab;

    public int mushroomsToSpawn = 10;
    public Vector2 rangeX = new Vector2(-9f, 9f);
    public Vector2 rangeY = new Vector2(-5f, 5f);

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SpawnMushrooms();
        }
    }

    private void SpawnMushrooms() {
        for (int i = 0; i < mushroomsToSpawn; i++) {
            Vector2 randomPosition = new Vector2(Random.Range(rangeX.x, rangeX.y), Random.Range(rangeY.x, rangeY.y));
            GameObject mushroomGO = Instantiate(mushroomPrefab, randomPosition, Quaternion.identity);
            mushroomGO.transform.SetParent(transform);
        }
    }
}
