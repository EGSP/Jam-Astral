using UnityEngine;

namespace Game
{
    public interface IParallaxGroup
    {
        /// <summary>
        /// Параллакс, содержащий данную группу.
        /// </summary>
        IParallax Parallax { get; set; }
        
        /// <summary>
        /// Глубина группы.
        /// </summary>
        float Depth { get; }
        
        /// <summary>
        /// Множитель движения объектов группы.
        /// </summary>
        float Ratio { get; }

        /// <summary>
        /// Находится ли объект в пределах параллакса.
        /// </summary>
        bool InBounds(Transform parallaxObject);

        void Move(Vector3 difference);
    }
}