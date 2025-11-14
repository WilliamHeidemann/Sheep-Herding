using Models;
using UnityEngine;
using Utility;

namespace DogAi.Strategies
{
    public class CircleHerdingStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Pen _pen;
        private readonly DogConfig _dogConfig;

        public CircleHerdingStrategy(Transform dog, Transform[] sheep, Pen pen, DogConfig dogConfig)
        {
            _dog = dog;
            _sheep = sheep;
            _pen = pen;
            _dogConfig = dogConfig;
        }

        public void Execute()
        {
            var furthestDistanceToPen = float.MinValue;
            foreach (Transform transform in _sheep)
            {
                float distanceToPen = Vector3.Distance(transform.position, _pen.CenterOfPen.position);
                if (distanceToPen > furthestDistanceToPen)
                {
                    furthestDistanceToPen = distanceToPen;
                }
            }

            var circleX = _pen.CenterOfPen.position.x + furthestDistanceToPen * Mathf.Cos(Time.time) * 1.2f;
            var circleZ = _pen.CenterOfPen.position.z + furthestDistanceToPen * Mathf.Sin(Time.time) * 1.2f;
            var targetPosition = new Vector3(circleX, _dog.position.y, circleZ);

            Debug.DrawLine(_dog.position, targetPosition, Color.black);

            _dog.MoveTowards(targetPosition, _dogConfig.MoveSpeed, _dogConfig.TurnSpeed);
        }
    }
}