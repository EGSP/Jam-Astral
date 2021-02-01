using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public interface IParallax
    {
        [CanBeNull] Transform Target { set; }
        
        /// <summary>
        /// Границы параллакса.
        /// </summary>
        Bounds Bounds { get; }
        
        /// <summary>
        /// Границы параллакса с учетом издержки.
        /// </summary>
        Bounds ThresholdBounds { get; }
        
        float GetRatio(IParallaxGroup group);

        void Add(IParallaxGroup group);
    }
}