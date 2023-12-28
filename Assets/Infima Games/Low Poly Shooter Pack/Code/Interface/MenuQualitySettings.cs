//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Quality Settings Menu.
    /// </summary>
    public class MenuQualitySettings : Element
    {
        [Title(label: "Settings")] [Tooltip("Canvas to play animations on.")] 
        [SerializeField] private GameObject animatedCanvas;
        [Tooltip("Animation played when showing this menu.")]
        [SerializeField] private AnimationClip animationShow;
        [Tooltip("Animation played when hiding this menu.")] 
        [SerializeField] private AnimationClip animationHide;
        
        private Animation animationComponent;
        private bool menuIsEnabled;
        private PostProcessVolume postProcessingVolume;
        private DepthOfField depthOfField;

        private void Start()
        {
            animatedCanvas.GetComponent<CanvasGroup>().alpha = 0;
            animationComponent = animatedCanvas.GetComponent<Animation>();

            postProcessingVolume = GameObject.Find("Post Processing Volume")?.GetComponent<PostProcessVolume>();
            if (postProcessingVolume != null)
                postProcessingVolume.profile.TryGetSettings(out depthOfField);
        }

        protected override void Tick()
        {
            if (!(characterBehaviour as Character).CanPause)
                return;
            var cursorLocked = characterBehaviour.IsCursorLocked();
            switch (cursorLocked)
            {
                case true when menuIsEnabled:
                    Hide();
                    break;
                case false when !menuIsEnabled:
                    Show();
                    break;
            }
        }

        private void Show()
        {
            menuIsEnabled = true;

            animationComponent.clip = animationShow;
            animationComponent.Play();

            if (depthOfField != null)
                depthOfField.active = true;
        }

        public void Hide()
        {
            menuIsEnabled = false;

            animationComponent.clip = animationHide;
            animationComponent.Play();

            if (depthOfField != null)
                depthOfField.active = false;
        }
    }
}