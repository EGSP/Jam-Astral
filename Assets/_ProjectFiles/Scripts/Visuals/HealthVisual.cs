using Egsp.Core;
using Egsp.Core.Ui;
using Game.Entities;
using UnityEngine;

namespace Game.Visuals
{
    /// <summary>
    /// Компонент отрисовывающий текущее состояние здоровья игрока.
    /// </summary>
    public class HealthVisual : Visual, IHealthListener
    {
        [SerializeField] private Transform healthPointPrefab;
        [SerializeField] private TransformContainer transformContainer;
        
        public void Accept(Health health)
        {
            health.HealthPointsBus.Subscribe<IHealthListener>(this);
            DrawAllPoints(health);
        }

        public void DrawAllPoints(Health health)
        {
            transformContainer.Clear();
            for (var i = 0; i < health.HealthPoints.count; i++)
            {
                transformContainer.PutPrefab(healthPointPrefab);
            }
        }
        
        public void OnHealed(Health health, HealthPoint heal)
        {
            for (var i = 0; i < heal.count; i++)
            {
                transformContainer.PutPrefab(healthPointPrefab);
            }
        }

        public void OnDamaged(Health health, HitInfo hitInfo)
        {
            for (var i = 0; i < hitInfo.damagePoint.count; i++)
            {
                transformContainer.DestroyLast();
            }
        }

        public void OnKilled(Health health)
        {
            transformContainer.Clear();
        }
    }
}