using System;
using DogAi.Strategies;
using Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace DogAi
{
    [Serializable]
    public class StrategyFactory
    {
        [SerializeField] private Transform _dog;
        [SerializeField] private Transform _man;
        [SerializeField] private Transform[] _sheep;
        [SerializeField] private Pen _pen;
        [SerializeField] private DogConfiguration _dogConfiguration;

        public IHerdingStrategy CreateStrategy(Command command)
        {
            return command switch
            {
                Command.Chase => new ChaseStrategy(_dog, _sheep, _pen, _dogConfiguration),
                Command.Circle => new CircleHerdingStrategy(_dog, _sheep, _pen, _dogConfiguration),
                Command.Wait => new StopStrategy(),
                Command.Return => new ReturnStrategy(_dog, _man, _dogConfiguration),
                Command.Clockwise => new ArcStrategy(_dog, _sheep, _man, _dogConfiguration, true),
                Command.CounterClockwise => new ArcStrategy(_dog, _sheep, _man, _dogConfiguration, false),
                Command.ChaseClosestSheep => new ChaseClosestSheep(_dog, _sheep, _dogConfiguration),
                _ => throw new System.ArgumentOutOfRangeException(nameof(command), command, null)
            };
        }
    }
}