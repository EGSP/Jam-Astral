using UnityEngine;

namespace Game
{
    /// <summary>
    /// Базовый интерфейс для группы объектов, которые перемещает параллакс.
    /// </summary>
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
        /// Находится ли объект в пределах параллакса.
        /// </summary>
        bool InBounds(Transform parallaxObject);

        void Move(Vector3 difference);
    }
}