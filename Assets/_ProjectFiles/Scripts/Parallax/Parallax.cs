using System.Collections.Generic;
using Egsp.Extensions.Primitives;
using Egsp.Other;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public abstract class Parallax : MonoBehaviour, IParallax
    {
        public static readonly Color GizmosColor = new Color(0.960f, 0.878f, 0.152f);
        public static readonly Color ThresholdColor = new Color(0.996f, 0.388f, 0.305f);

        [SerializeField] [Min(0.01f)] protected float width = 1;
        [SerializeField] [Min(0.01f)] protected float thresholdWidth = 1;

        [SerializeField] protected Curve displacementRatioCurve;

        [SerializeField] protected Transform target;

        protected Vector3 oldTargetPosition;
        
        public Transform Target
        {
            protected get => target;
            set
            {
                target = value;

                if (target != null)
                    oldTargetPosition = target.position;
            }
        }

        protected virtual float MinDepth => 0;
        
        protected virtual float MaxDepth => 1;
        
        public virtual Bounds Bounds => 
            new Bounds(transform.position, new Vector3(width, 0));
        
        public virtual Bounds ThresholdBounds => 
            new Bounds(transform.position, new Vector3(width + thresholdWidth, 0));
        
        [NotNull] protected List<IParallaxGroup> ParallaxGroups { get; set; } = new List<IParallaxGroup>();

        public virtual float GetRatio(IParallaxGroup group)
        {
            var ratio = 1 - displacementRatioCurve.Get(group.Depth
                .ToNormalized(MinDepth, MaxDepth));

            return ratio;
        }

        public virtual void Add(IParallaxGroup group)
        {
            AddGroupInternal(group);
        }

        /// <summary>
        /// Добавление группы, без каких-либо операций.
        /// </summary>
        protected void AddGroupInternal(IParallaxGroup group)
        {
            ParallaxGroups.Add(group);
            group.Parallax = this;
        }

        protected virtual void OnEnable()
        {
            FindGroups();
        }

        protected virtual void Update()
        {
            if (Target == null)
                return;
            
            MoveGroups(-CalculateDifference());

            oldTargetPosition = target.position;
        }
        
        /// <summary>
        /// Поиск групп в дочерних объектах.
        /// </summary>
        public virtual void FindGroups()
        {
            var groups = GetComponentsInChildren<IParallaxGroup>(true);

            for (var i = 0; i < groups.Length; i++)
            {
                AddGroupInternal(groups[i]);
            }
        }

        protected void MoveGroups(Vector3 difference)
        {
            for (var i = 0; i < ParallaxGroups.Count; i++)
            {
                var group = ParallaxGroups[i];
                group.Move(difference);
            }
        }

        protected abstract Vector3 CalculateDifference();
        

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = ThresholdColor;
            Gizmos.DrawLine(ThresholdBounds.min,
                ThresholdBounds.max);

            Gizmos.color = GizmosColor;
            Gizmos.DrawLine(Bounds.min, Bounds.max);
        }
    }
}