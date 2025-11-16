using System;
using Models;
using UnityEngine;
using Utility.ExtensionMethods;

namespace DogAi.Strategies
{
    [Serializable]
    public class ChaseStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Pen _pen;
        private readonly DogConfiguration _dogConfiguration;

        public ChaseStrategy(Transform dog, Transform[] sheep, Pen pen, DogConfiguration dogConfiguration)
        {
            _dog = dog;
            _sheep = sheep;
            _pen = pen;
            _dogConfiguration = dogConfiguration;
        }

        public void Execute()
        {
            Transform furthestFromPen = _sheep
                .MaxBy(sheep => Vector3.Distance(sheep.position, _pen.CenterOfPen.position));
            
            Vector3 offset = (furthestFromPen.position - _pen.CenterOfPen.position).normalized * 2f;
            Vector3 targetPosition = furthestFromPen.position + offset;
            
            if (Vector3.Distance(_dog.position, targetPosition) < 1f)
            {
                Vector3 targetRotation = furthestFromPen.position - _dog.position;
                _dog.RotateTowards(targetRotation, _dogConfiguration.BaseTurnSpeed);
            }
            else
            {
                _dog.MoveTowards(targetPosition, _dogConfiguration.BaseMoveSpeed, _dogConfiguration.BaseTurnSpeed);
            }
            
            Debug.DrawLine(_dog.position, targetPosition, Color.black);
        }
    }
}