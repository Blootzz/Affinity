using UnityEngine;

namespace SuperTiled2Unity
{
    public class SuperObjectLayer : SuperLayer
    {
        [ReadOnly]
        public Color m_Color;
        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Invisible Zone");
        }
    }
}
