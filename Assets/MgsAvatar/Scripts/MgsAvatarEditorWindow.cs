using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MgsCommonLib.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MgsAvatarEditorWindow : MonoBehaviour
{
    public float TransitionDuration = 1;
    public RectTransform ImageRoot;

    private Image[][] _images;
    private RectTransform[][] _rectTransforms;
    
    #region Instance

    private static MgsAvatarEditorWindow _instance;

    public static MgsAvatarEditorWindow Instance
    {
        get { return _instance ?? FindObjectOfType<MgsAvatarEditorWindow>(); }
    }

    public int FeatureCount { get { return ImageRoot.childCount; } }

    #endregion

    #region GetRandomAvatarValues

    public List<int> GetRandomAvatarValues()
    {
        // initialize results
        var results = new List<int>();

        // ********** Add random value for each feature
        foreach (Transform feature in ImageRoot)
            results.Add(Random.Range(0, feature.childCount));

        return results;
    }


    #endregion

    #region Start - Cache Images and rectTransforms

    public void Start()
    {
        // Get images of all features as matrix
        _images = transform
            .GetChilds()
            .Select(child => child.GetComponentsInChildren<Image>(true))
            .ToArray();

        // Get RectTransform of all images
        _rectTransforms = _images
            .Select(images =>
                images
                    .Select(image => image.GetComponent<RectTransform>())
                    .ToArray())
            .ToArray();
    }

    #endregion

    #region GetFeature

    public void GetFeature(int featureIndex, int featureValue, RectTransform rectTransform, Image image)
    {
        rectTransform.localPosition = _rectTransforms[featureIndex][featureValue].localPosition;
        rectTransform.localScale = _rectTransforms[featureIndex][featureValue].localScale;

        image.sprite =_images[featureIndex][featureValue].sprite;
    }

    #endregion

}
