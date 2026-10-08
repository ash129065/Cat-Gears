using Game.Art;
using UnityEngine;

// Fits the camera so the painted art always fills the screen width:
//   orthographicSize = artWidth / (2 * aspect)
// Use this OR BoardCameraFitter on a level, not both. Both control the same camera.
namespace Game.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class ArtCameraFitter : MonoBehaviour
    {
        public enum VerticalAnchor { Top, Center, Bottom }

        [SerializeField] MapArt mapArt;
        // Used when the screen is taller than the art: this edge of the art is pinned to the screen edge
        // and the extra height shows beyond the opposite side (where your blending decoration goes).
        [SerializeField] VerticalAnchor anchor = VerticalAnchor.Top;
        // Crops this fraction of the art width so a hairline of background never shows at the sides.
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
            cam.orthographicSize = art.size.x * (1f - edgeCrop) / (2f * aspect);

            float halfH = cam.orthographicSize;
            float y;
            if (halfH * 2f >= art.size.y)
            {
                // Taller screen: pin one edge of the art, the extra height shows on the other side.
                y = anchor == VerticalAnchor.Top ? art.max.y - halfH
                    : anchor == VerticalAnchor.Bottom ? art.min.y + halfH
                    : art.center.y;
            }
            else
            {
                // Shorter screen (tablets): follow the lawn, but never show beyond the art.
                y = Mathf.Clamp(mapArt.BoardCenter.y, art.min.y + halfH, art.max.y - halfH);
            }

            transform.position = new Vector3(art.center.x, y, transform.position.z);
        }
    }
}