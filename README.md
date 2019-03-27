# SAction - Unity's Coroutines made simpler!

SAction is a Unity C# script that wraps Unity's Coroutines, and easily allows developers to create and manage actions over time.

## Features
1. Easy to use: just create a new SAction, subscribe to callbacks and you're done.
2. You can use lambdas or standard functions to specify actions to perform.
3. The duration and callbacks of SAction can be changed on the fly.
4. Any SAction can be paused, resumed or stopped at your will.
6. You have access to the SAction state (e.g., HasStarted, IsPaused, HasEnded).
7. More to come

## Get started

### Set up
To use SAction, simply download the SAction.cs file and add it to your Unity assets folder.

### How to use it

#### 1. Creating the SAction
The first thing to do is to create a SAction object, as below:
```
SAction _action = new SAction(this);
```

___Note: SAction needs a MonoBehaviour reference to manage Coroutines. In that example, "this" is the GameObject the script is attached to, but it can be any kind of MonoBehaviour.___

Now you need to specify how long the SAction will take to fully complete the action. By default, the duration is set to 1 second.

```
_action.Duration = 2;
```

#### 2. Registering actions
Now that your SAction is created, you need to register actions to callbacks. Those callbacks will then be automatically called by the SAction to perform the associated action. 

Here is a list of available callbacks:
1. OnStart: ___void___
2. OnPause: ___void___
3. OnResume: ___void___
4. OnStop: ___void___
5. OnEnd: ___void___
6. OverTime: ___float [0,1]___

___Note: OverTime callback is the only callback returning a value, which represents the progression of the Coroutine.___

To register an action to a callback you can either use lambda or standard functions:
```
// using Lamba
_action.OverTime = (progress) => { Debug.Log("The SAction is completed at " + (progress * 100) + "%"); };
_action.OnEnd = () => { Debug.Log("The SAction has ended"); };

// using function
_action.OverTime = ActionToPerformOverTime;
_action.OnEnd = ActionToPerformOnEnd;

[...]

void ActionToPerformOverTime(float progress)
{
	Debug.Log("The SAction is completed at " + (progress * 100) + "%");
}

void ActionToPerformOnEnd()
{
	Debug.Log("The SAction has ended");
}
```

#### 3. Controlling the SAction
Now that our SAction is created and all desired actions are registered to the appropriate callback, it's time to start the SAction!

```
_action.Start();
```

It's that simple. 

Usually SActions are "Fire-and-forget", meaning you only have to start it once to do the job. However you can control the SAction to perform various task. 

Here is a list of available control functions:
1. Start()
2. Pause()
3. Resume()
4. Stop()

___Note: you can restart a SAction by using Start(). This will first automatically stop the SAction, then start back from zero.___

You can also get various informations about the SAction, such as the current progress (even outside the Action itself), or is current state.

Here is a list of available control attributes:
1. Progress: ___float [0,1]___
2. HasStarted: ___bool___
3. IsPaused: ___bool___
4. HasEnded: ___bool___

## Example
Here is a full example, based on the __Get started__ section:

```
private SAction _action;

void Start ()
{
    _action = new SAction(this);
    _action.Duration = 2;
    _action.OverTime = ActionToPerformOverTime;
    _action.OnEnd = ActionToPerformOnEnd;
  	_action.Start();
}

void ActionToPerformOverTime(float progress)
{
	Debug.Log("The SAction is completed at " + (progress * 100) + "%");
}

void ActionToPerformOnEnd()
{
	Debug.Log("The SAction has ended");
}
```
You can also shorten the code above:
```
private SAction _action;

void Start ()
{
    _action = new SAction(this)
    {
        Duration = 2,
        OverTime = (progress) => { Debug.Log("The SAction is completed at " + (progress * 100) + "%"); },
        OnEnd = () => { Debug.Log("The SAction has ended"); }
    };
  	_action.Start();
}
```

## Use cases
I first created SAction to simplify redundant task I've had in most of my previous projects, such as fading screens, controlling Vector3 interpolation, fading volume and so on. Now I'm using SAction in every new project I create and I still continue finding new uses of this great tool, such as:
+ Fading screens
+ Fading audio volume
+ Vector3 interpolation
+ Background parallax scrolling
+ Changing HSV color
+ Speed and inertia controlling
+ Progress bar
+ Smooth score display incrementation
+ Animations
+ And much more...
