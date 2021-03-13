using UnityEngine;

namespace Game
{
    public sealed class ParallaxGroup : MonoBehaviour, IParallaxGroup
    {
        [SerializeField] [Tooltip("Глубина группы объектов. Чем больше значение тем дальше от цели группа.")]
        [Range(0,1)]
        private float depth;
        
        public IParallax Parallax { get; set; }

        public float Depth => depth;

        public float Ratio => Parallax.GetRatio(this);

        public bool InBounds(Transform parallaxObject) 
            => Parallax.ThresholdBounds.Contains(parallaxObject.position);
        
        public void Move(Vector3 difference)
        {
            var ratio = Ratio;
            
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);

                child.position += difference * ratio;
            }
        }
    }
}