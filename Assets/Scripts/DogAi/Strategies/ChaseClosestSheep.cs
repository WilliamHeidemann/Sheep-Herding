using Models;
using UnityEngine;
using Utility.ExtensionMethods;

namespace DogAi.Strategies
{
    public class ChaseClosestSheep : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly DogConfiguration _dogConfiguration;


        public ChaseClosestSheep(Transform dog, Transform[] sheep, DogConfiguration dogConfiguration)
        {
            _dog = dog;
            _sheep = sheep;
            _dogConfiguration = dogConfiguration;
        }

        public void Execute()
        {
            var dogPosition = _dog.position;
            var closestSheep = _sheep
                .MinBy(sheep => Vector3.SqrMagnitude(sheep.position - dogPosition));
            
            _dog.MoveTowards(closestSheep.position, _dogConfiguration.BaseMoveSpeed, _dogConfiguration.BaseTurnSpeed);
        }
    }
}