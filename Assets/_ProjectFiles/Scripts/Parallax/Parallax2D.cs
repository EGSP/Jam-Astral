using UnityEngine;

namespace Game
{
    public class Parallax2D : Parallax
    {
        [SerializeField][Range(0.01f,1)] private float maxDepth = 1;

        protected override float MinDepth => 0f;

        protected override float MaxDepth => maxDepth;

        // Не учитываются Y & Z плоскости.
        protected override Vector3 CalculateMoveDelta()
        {
            if (Target == null)
                return oldTargetPosition;

            var difference = Target.position - oldTargetPosition;

            difference.y = 0;
            difference.z = 0;

            return difference;
        }
    }
}