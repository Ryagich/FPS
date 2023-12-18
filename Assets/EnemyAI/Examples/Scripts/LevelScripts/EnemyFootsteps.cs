using UnityEngine;
using AudioSettings = InfimaGames.LowPolyShooterPack.AudioSettings;

// This class is created for the example scene. There is no support for this script.
public class EnemyFootsteps : MonoBehaviour
{
    public AudioClip[] stepClips;

    private int index;
    private Animator anim;
    private bool isLeftFootAhead;
    private bool playedLeftFoot;
    private bool playedRightFoot;
    private Vector3 leftFootIKPos;
    private Vector3 rightFootIKPos;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        var factor = 0.15f;

        if (anim.velocity.magnitude > 1.4f)
        {
            // Distance between the pivot position and the left foot
            if (Vector3.Distance(leftFootIKPos, anim.pivotPosition) <= factor && playedLeftFoot == false)
            {
                PlayFootStep();
                playedLeftFoot = true;
                playedRightFoot = false;
            }

            // Distance between the pivot position and the right foot
            if (Vector3.Distance(rightFootIKPos, anim.pivotPosition) <= factor && playedRightFoot == false)
            {
                PlayFootStep();
                playedRightFoot = true;
                playedLeftFoot = false;
            }
        }
    }

    void PlayFootStep()
    {
        var oldIndex = index;
        while (oldIndex == index)
        {
            index = Random.Range(0, stepClips.Length);
        }

        AudioManager.Instance.PlaySound(stepClips[index], AudioSourceType.Steps,transform);
        AudioSource.PlayClipAtPoint(stepClips[index], transform.position, 0.1f);
    }

    void OnAnimatorIK()
    {
        leftFootIKPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
        rightFootIKPos = anim.GetIKPosition(AvatarIKGoal.RightFoot);
    }
}