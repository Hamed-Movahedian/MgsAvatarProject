using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgsAvatarEditorWindow : MonoBehaviour
{
    public float TransitionDuration = 1;

    #region Canvas Group
    private CanvasGroup _canvasGroup;

    public CanvasGroup CanvasGroup
    {
        get { return _canvasGroup ?? GetComponent<CanvasGroup>(); }
    }
    #endregion

    public void DisableInteraction()
    {
        CanvasGroup.interactable = false;
    }

    public void EnableInteraction()
    {
        CanvasGroup.interactable = true;
    }
}
