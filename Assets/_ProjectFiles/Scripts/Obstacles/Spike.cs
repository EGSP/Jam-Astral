using System;
using Egsp.Core;
using Egsp.Extensions.Primitives;
using Game.Entities;
using UnityEngine;

namespace Game.Obstacles
{
    public class Spike : BoxTrigger2D
    {
        public static readonly Color SpikeColor = Color.red;  
        
        [SerializeField] private float punchPower;
        [SerializeField] private HitInfo damage;

        protected override void OnEnter(GameObject enteredObject)
        {
            var physicsEntity = enteredObject.GetComponent<IPhysicsEntity>();
            
            if (physicsEntity != null)
            {
                var health = enteredObject.GetComponent<Health>();

                if (health != null)
                    health.Damage(damage);
                
                physicsEntity.ApplyForceFrom(enteredObject.transform, transform, punchPower);
            }
        }

        protected override Color GetGizmosColor() => SpikeColor;
    }
}