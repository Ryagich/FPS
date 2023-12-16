using UnityEngine;
using UnityEngine.AI;
using System;
using InfimaGames.LowPolyShooterPack;

namespace EnemyAI
{
    // EnemyHealth is a the enemy NPC specific health manager.
    // Any in-game entity that reacts to a shot must have a HealthManager script.
    public class EnemyHealth : HealthManager
    {
        public event System.Action Dead;
        [Tooltip("The current NPC health.")] public float health = 100f;

        [Tooltip("The game object particle emitted when hit.")]
        public GameObject bloodSample;

        [Tooltip("Use headshot damage multiplier?")]
        public bool headshot;

        private float totalHealth; // The total NPC initial health.
        private Transform weapon; // The NPC weapon.
        private float originalBarScale; // The initial NPC health bar size.
        private Animator anim; // The NPC animator controller.
        private StateController controller; // The NPC AI FSM controller.

        private void Awake()
        {
            totalHealth = health;
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

        public void TakeDamageFromExplossion(float damage, GameObject origin = null)
        {
            
        }
        
        public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart,
            GameObject origin = null)
        {
            // Headshot multiplier. On default values, instantly kills NPC.
            if (!dead && headshot && bodyPart.transform == anim.GetBoneTransform(HumanBodyBones.Head))
            {
                damage *= 10;
            }

            // Create spouted blood particle on shot location.
            Instantiate(bloodSample, location, Quaternion.LookRotation(-direction), transform);
            // Take damage received from current health.
            health -= damage;

            if (!dead)
            {
                // Trigger hit animation.
                if (!anim.IsInTransition(3) && anim.GetCurrentAnimatorStateInfo(3).IsName("No hit"))
                    anim.SetTrigger("Hit");
                // Update FSM related references.
                if (origin)
                {
                    controller.variables.feelAlert = true;
                    controller.personalTarget = origin.transform.position;
                }
            }
            
            if (health <= 0)
            {
                if (!dead)
                {
                    Kill();
                    var callback = origin?.GetComponent<Character>()?.GetComponent<KillEnemyCallback>();
                    if (callback)
                    {
                        callback.GetKillCallback();
                    }
                }
                // Shooting a dead body? Just apply shot force on the ragdoll part.
                bodyPart.GetComponent<Rigidbody>().AddForce(100f * direction.normalized, ForceMode.Impulse);
            }
        }

        public void Kill()
        {
            foreach (var mb in GetComponents<MonoBehaviour>())
            {
                if (this != mb)
                    Destroy(mb);
            }

            Destroy(this.GetComponent<NavMeshAgent>());
            RemoveAllForces();
            anim.enabled = false;
            Destroy(weapon.gameObject);
            dead = true;
            
            Dead?.Invoke();
        }

        // Remove existing forces and set ragdoll parts as not kinematic to interact with physics.
        private void RemoveAllForces()
        {
            foreach (var member in GetComponentsInChildren<Rigidbody>())
            {
                member.isKinematic = false;
                member.velocity = Vector3.zero;
            }
        }
    }
}