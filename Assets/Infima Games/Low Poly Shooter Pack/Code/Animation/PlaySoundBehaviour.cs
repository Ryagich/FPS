using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class PlaySoundBehaviour : StateMachineBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioSettings settings = new AudioSettings(1.0f, 0.0f);

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioManager.Instance.PlaySound(clip, AudioSourceType.Player);
            //, settings);
        }
    }
}