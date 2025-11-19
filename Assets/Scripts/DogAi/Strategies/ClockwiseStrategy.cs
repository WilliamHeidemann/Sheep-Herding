using System.Linq;
using Models;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Utility.ExtensionMethods;

namespace DogAi.Strategies
{
    public class ClockwiseStrategy : IHerdingStrategy
    {
        private readonly Transform _dog;
        private readonly Transform[] _sheep;
        private readonly Transform _man;
        private readonly DogConfiguration _dogConfiguration;

        public ClockwiseStrategy(Transform dog, Transform[] sheep, Transform man, DogConfiguration dogConfiguration)
        {
            _dog = dog;
            _sheep = sheep;
            _man = man;
            _dogConfiguration = dogConfiguration;
        }

        public void Execute()
        {
            var balancedPoint = _man.position.BalancedPoint(_sheep.Select(s => s.position).ToArray());
        }
    }
}