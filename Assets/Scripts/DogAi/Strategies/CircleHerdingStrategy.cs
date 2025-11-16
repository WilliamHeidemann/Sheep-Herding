using System.Linq;
using Models;
using UnityEngine;
using Utility.ExtensionMethods;

namespace DogAi.Strategies
{
    public class CircleHerdingStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Pen _pen;
        private readonly DogConfiguration _dogConfiguration;

        public CircleHerdingStrategy(Transform dog, Transform[] sheep, Pen pen, DogConfiguration dogConfiguration)
        {
            _dog = dog;
            _sheep = sheep;
            _pen = pen;
            _dogConfiguration = dogConfiguration;
        }

        public void Execute()
        {
            float furthestDistanceToPen = _sheep
                .Select(transform => Vector3.Distance(transform.position, _pen.CenterOfPen.position))
                .Max();

            float circleX = _pen.CenterOfPen.position.x + furthestDistanceToPen * Mathf.Cos(Time.time) * 1.2f;
            float circleZ = _pen.CenterOfPen.position.z + furthestDistanceToPen * Mathf.Sin(Time.time) * 1.2f;
            Vector3 targetPosition = new(circleX, _dog.position.y, circleZ);

            _dog.MoveTowards(targetPosition, _dogConfiguration.MoveSpeed, _dogConfiguration.TurnSpeed);
            
            Debug.DrawLine(_dog.position, targetPosition, Color.black);
        }
    }
}