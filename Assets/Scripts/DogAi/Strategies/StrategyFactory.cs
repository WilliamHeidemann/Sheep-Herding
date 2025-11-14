using System;
using DogAi.Strategies;
using Models;
using UnityEngine;

namespace DogAi
{
    [Serializable]
    public class StrategyFactory
    {
        [SerializeField] private Transform _dog;
        [SerializeField] private Transform _man;
        [SerializeField] private Transform[] _sheep;
        [SerializeField] private Pen _pen;
        [SerializeField] private DogConfig _dogConfig;

        public IHerdingStrategy CreateStrategy(Command command)
        {
            switch (command)
            {
                case Command.Chase:
                    return new ChaseStrategy(_dog, _sheep, _pen, _dogConfig);
                case Command.Circle:
                    return new CircleHerdingStrategy(_dog, _sheep, _pen, _dogConfig);
                case Command.Wait:
                    return new StopStrategy();
                case Command.Return:
                    return new ReturnStrategy(_dog, _man, _dogConfig);
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }
    }
}