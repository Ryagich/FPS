using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace EnemyAI
{
    // EnemyHealth is a the enemy NPC specific health manager.
    // Any in-game entity that reacts to a shot must have a HealthManager script.
    public class EnemyHealth : HealthManager
    {
        public UnityEvent Dead;
        [Tooltip("The current NPC health.")] public float health = 100f;

        [Tooltip("The game object particle emitted when hit.")]
        public GameObject bloodSample;

        [Tooltip("Use headshot damage multiplier?")]
        public bool headshot;

        private Transform weapon; // The NPC weapon.
        private float originalBarScale; // The initial NPC health bar size.
        private HealthBillboardManager healthUI; // The NPC health HUD.
        private Animator anim; // The NPC animator controller.
        private StateController controller; // The NPC AI FSM controller.

        [SerializeField] private List<AudioClip> _hurtSounds = new();
        [SerializeField] private AudioSource _source;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            controller = GetComponent<StateController>();

            foreach (Transform child in anim.GetBoneTransform(HumanBodyBones.RightHand))
            {
                weapon = child.Find("muzzle");
                if (weapon != null)
                {
                    break;
                }
            }

            weapon = weapon.parent;
        }

        public override void TakeDamage(Vector3 location, Vector3 direction, float damage, bool isPlayer,
            Collider bodyPart,
            GameObject origin = null)
        {
            if (!dead && headshot && bodyPart.transform == anim.GetBoneTransform(HumanBodyBones.Head))
            {
                damage *= 2;
                GameObject.FindGameObjectWithTag("GameController")
                    .SendMessage("HeadShotCallback", SendMessageOptions.DontRequireReceiver);
                if (isPlayer)
                    CallbackController.Instance.AddDamageCallBack(new CallbackInfo(CallbackTypes.Headshot, ""));
            }

            Object.Instantiate<GameObject>(bloodSample, location, Quaternion.LookRotation(-direction), this.transform);
            health -= damage;

            if (!dead)
            {
                if (!anim.IsInTransition(3) && anim.GetCurrentAnimatorStateInfo(3).IsName("No hit"))
                    anim.SetTrigger("Hit");
                controller.variables.feelAlert = true;
                controller.personalTarget = controller.aimTarget.position;
                if (isPlayer)

                    CallbackController.Instance.AddDamageCallBack(new CallbackInfo(CallbackTypes.Damage,
                        ((int)damage).ToString()));
                _source.PlayOneShot(_hurtSounds[Random.Range(0, _hurtSounds.Count - 1)]);
            }

            // Time to die.
            if (health <= 0)
            {
                // Kill the NPC?
                if (!dead)
                {
                    Kill();
                    Dead?.Invoke();
                }

                // Shooting a dead body? Just apply shot force on the ragdoll part.
                bodyPart.GetComponent<Rigidbody>().AddForce(100f * direction.normalized, ForceMode.Impulse);
            }
        }

        // Remove unecessary components on killed NPC and set as dead.
        public void Kill()
        {
            // Destroy all other MonoBehaviour scripts attached to the NPC.
            foreach (MonoBehaviour mb in this.GetComponents<MonoBehaviour>())
            {
                if (this != mb)
                    Destroy(mb);
            }

            Destroy(this.GetComponent<NavMeshAgent>());
            RemoveAllForces();
            anim.enabled = false;
            Destroy(weapon.gameObject);
            dead = true;
        }

        // Remove existing forces and set ragdoll parts as not kinematic to interact with physics.
        private void RemoveAllForces()
        {
            foreach (Rigidbody member in GetComponentsInChildren<Rigidbody>())
            {
                member.isKinematic = false;
                member.velocity = Vector3.zero;
            }
        }
    }
}