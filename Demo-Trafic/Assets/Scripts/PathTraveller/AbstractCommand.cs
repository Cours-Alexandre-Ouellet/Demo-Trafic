/**
 * Alexandre Ouellet - 9 octobre 2022
 */
using UnityEngine;

/// <summary>
/// Implementation of Command paterns where the controlled object is a MonoBehaviour subclass.
/// May implement the Revert action.
/// </summary>
/// <typeparam name="T">Any subclass of MonoBehaviour</typeparam>
public abstract class AbstractCommand<T> where T : MonoBehaviour
{
    /// <summary>
    /// Execute the specified command.
    /// </summary>
    /// <param name="controlledObject">The object on which to perform the command.</param>
    public abstract void Execute(T controlledObject);

    /// <summary>
    /// Undo the command.
    /// </summary>
    /// <param name="controlledObject">The object on which the command was performed.</param>
    public virtual void Revert(T controlledObject)
    {

    }
}
