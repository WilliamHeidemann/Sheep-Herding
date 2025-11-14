using System;
using Models;
using UnityEngine;
using Utility;

namespace DogAi.Strategies
{
    [Serializable]
    public class ChaseStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Pen _pen;
        private readonly DogConfig _dogConfig;

        public ChaseStrategy(Transform dog, Transform[] sheep, Pen pen, DogConfig dogConfig)
        {
            _dog = dog;
            _sheep = sheep;
            _pen = pen;
            _dogConfig = dogConfig;
        }

        public void Execute()
        {
            var furthestFromPen = _sheep
                .MaxBy(sheep => Vector3.Distance(sheep.position, _pen.CenterOfPen.position));
            
            var offset = (furthestFromPen.position - _pen.CenterOfPen.position).normalized * 2f;
            
            var targetPosition = furthestFromPen.position + offset;
            
            Debug.DrawLine(_dog.position, targetPosition, Color.black);
            
            if (Vector3.Distance(_dog.position, targetPosition) < 1f)
            {
                Vector3 targetRotation = furthestFromPen.position - _dog.position;
                _dog.RotateTowards(targetRotation, _dogConfig.TurnSpeed);
            }
            else
            {
                _dog.MoveTowards(targetPosition, _dogConfig.MoveSpeed, _dogConfig.TurnSpeed);
            }
        }
    }
}