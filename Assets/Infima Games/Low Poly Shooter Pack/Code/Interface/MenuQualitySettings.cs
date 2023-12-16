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
        #region FIELDS SERIALIZED

        [Title(label: "Settings")] [Tooltip("Canvas to play animations on.")] [SerializeField]
        private GameObject animatedCanvas;

        [Tooltip("Animation played when showing this menu.")] [SerializeField]
        private AnimationClip animationShow;

        [Tooltip("Animation played when hiding this menu.")] [SerializeField]
        private AnimationClip animationHide;

        #endregion

        #region FIELDS

        private Animation animationComponent;
        private bool menuIsEnabled;
        private PostProcessVolume postProcessingVolume;
        private PostProcessVolume postProcessingVolumeScope;
        private DepthOfField depthOfField;

        #endregion

        #region UNITY

        private void Start()
        {
            animatedCanvas.GetComponent<CanvasGroup>().alpha = 0;
            animationComponent = animatedCanvas.GetComponent<Animation>();

            postProcessingVolume = GameObject.Find("Post Processing Volume")?.GetComponent<PostProcessVolume>();
            postProcessingVolumeScope =
                GameObject.Find("Post Processing Volume Scope")?.GetComponent<PostProcessVolume>();

            if (postProcessingVolume != null)
                postProcessingVolume.profile.TryGetSettings(out depthOfField);
        }

        protected override void Tick()
        {
            if (!(characterBehaviour as Character).CanPause)
                return;
            //Switch. Fades in or out the menu based on the cursor's state.
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

        #endregion

        #region METHODS

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

        private void SetPostProcessingState(bool value = true)
        {
            if (postProcessingVolume != null)
                postProcessingVolume.enabled = value;
            if (postProcessingVolumeScope != null)
                postProcessingVolumeScope.enabled = value;
        }

        public void SetQualityVeryLow()
        {
            QualitySettings.SetQualityLevel(0);
            SetPostProcessingState(false);
        }

        public void SetQualityLow()
        {
            QualitySettings.SetQualityLevel(1);
            SetPostProcessingState(false);
        }

        public void SetQualityMedium()
        {
            QualitySettings.SetQualityLevel(2);
            SetPostProcessingState();
        }

        public void SetQualityHigh()
        {
            QualitySettings.SetQualityLevel(3);
            SetPostProcessingState();
        }

        public void SetQualityVeryHigh()
        {
            QualitySettings.SetQualityLevel(4);
            SetPostProcessingState();
        }

        public void SetQualityUltra()
        {
            QualitySettings.SetQualityLevel(5);
            SetPostProcessingState();
        }

        public void Restart()
        {
            var sceneToLoad = SceneManager.GetActiveScene().path;

#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(sceneToLoad,
                new LoadSceneParameters(LoadSceneMode.Single));
#else
            //Load the scene.
            SceneManager.LoadSceneAsync(sceneToLoad, new LoadSceneParameters(LoadSceneMode.Single));
#endif
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}