using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.UI.Popup
{
    public abstract class PopupBase : MonoBehaviour
    {
	    protected virtual void OnEnable()
	    {
		    Enter();
	    }

	    protected virtual void Enter()
	    {
		    
	    }

	    protected virtual void Exit()
	    {
		    
	    }
    }
}
