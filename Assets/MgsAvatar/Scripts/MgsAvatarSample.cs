using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MgsCommonLib.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MgsAvatarSample : MonoBehaviour
{
    private List<RectTransform> _rectTransforms;
    private List<Image> _images;
    private MgsAvatarEditorWindow _avatarWindow;

    #region Editor tools

    #region Initialize 

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
            // Copy child 
            GameObject copyGo = Instantiate(child.gameObject);

            // Correct name - remove "(Clone)" part
            copyGo.name = child.name;

            // Get rect transforms
            var copyRT = copyGo.GetComponent<RectTransform>();
            var originalRT = child.GetComponent<RectTransform>();

            // Set p
            copyRT.SetParent(transform);

            // Set rect transform of copy to original
            copyRT.localPosition = originalRT.localPosition;
            copyRT.localScale = originalRT.localScale;

            // Delete Extra children so that only one child remains
            while (copyRT.childCount>1)
            {
                //delete first child
                DestroyImmediate(copyRT.GetChild(0).gameObject);
            }

            // Enable the only remaining child
            copyRT.GetChild(0).gameObject.SetActive(true);

            // Set the only child name to name of his parent
            copyRT.GetChild(0).name = copyRT.name;
        }


    }
    #endregion

    #region Set Random avatar

    [ContextMenu("Random Avatar")]
    public void RandomAvatar()
    {
        // Initialize sample and avatar window
        MgsAvatarEditorWindow.Instance.Start();
        Start();

        // Get random avatar values from editor
        var avatarValues = MgsAvatarEditorWindow
            .Instance
            .GetRandomAvatarValues();

        // Set avatar values
        SetFeatureValues(avatarValues);
    }

    #endregion

    #endregion


    #region Start

    void Start()
    {
        //******************   Initialize images and rectTransforms

        // Get RectTransform
        _rectTransforms = transform
            .GetChilds()                                                    // get first childs
            .Select(child=>child.GetChild(0).GetComponent<RectTransform>()) // get rectTransform of first child
            .ToList();

        // Get images
        _images = transform
            .GetChilds()    // get first childs
            .Select(child => child.GetChild(0).GetComponent<Image>()) // get rectTransform of first child
            .ToList();

        // *************** check Sample validation
        if (_images.Count != _avatarWindow.FeatureCount)
        {
            Debug.LogError("Avatar sample " + name + " is Invalid !! (Initialization required)");
            return;
        }

        // **********************  Cache Avatar window image root
        _avatarWindow = MgsAvatarEditorWindow.Instance;
    }

    #endregion

    #region SetAvatarValues

    public void SetFeatureValues(List<int> featureValues)
    {
        for (int i = 0; i < _avatarWindow.FeatureCount; i++)
            _avatarWindow.GetFeature(i, featureValues[i], _rectTransforms[i], _images[i]);
    }


    #endregion

}
