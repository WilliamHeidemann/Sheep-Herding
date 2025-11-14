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
        [SerializeField] private Transform[] _sheep;
        [SerializeField] private Pen _pen;
        [SerializeField] private DogConfig _dogConfig;
        

        public IHerdingStrategy CreateStrategy(Command command)
        {
            switch (command)
            {
                case Command.CounterClockWise:
                    return new CircleHerdingStrategy(_dog, _sheep, _pen, _dogConfig);
                case Command.ClockWise:
                    return new CircleHerdingStrategy(_dog, _sheep, _pen, _dogConfig);
                case Command.ChaseSheep:
                    return new ChaseStrategy(_dog, _sheep, _pen, _dogConfig);
                case Command.LieDown:
                    return new StopStrategy();
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }
    }
}