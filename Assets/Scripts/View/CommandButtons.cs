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
            VisualElement root = _uiDocument.rootVisualElement;

            var buttons = root.Query<Button>().ToList();
            buttons[0].clicked += ButtonOne;
            buttons[1].clicked += ButtonTwo;
            buttons[2].clicked += ButtonThree;
            buttons[3].clicked += ButtonFour;
        }
        
        private void ButtonOne()
        {
            _commandRegistry.IssueCommand(Command.Circle);
        }

        private void ButtonTwo()
        {
            _commandRegistry.IssueCommand(Command.Chase);
        }

        private void ButtonThree()
        {
            _commandRegistry.IssueCommand(Command.Wait);
        }
        
        private void ButtonFour()
        {
            _commandRegistry.IssueCommand(Command.Return);
        }
    }
}
