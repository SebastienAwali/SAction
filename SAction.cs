// Copyright (c) 2018 Sébastien AWALI

using System;
using System.Collections;
using UnityEngine;

public class SAction
{
    #region Public properties
    /// <summary>
    /// The total duration of the action in seconds. Default is 1 second.
    /// </summary>
    public float Duration = 1;
    /// <summary>
    /// The action to perform when the SAction starts.
    /// </summary>
    public Action OnStart = null;
    /// <summary>
    /// The action to perform over time.
    /// </summary>
    public Action<float> OverTime = null;
    /// <summary>
    /// The action to perform when the SAction ends.
    /// </summary>
    public Action OnEnd = null;
    /// <summary>
    /// The action to perform when the SAction is paused.
    /// </summary>
    public Action OnPause = null;
    /// <summary>
    /// The action to perform when the SAction is resumed.
    /// </summary>
    public Action OnResume = null;
    /// <summary>
    /// The action to perform when the SAction is stopped.
    /// </summary>
    public Action OnStop = null;
    /// <summary>
    /// The current progress of the SAction.
    /// </summary>
    public float Progress { get; private set; }
    /// <summary>
    /// Tells if the SAction is paused.
    /// </summary>
    public bool IsPaused { get; private set; }
    /// <summary>
    /// Tells if the SAction has started.
    /// </summary>
    public bool HasStarted { get; private set; }
    /// <summary>
    /// Tells if the SAction has ended.
    /// </summary>
    public bool HasEnded { get; private set; }
    #endregion

    #region Private properties
    /// <summary>
    /// The controller is a MonoBehaviour that's used by the SAction to start and stop Unity's Coroutines.
    /// </summary>
    private MonoBehaviour _controller = null;
    /// <summary>
    /// The coroutine used to do the action over time.
    /// </summary>
    private Coroutine _coroutine = null;
    #endregion

    public SAction(MonoBehaviour controller)
    {
        _controller = controller;
        Initialize();
    }

    #region Public methods
    /// <summary>
    /// Start or restart the SAction. Make sure you've at least given a Controller.
    /// </summary>
    public void Start()
    {
        // If there is no controller, we don't continue
        if(!HasController())
            return;

        // Stop the previous coroutine if there was one alive
        StopCoroutine();

        // Callback for start action
        if (OnStart != null)
            OnStart.Invoke();

        HasStarted = true;

        StartCoroutine();
    }

    /// <summary>
    /// Temporary pause the SAction and can be resumed with Resume();
    /// </summary>
    public void Pause()
    {
        // If the SAction is already paused, we don't continue
        if (IsPaused)
        {
            Debug.LogWarning("SAction - Pause() was called on an already paused SAction!");
            return;
        }
        
        StopCoroutine();
        IsPaused = true;

        // Callback for pause action
        if (OnPause != null)
            OnPause.Invoke();
    }

    /// <summary>
    /// Resume the SAction after a pause.
    /// </summary>
    public void Resume()
    {
        // If the SAction wasn't paused, we notify the user
        if (!IsPaused)
        {
            Debug.LogWarning("SAction - Resume() was called on a not paused SAction!");
            return;
        }

        // Callback for resume action
        if (OnResume != null)
            OnResume.Invoke();

        IsPaused = false;
        StartCoroutine();
    }

    /// <summary>
    /// Stop the SAction and set it back to the default state.
    /// </summary>
    public void Stop()
    {
        StopCoroutine();

        // Callback for stop action
        if (OnStop != null)
            OnStop.Invoke();

        // Set default values
        Initialize();
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Make sure the SAction has a Controller.
    /// </summary>
    /// <returns>True if the SAction has a Controller, False otherwise.</returns>
    private bool HasController()
    {
        if (_controller == null)
        {
            Debug.LogError("SAction - This SAction doesn't have a Controller!");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sets default values.
    /// </summary>
    private void Initialize()
    {
        Progress = 0;
        IsPaused = false;
        HasStarted = false;
        HasEnded = false;
    }

    /// <summary>
    /// Start the coroutine.
    /// </summary>
    private void StartCoroutine()
    {
        // If there is no controller, we don't continue
        if (!HasController())
            return;

        // Create coroutine to perform the action over time
        _coroutine = _controller.StartCoroutine(Coroutine());
    }

    /// <summary>
    /// Stop the running coroutine.
    /// </summary>
    private void StopCoroutine()
    {
        // If there is no controller, we don't continue
        if (!HasController())
            return;

        // Stop the coroutine
        if (_coroutine != null)
        {
            _controller.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator Coroutine()
    {
        float percent = Progress;

        // Make sure Duration > 0 to avoid div by 0, or negative percentage
        if (Duration <= 0)
            percent = 1;    // Setting percent to 1 skip the while loop

        if (OverTime != null)
            OverTime.Invoke(percent);

        while (percent < 1f)
        {
            percent += Time.deltaTime * (1 / Duration);
            percent = Mathf.Clamp(percent, 0, 1);
            Progress = percent;

            if (OverTime != null)
                OverTime.Invoke(percent);

            yield return null;
        }

        if (OnEnd != null)
            OnEnd.Invoke();

        HasEnded = true;
    }
    #endregion
}
