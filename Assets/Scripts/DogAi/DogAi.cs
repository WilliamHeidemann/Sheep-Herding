using DogAi.Strategies;
using UnityEngine;

namespace DogAi
{
    public class DogAi : MonoBehaviour
    {
        [SerializeField] private CommandRegistry _commandRegistry;
        [SerializeField] private StrategyFactory _strategyFactory;

        public IHerdingStrategy HerdingStrategy;

        private void Awake()
        {
            HerdingStrategy = _strategyFactory.CreateStrategy(Command.ChaseSheep);
            _commandRegistry.Subscribe(ReadCommand);
        }

        private void ReadCommand(Command command)
        {
            HerdingStrategy = _strategyFactory.CreateStrategy(command);
        }

        private void Update()
        {
            HerdingStrategy.Execute();
        }
    
        private void OnDestroy()
        {
            _commandRegistry.Unsubscribe(ReadCommand);
        }
    }
}