using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class Parallax2DAuto : Parallax
    {
        private float minDepth;
        private float maxDepth;

        protected IOrderedEnumerable<IParallaxGroup> cachedGroupsOrder;

        /// <summary>
        /// Сортированная коллекция групп.
        /// </summary>
        [CanBeNull] protected IOrderedEnumerable<IParallaxGroup> OrderedGroups
        {
            get
            {
                if(cachedGroupsOrder == null)
                    CacheGroupsOrder();

                return cachedGroupsOrder;
            }
        }

        protected override float MinDepth => minDepth;

        protected override float MaxDepth => maxDepth;

        protected override Vector3 CalculateMoveDelta()
        {
            if (Target == null)
                return oldTargetPosition;

            var difference = Target.position - oldTargetPosition;

            difference.y = 0;
            difference.z = 0;

            return difference;
        }

        protected void UpdateGroups(Vector3 difference)
        {
            for (var i = 0; i < ParallaxGroups.Count; i++)
            {
                var group = ParallaxGroups[i];
                group.Move(difference);
            }
        }

        /// <summary>
        /// Поиск групп в дочерних объектах.
        /// </summary>
        public override void FindGroups()
        {
            base.FindGroups();
            CacheGroupsOrder();
        }

        /// <summary>
        /// Добавление группы с кешированием.
        /// </summary>
        public override void Add(IParallaxGroup group)
        {
            base.Add(group);
            CacheGroupsOrder();
        }

        /// <summary>
        /// Кеширование очереди групп по глубине.
        /// </summary>
        public void CacheGroupsOrder()
        {
            if (ParallaxGroups.Count == 0)
                return;

            cachedGroupsOrder = ParallaxGroups.OrderBy(x => x.Depth);
            
            CacheDepth();
        }

        /// <summary>
        /// Кеширование минимальной и максимальной глубин.
        /// </summary>
        protected void CacheDepth()
        {
            if (OrderedGroups == null)
                return;

            minDepth = OrderedGroups.Min(x => x.Depth);
            maxDepth = OrderedGroups.Max(x => x.Depth);
        }
    }
}