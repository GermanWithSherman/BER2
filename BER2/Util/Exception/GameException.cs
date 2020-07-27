using System.Collections;
using System.Collections.Generic;

public class GameException : System.Exception
{
    public new string Message;

    public GameException(string message)
    {
        Message = message;
    }

    public override string ToString()
    {
        return $"{Message}\n{base.ToString()}";
    }
}

public class ModDependencyException : GameException
{
    public ModDependencyException() : base("ModDependencyException"){}
    public ModDependencyException(string message) : base(message) { }
}