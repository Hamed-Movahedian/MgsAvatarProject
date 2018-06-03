using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgsAvatarEditorWindow : MonoBehaviour
{
    public float TransitionDuration = 1;
    public RectTransform ImageRoot;


    #region Instance

    private static MgsAvatarEditorWindow _instance;

    public static MgsAvatarEditorWindow Instance
    {
        get { return _instance ?? FindObjectOfType<MgsAvatarEditorWindow>(); }
    }

    #endregion

}
