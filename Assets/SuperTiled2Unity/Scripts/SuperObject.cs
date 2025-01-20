using UnityEngine;

namespace SuperTiled2Unity
{
    public class SuperObject : MonoBehaviour
    {
        [ReadOnly]
        public int m_Id;

        [ReadOnly]
        public string m_TiledName; //Name of what we are importing from Tiled (can be identical to others in the scene)

        [ReadOnly]
        public string m_Type;

        [ReadOnly]
        public float m_X;

        [ReadOnly]
        public float m_Y;

        [ReadOnly]
        public float m_Width;

        [ReadOnly]
        public float m_Height;

        [ReadOnly]
        public float m_Rotation;

        [ReadOnly]
        public uint m_TileId;

        [ReadOnly]
        public SuperTile m_SuperTile;

        [ReadOnly]
        public bool m_Visible;

        [ReadOnly]
        public string m_Template;

        public void Start()
        {
            if (m_TiledName.Equals("Ledge"))
            {
                //It's a "Ledge," as defined in Tiled (Doesn't matter if Unity calls it Ledge(1), Ledge(2), etc
                CreateLedge();
                gameObject.layer = LayerMask.NameToLayer("Invisible Zone");
                return;
            }
        }

        public void CreateLedge()
        {
            gameObject.tag = "Ledge";
            CircleCollider2D circle = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
            circle.isTrigger = true;
            circle.radius = 0.3f;// At 0.5, the ledge can be reached from above, causing the player to be able to grab the ledge from the wrong side sometimes
        }    

        public Color CalculateColor()
        {
            Color color = Color.white;

            foreach (var layer in gameObject.GetComponentsInParent<SuperLayer>())
            {
                color *= layer.m_TintColor;
                color.a *= layer.m_Opacity;
            }

            return color;
        }
    }
}
