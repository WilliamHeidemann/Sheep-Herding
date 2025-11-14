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
        private readonly float _dogMoveSpeed;
        private readonly float _dogTurnSpeed;

        public CircleHerdingStrategy(Transform dog, Transform[] sheep, Pen pen, float dogMoveSpeed, float dogTurnSpeed)
        {
            _dog = dog;
            _sheep = sheep;
            _pen = pen;
            _dogMoveSpeed = dogMoveSpeed;
            _dogTurnSpeed = dogTurnSpeed;
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

            _dog.MoveTowards(targetPosition, _dogMoveSpeed, _dogTurnSpeed);
        }
    }
}