﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MgsCommonLib.Animation;
using MgsCommonLib.UI;
using MgsCommonLib.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MgsAvatar
{
    public class MgsAvatarFeatureButton : MgsUIToggle
    {
        public List<Image> Images;
        
        private void Start()
        {
            base.Start();

            if (IsOn)
            {
                // Active feature images
                Images.ForEach(image => image.gameObject.SetActive(true));

                // Deactivate sibling images
                GetSiblingImages().ForEach(image => image.gameObject.SetActive(false));
            }
            OnActivate.AddListener(() => StartCoroutine(ActivateImages()));
        }
        
        private IEnumerator ActivateImages()
        {
            // Get Fade in images
            var fadeInImages = Images;
            List<Image> fadeOutImages = GetSiblingImages();

            // Disable Interaction during animation
            var eventSystem = EventSystem.current;
            eventSystem.enabled = false;

            // Enable FadeIn Images
            fadeInImages.ForEach(fiImage => fiImage.gameObject.SetActive(true));

            // Run fade in/out animation
            yield return MsgAnimation.RunAnimation(
                AvatarWindow.TransitionDuration,
                v =>
                {
                        // Fade out
                        foreach (var fadeOutImage in fadeOutImages)
                            fadeOutImage.color = MgsColorUtility.FadeOut(v);

                        // Fade in
                        foreach (var fadeInImage in fadeInImages)
                            fadeInImage.color = MgsColorUtility.FadeIn(v);
                });

            // Disable Fade out images
            fadeOutImages.ForEach(foImage => foImage.gameObject.SetActive(false));

            // Enable Interaction during animation
            eventSystem.enabled = true;
            
        }

        private List<Image> GetSiblingImages()
        {

            // Get Fade out images
            var fadeOutImages = Images
                .SelectMany(fiImage =>
                        fiImage
                            .transform
                            .parent
                            .GetComponentsInChildren<Image>()       // get sibling images
                            .Where(foImage => foImage != fiImage)   // remove fiImages from foImages
                )
                .ToList();
            return fadeOutImages;
        }

        #region AvatarWindow

        private MgsAvatarEditorWindow _avatarWindow;

        public MgsAvatarEditorWindow AvatarWindow
        {
            get
            {
                if (_avatarWindow == null)
                {
                    _avatarWindow = GetComponentInParent<MgsAvatarEditorWindow>();
                    if (_avatarWindow == null)
                        Debug.LogError("Avatar Editor Not Found !!!");
                }
                return _avatarWindow;
            }
        }
        #endregion

    }
}
