using System.Collections;
using System.Collections.Generic;
using MgsCommonLib.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class MgsUIToggle : MonoBehaviour , ISelectHandler
{
    public bool IsOn = false;

    private List<MgsUIToggle> _siblingToggles;

	void Start ()
	{
	    // get sibling toggles
	    _siblingToggles = this.GetComponentInSibling<MgsUIToggle>();

	}

    public void OnSelect(BaseEventData eventData)
    {
        // if it's already on do nothing
        if(IsOn)
            return;

        // set this toggle on
        SetOn();

        // set sibling toggles off
        _siblingToggles.ForEach(st =>
        {
            if (st.IsOn)
                st.SetOff();
        });
    }

    private void SetOn()
    {
        IsOn = true;
    }

    private void SetOff()
    {
        IsOn = false;
    }
}
