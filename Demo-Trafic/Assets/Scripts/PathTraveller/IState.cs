/**
 * Alexandre Ouellet - 9 octobre 2022
 */
using UnityEngine;

/// <summary>
/// Defines a StateMachine state
/// </summary>
/// <typeparam name="T">Any subclass of MonoBehaviour</typeparam>
public interface IState<T> where T : MonoBehaviour
{
    /// <summary>
    /// Handles the transition in the state.  Called on the first frame the machine is in this state, right before OnStateStay.
    /// </summary>
    /// <param name="controlledObject">The object controlled by the state machine.</param>
    /// <returns>An array of command to perform on the controlled object.</returns>
    AbstractCommand<T>[] OnStateEnter(T controlledObject);

    /// <summary>
    /// Handles the transition out of the state. Called on the last frame the machine is in this state, right after OnStateStay.
    /// </summary>
    /// <param name="controlledObject">The object controlled by the state machine.</param>
    /// <returns>An array of command to perform on the controlled object.</returns>
    AbstractCommand<T>[] OnStateExit(T controlledObject);

    /// <summary>
    /// Handles the action of staying in the state. Called once per frame while the objet is in this state.
    /// </summary>
    /// <param name="controlledObject">The object controlled by the state machine.</param>
    /// <returns>An array of command to perform on the controlled object.</returns>
    AbstractCommand<T>[] OnStateStay(T controlledObject, out IState<T> nextState);
}
