using UnityEngine;

namespace DogAi
{
    public class DogAi : MonoBehaviour
    {
        [SerializeField] private CommandRegistry _commandRegistry;
        [SerializeField] private StrategyFactory _strategyFactory;

        private IHerdingStrategy _herdingStrategy;

        private void Awake()
        {
            _herdingStrategy = _strategyFactory.CreateStrategy(Command.Return);
            _commandRegistry.Subscribe(ReadCommand);
        }

        private void ReadCommand(Command command)
        {
            _herdingStrategy = _strategyFactory.CreateStrategy(command);
        }

        private void Update()
        {
            _herdingStrategy.Execute();
        }
    
        private void OnDestroy()
        {
            _commandRegistry.Unsubscribe(ReadCommand);
        }
    }
}