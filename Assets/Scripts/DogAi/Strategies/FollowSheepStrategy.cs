using System;
using Models;
using UnityEngine;
using Utility.ExtensionMethods;

namespace DogAi.Strategies
{
    public class FollowSheepStrategy : IHerdingStrategy
    {
        private Transform _dog;
        private Transform _sheep;
        private DogConfiguration _dogConfiguration;
        
        public FollowSheepStrategy(Transform dog, Transform sheep, DogConfiguration dogConfiguration)
        {
            _dog = dog;
            _sheep = sheep;
            _dogConfiguration = dogConfiguration;
        }
        
        public void Execute()
        {
            float slowSpeed = _dogConfiguration.BaseMoveSpeed * 0.2f;
            _dog.MoveTowards(_sheep.position, slowSpeed, 1f);
        }
    }
}