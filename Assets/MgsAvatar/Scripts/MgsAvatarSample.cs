using System.Collections;
using System.Collections.Generic;
using MgsCommonLib.Utilities;
using UnityEngine;

public class MgsAvatarSample : MonoBehaviour
{
    [ContextMenu("Initialize")]
    public void Initialize()
    {
        // Cleanup - Destroy all children
        transform.DeleteAllChilds();

        // Get sample rect transform
        var sampleRT = GetComponent<RectTransform>();

        // Get avatar image root
        var imageRootRT = MgsAvatarEditorWindow
            .Instance
            .ImageRoot;

        // Set Sample size
        sampleRT.sizeDelta = imageRootRT.sizeDelta;

        // Copy Images from avatar
        foreach (Transform child in imageRootRT)
        {
            var newRT = Instantiate(child.gameObject).GetComponent<RectTransform>();
            var originalRT = child.GetComponent<RectTransform>();

            newRT.SetParent(transform);

            newRT.localPosition = originalRT.localPosition;
            newRT.localScale = originalRT.localScale;
        }


    }
}
