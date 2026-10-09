using Game.Art;
using UnityEngine;

// Fits the camera so the painted art always fills the screen HEIGHT:
//   orthographicSize = artHeight / 2
// Screens narrower than the art crop the sides, so the camera follows the lawn horizontally
// (never showing beyond the art). Screens wider than the art are centred, and the extra width
// shows beyond the left and right edges (where your blending decoration goes).
// Use this OR BoardCameraFitter on a level, not both. Both control the same camera.
namespace Game.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class ArtCameraFitter : MonoBehaviour
    {
        [SerializeField] MapArt mapArt;
        // Crops this fraction of the art height so a hairline of background never shows at the top or bottom.
        [SerializeField, Range(0f, 0.02f)] float edgeCrop = 0.005f;

        UnityEngine.Camera cam;
        Vector2Int lastScreen;

        void Awake() => cam = GetComponent<UnityEngine.Camera>();
        void OnEnable() => Apply();

        // Refit when the window or orientation changes.
        void Update()
        {
            if (Screen.width != lastScreen.x || Screen.height != lastScreen.y) Apply();
        }

        void Apply()
        {
            if (cam == null) cam = GetComponent<UnityEngine.Camera>();
            lastScreen = new Vector2Int(Screen.width, Screen.height);

            Bounds art = mapArt.ArtBounds;
            float aspect = (float)Screen.width / Screen.height;

            cam.orthographic = true;
            cam.orthographicSize = art.size.y * (1f - edgeCrop) / 2f;

            float halfW = cam.orthographicSize * aspect;
            float x;
            if (halfW * 2f >= art.size.x)
            {
                // Wider screen: centre the art, the extra width shows on both sides.
                x = art.center.x;
            }
            else
            {
                // Narrower screen (most portrait phones): follow the lawn, but never show beyond the art.
                x = Mathf.Clamp(mapArt.BoardCenter.x, art.min.x + halfW, art.max.x - halfW);
            }

            // The view height equals the art height, so the camera sits at the art's vertical centre.
            transform.position = new Vector3(x, art.center.y, transform.position.z);
        }
    }
}