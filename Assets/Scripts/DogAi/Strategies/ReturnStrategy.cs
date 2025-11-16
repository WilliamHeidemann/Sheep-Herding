using Models;
using UnityEngine;
using Utility;

namespace DogAi.Strategies
{
    public class ReturnStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform _man;
        private readonly DogConfiguration _dogConfiguration;

        public ReturnStrategy(Transform dog, Transform man, DogConfiguration dogConfiguration)
        {
            _dog = dog;
            _man = man;
            _dogConfiguration = dogConfiguration;
        }

        public void Execute()
        {
            Vector3 rightOfMan = _man.position + _man.right * 2f;
            
            if (Vector3.Distance(_dog.position, rightOfMan) < 1f)
            {
                _dog.RotateTowards(_man.rotation, _dogConfiguration.TurnSpeed);
            }
            else
            {
                _dog.MoveTowards(rightOfMan, _dogConfiguration.MoveSpeed, _dogConfiguration.TurnSpeed);
            }
            
            Debug.DrawLine(_dog.position, rightOfMan, Color.black);
        }
    }
}