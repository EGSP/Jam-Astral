using System;
using Egsp.Core;
using Egsp.Extensions.Primitives;
using UnityEngine;

namespace Game.Obstacles
{
    public class Spike : BoxTrigger2D
    {
        public static readonly Color SpikeColor = Color.red;  
        
        [SerializeField] private float punchPower;

        protected override void OnEnter(GameObject enteredObject)
        {
            var physicsEntity = enteredObject.GetComponent<IPhysicsEntity>();
            
            if (physicsEntity != null)
            {
                physicsEntity.ApplyForceFrom(enteredObject.transform, transform, punchPower);
            }
        }

        protected override Color GetGizmosColor() => SpikeColor;
    }
}