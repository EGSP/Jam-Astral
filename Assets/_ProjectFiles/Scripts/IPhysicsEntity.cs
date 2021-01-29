using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public interface IPhysicsEntity
    {
        /// <summary>
        /// Приложение силы к физической сущности.
        /// </summary>
        /// <param name="force">Прикладываемая сила.</param>
        /// <param name="actor">Отправитель.</param>
        void ApplyForce(Force force, [CanBeNull] IPhysicsEntity actor = null);
    }

    /// <summary>
    /// Представление физической силы, которое содержит вектор и режим.
    /// </summary>
    public struct Force
    {
        private static readonly Force zeroForce = new Force(Vector3.zero);

        public Vector3 vector;
        public ForceMode mode;

        public static Force Zero => zeroForce;

        public Force(Vector3 _vector, ForceMode _mode = ForceMode.Force)
        {
            vector = _vector;
            mode = _mode;
        }
    }
}