/**
 * Alexandre Ouellet - 9 octobre 2022
 */
using System.Collections;
using UnityEngine;

/// <summary>
/// State machine controlling a object subclass of Monobehaviour using a system of commands.
/// State machine are executed every frame as a coroutine of the objet they control.
/// </summary>
/// <typeparam name="T">Any subclass of MonoBehaviour.</typeparam>
public class StateMachine<T> where T : MonoBehaviour
{
    private IState<T> currentState;             // Current state of the machine
    
    private bool stateChanged;                  // Has state change since last frame

    private readonly T controlledObject;        // The object controlled by the state machine

    public bool IsRunning { get; private set; } // Indicates if the state machine is running (as been started and not stopped)

    public bool IsPaused { get; private set; }  // Indicates if the state machine is on pause.

    /// <summary>
    /// Creates a new state machine with a specified objet to control.
    /// </summary>
    /// <param name="controlledObject">The object controlled by the state machine.</param>
    public StateMachine(T controlledObject)
    {
        stateChanged = false;
        IsRunning = false;
        IsPaused = false;
        this.controlledObject = controlledObject;
    }

    /// <summary>
    /// Starts the execution of the state machine.
    /// </summary>
    /// <param name="startingState">The initial state of the machine.</param>
    public void Start(IState<T> startingState)
    {
        currentState = startingState;
        stateChanged = true;
        IsRunning = true;
        controlledObject.StartCoroutine(Run());
    }

    /// <summary>
    /// Interrupts the execution of the state machine. All internal state are kept intact during the pause.
    /// </summary>
    public void Pause()
    {
        IsPaused = true;
    }

    /// <summary>
    /// Resumes the execution of a paused state machine.
    /// </summary>
    public void Resume()
    {
        IsPaused = false;
    }

    /// <summary>
    /// Stops the state machine. Internal states may not be retreive after a call to stop.
    /// </summary>
    public void Stop()
    {
        IsRunning = false;
    }

    /// <summary>
    /// Coroutine that handles the execution of the state machine.
    /// </summary>
    /// <returns>Enumerator to distribute execution over multiple frames.</returns>
    private IEnumerator Run()
    {
        while(IsRunning)
        {
            if(!IsPaused)
            {
                ExecuteState();
            }
            yield return null;
        }
    }

    /// <summary>
    /// Executes the current state and verify if there needs to be a state transition in the next frame.
    /// </summary>
    private void ExecuteState()
    {
        if (stateChanged)
        {
            ExecuteCommands(currentState.OnStateEnter(controlledObject));
            stateChanged = false;
        }

        ExecuteCommands(currentState.OnStateStay(controlledObject, out IState<T> nextState));

        if (nextState != currentState)
        {
            ExecuteCommands(currentState.OnStateExit(controlledObject));
            currentState = nextState;
            stateChanged = true;
        }
    }

    /// <summary>
    /// Execute a list of commands provided on the controlled object.
    /// </summary>
    /// <param name="commandArray">The array of commands to execute.</param>
    private void ExecuteCommands(AbstractCommand<T>[] commandArray)
    {
        if (commandArray != null)
        {
            foreach (AbstractCommand<T> command in commandArray)
            {
                command.Execute(controlledObject);
            }
        }
    }
}
