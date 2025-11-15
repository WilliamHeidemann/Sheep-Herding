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
            return command switch
            {
                Command.Chase => new ChaseStrategy(_dog, _sheep, _pen, _dogConfig),
                Command.Circle => new CircleHerdingStrategy(_dog, _sheep, _pen, _dogConfig),
                Command.Wait => new StopStrategy(),
                Command.Return => new ReturnStrategy(_dog, _man, _dogConfig),
                _ => throw new System.ArgumentOutOfRangeException(nameof(command), command, null)
            };
        }
    }
}