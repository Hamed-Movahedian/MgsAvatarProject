using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MgsCommonLib.UI;
using MgsCommonLib.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MgsAvatarEditorWindow : MgsUIWindow
{
    public float TransitionDuration = 1;
    public RectTransform ImageRoot;

    private Image[][] _images;
    private RectTransform[][] _rectTransforms;
    
    #region Instance

    private static MgsAvatarEditorWindow _instance;
    private List<List<int>> _duplicateFeatures;

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

        // Add random value for each feature
        foreach (Transform feature in ImageRoot)
            results.Add(Random.Range(0, feature.childCount));

        // Duplicate features must has same value
        foreach (List<int> duplicateFeature in _duplicateFeatures)
        {
            var min = duplicateFeature.Min(index=>results[index]);

            foreach (var index in duplicateFeature)
                results[index] = min;
        }
        return results;
    }


    #endregion

    #region Start - Cache Images and rectTransforms

    public void Start()
    {
        // Get images of all features as matrix
        _images = ImageRoot
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

        // Extract duplicate features
        _duplicateFeatures = ImageRoot
            .GetChilds()
            .Select((child, index) => new {Index = index, Name = child.name.Split(' ', ',', '_')[0]})
            .GroupBy(pair => pair.Name)
            .Where(g => g.Count() > 1)
            .Select(g => g.Select(pair=>pair.Index).ToList())
            .ToList();
    }

    #endregion

    #region SetFeature

    public void SetFeature(int featureIndex, int featureValue, RectTransform rectTransform, Image image)
    {
        rectTransform.localPosition = _rectTransforms[featureIndex][featureValue].localPosition;
        rectTransform.localScale = _rectTransforms[featureIndex][featureValue].localScale;
        rectTransform.sizeDelta = _rectTransforms[featureIndex][featureValue].sizeDelta;

        image.sprite =_images[featureIndex][featureValue].sprite;
    }

    #endregion

    #region SetFeatureValues

    public void SetFeatureValues(List<int> featureValues)
    {
        // Disable all images
        DisableAllFeatures();

        // **************    Enable specified images
        
        // Get min (in case of error
        var minFeatureIndex = Mathf.Min(featureValues.Count,FeatureCount);

        // Enable specified images
        for (int featureIndex = 0; featureIndex < minFeatureIndex; featureIndex++)
        {
            _images[featureIndex][featureValues[featureIndex]].gameObject.SetActive(true);
        }
    }

    #endregion

    #region DisableAllFeatures

    // disable all images
    private void DisableAllFeatures()
    {
        foreach (var imageCollection in _images)
            foreach (var image in imageCollection)
                image.gameObject.SetActive(false);
    }


    #endregion

    #region GetFeatureValues

    public List<int> GetFeatureValues()
    {
        var featureValues = new List<int>();

        foreach (var featureImages in _images)
        {
            int i = 0;
            while (!featureImages[i].gameObject.activeSelf)
            {
                i++;
            }
            featureValues.Add(i);
        }

        return featureValues;
    }
    
    #endregion
}
