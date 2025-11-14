using Models;
using UnityEngine;
using Utility;

namespace DogAi.Strategies
{
    public class ChaseStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Pen _pen;
        private readonly float _dogMoveSpeed;
        private readonly float _dogTurnSpeed;

        public ChaseStrategy(Transform dog, Transform[] sheep, Pen pen, float dogMoveSpeed, float dogTurnSpeed)
        {
            _dog = dog;
            _sheep = sheep;
            _pen = pen;
            _dogMoveSpeed = dogMoveSpeed;
            _dogTurnSpeed = dogTurnSpeed;
        }

        public void Execute()
        {
            var furthestFromPen = _sheep
                .MaxBy(sheep => Vector3.Distance(sheep.position, _pen.CenterOfPen.position));
            
            var offset = (furthestFromPen.position - _pen.CenterOfPen.position).normalized * 2f;
            
            var targetPosition = furthestFromPen.position + offset;
            
            Debug.DrawLine(_dog.position, targetPosition, Color.black);
            _dog.MoveTowards(targetPosition, _dogMoveSpeed, _dogTurnSpeed);
        }
    }
}