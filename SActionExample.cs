using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SActionExample : MonoBehaviour
{
    private SAction _action;

	void Start ()
	{
		_action = new SAction(this);
		_action.Duration = 2;
		_action.OverTime = ActionToPerformOverTime;
		_action.OnEnd = ActionToPerformOnEnd;
		_action.Start();
		
		// === OR === //
		
		_action = new SAction(this)
		{
			Duration = 2,
			OverTime = (progress) => { Debug.Log("The SAction is completed at " + (progress * 100) + "%"); },
			OnEnd = () => { Debug.Log("The SAction has ended"); }
		};
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
}
