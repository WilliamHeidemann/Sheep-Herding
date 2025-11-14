using System;
using Models;
using UnityEngine;
using Utility;

namespace DogAi
{
    public class ReturnStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform _man;
        private readonly DogConfig _dogConfig;

        public ReturnStrategy(Transform dog, Transform man, DogConfig dogConfig)
        {
            _dog = dog;
            _man = man;
            _dogConfig = dogConfig;
        }

        public void Execute()
        {
            var rightOfMan = _man.position + _man.right * 2f;

            Debug.DrawLine(_dog.position, rightOfMan, Color.black);
            
            if (Vector3.Distance(_dog.position, rightOfMan) < 1f)
            {
                _dog.RotateTowards(_man.rotation, _dogConfig.TurnSpeed);
            }
            else
            {
                _dog.MoveTowards(rightOfMan, _dogConfig.MoveSpeed, _dogConfig.TurnSpeed);
            }
        }
    }
}