using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Представление точки в пространстве.
    /// </summary>
    public interface IPoint
    {
        Vector3 Coordinates { get; }
    }

    /// <summary>
    /// Точка равна позиции объекта компонента.
    /// </summary>
    public class TransformPoint : IPoint
    {
        private Transform transform;
        private Vector3 lastPoint;

        public Vector3 Coordinates
        {
            get
            {
                if (transform == null)
                    return lastPoint;
                else
                {
                    lastPoint = transform.position;
                    return lastPoint;
                }
            }
        }

        public TransformPoint([NotNull] Transform transform)
        {
            this.transform = transform;
            lastPoint = transform.position;
        }
    }

    /// <summary>
    /// Точка равна позиции объекта компонента. Однако учитывает особенности двумерного пространства.
    /// </summary>
    public class Transform2DPoint : IPoint
    {
        private readonly float zCoord;
        
        private Transform transform;
        private Vector3 lastPoint;

        public Vector3 Coordinates
        {
            get
            {
                if (transform == null)
                {
                    lastPoint.z = zCoord;
                    return lastPoint;
                }
                else
                {
                    lastPoint = transform.position;
                    lastPoint.z = zCoord;
                    return lastPoint;
                }
            }
        }

        public Transform2DPoint([NotNull] Transform transform, float zCoord = -10f)
        {
            this.transform = transform;
            this.zCoord = zCoord;
            lastPoint = transform.position;
        }
    }

    /// <summary>
    /// Точка является простым вектором.
    /// </summary>
    public class Vector3Point : IPoint
    {
        private readonly Vector3 point;
        public Vector3 Coordinates => point;

        public Vector3Point(Vector3 point)
        {
            this.point = point;
        }
    }
    
    /// <summary>
    /// Компонент камеры следующий за точкой, при ее наличии.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private float timeToTarget;
        [SerializeField] private Vector3 offset;

        [CanBeNull] public IPoint Target { get; set; }

        // Данный способ вычисления скорости/времени передвжиения подходит для Lerp функции.
        // Чем больше времени, тем меньше будет число и медленнее интерполяция.
        private float InversedTime => 1f / timeToTarget;

        // Данный режим обновления выбран из-за того, что игрок движется с помощью физики.
        private void FixedUpdate()
        {
            MoveToPoint();
        }

        private void MoveToPoint()
        {
            if (Target == null)
                return;

            var newPosition = Vector3
                .Lerp(transform.position, Target.Coordinates + offset, InversedTime * Time.deltaTime);

            transform.position = newPosition;
        }
    }
}