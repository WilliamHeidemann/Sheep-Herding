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
        [SerializeField] private float _dogMoveSpeed = 1f;
        [SerializeField] private float _dogTurnSpeed = 1f;

        public IHerdingStrategy CreateStrategy(Command command)
        {
            switch (command)
            {
                case Command.ChaseSheep:
                    return new ChaseStrategy(_dog, _sheep, _pen, _dogMoveSpeed, _dogTurnSpeed);
                case Command.ClockWise:
                    return new CircleHerdingStrategy(_dog, _sheep, _pen, _dogMoveSpeed, _dogTurnSpeed);
                case Command.LieDown:
                    return new StopStrategy();
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }
    }
}