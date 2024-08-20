using UnityEngine;
public class MushroomFollowCamera : MonoBehaviour {
    public static MushroomFollowCamera instance;
    private Camera c;
    public Transform target;
    //public RawImage image;
    private void Awake() {
        instance = this;
        c = gameObject.GetComponent<Camera>();
    }

    private void Update() {
        if (target != null) {
            c.transform.position = new Vector3(target.position.x, target.position.y, -3);
        }
    }

    public void UpdateCameraPosition(float x, float y) {

        this.c.gameObject.transform.position = new Vector3(x, y, -3);
    }

}
