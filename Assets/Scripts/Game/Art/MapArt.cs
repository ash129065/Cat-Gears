using UnityEngine;

namespace Game.Art
{
    public class MapArt : MonoBehaviour
    {
        [SerializeField] SpriteRenderer art;     // the painted road + lawn sprite
        [SerializeField] Transform lawnCenter;   // an empty child placed at the centre of the painted lawn
        [SerializeField] Vector2 lawnSize = new Vector2(6.5f, 6.3f);   // painted lawn size in slots (measure yours)

        public Bounds ArtBounds => art.bounds;           // width and height of the art in world units
        public Vector3 BoardCenter => lawnCenter.position;

        // The gears must sit on the painted grass, so a board only fits if it is no bigger than the lawn.
        // public bool Fits(BoardModel model) => model.cols <= lawnSize.x && model.rows <= lawnSize.y;
    }
}