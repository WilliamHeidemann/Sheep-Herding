using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Commands", menuName = "Scriptable Objects/Commands")]
public class CommandRegistry : ScriptableObject
{
    private Action<Command> _onCommandIssued;
    
    public void IssueCommand(Command command)
    {
        _onCommandIssued?.Invoke(command);
    }
    
    public void Subscribe(Action<Command> listener)
    {
        _onCommandIssued += listener;
    }
    
    public void Unsubscribe(Action<Command> listener)
    {
        _onCommandIssued -= listener;
    }
}

public enum Command
{
    ClockWise,
    CounterClockWise,
    ChaseSheep,
    LieDown
}