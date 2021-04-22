using Egsp.Extensions.Primitives;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Компонент визуального отображения направления атаки игрока. Находится в пространстве объектов, а не интерфейса.
    /// </summary>
    public class PlayerAttackVisual : SerializedMonoBehaviour
    {
        [TitleGroup("Attack")] [SerializeField] private Transform attackVisual;

        [SerializeField] private Player player;

        private void Update()
        {
            Rotate(player);
        }

        public void Rotate(Player playerParameter)
        {
            if (playerParameter == null)
                return;

            var visualPosition = playerParameter.transform.position + playerParameter.LookDirection *
                player.AbilityDistance;
            
            attackVisual.position = visualPosition;
            attackVisual.localRotation = playerParameter.LookDirection.LookRotation2D();
        }
    }
}