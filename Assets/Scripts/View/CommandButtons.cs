using DogAi;
using UnityEngine;
using UnityEngine.UIElements;

namespace View
{
    public class CommandButtons : MonoBehaviour
    {
        private UIDocument _uiDocument;
        [SerializeField] private CommandRegistry _commandRegistry;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            var root = _uiDocument.rootVisualElement;

            var buttons = root.Query<Button>().ToList();
            buttons[0].clicked += ClockWise;
            buttons[1].clicked += CounterClockWise;
            buttons[2].clicked += ChaseSheep;
            buttons[3].clicked += LieDown;
        }
        
        private void ClockWise()
        {
            _commandRegistry.IssueCommand(Command.ClockWise);
        }

        private void CounterClockWise()
        {
            _commandRegistry.IssueCommand(Command.CounterClockWise);
        }

        private void ChaseSheep()
        {
            _commandRegistry.IssueCommand(Command.ChaseSheep);
        }
        
        private void LieDown()
        {
            _commandRegistry.IssueCommand(Command.LieDown);
        }
    }
}
