using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
	// EnemyHealth is a the enemy NPC specific health manager.
	// Any in-game entity that reacts to a shot must have a HealthManager script.
	public class EnemyHealth : HealthManager
	{
		[Tooltip("The current NPC health.")]
		public float health = 100f;
		[Tooltip("The game object particle emitted when hit.")]
		public GameObject bloodSample;
		[Tooltip("Use headshot damage multiplier?")]
		public bool headshot;

		private float totalHealth;                                  // The total NPC initial health.
		private Transform weapon;                                   // The NPC weapon.
		private float originalBarScale;                             // The initial NPC health bar size.
		private Animator anim;                                      // The NPC animator controller.
		private StateController controller;                         // The NPC AI FSM controller.

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

		public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin = null)
		{
			// Headshot multiplier. On default values, instantly kills NPC.
			if (!dead && headshot && bodyPart.transform == anim.GetBoneTransform(HumanBodyBones.Head))
			{
				// Default damage multiplier is 10x.
				damage *= 10;
				// Call headshot HUD callback, if any.
				GameObject.FindGameObjectWithTag("GameController").SendMessage("HeadShotCallback", SendMessageOptions.DontRequireReceiver);
			}

			// Create spouted blood particle on shot location.
			Instantiate(bloodSample, location, Quaternion.LookRotation(-direction), transform);
			// Take damage received from current health.
			health -= damage;

			if (!dead)
			{
				// Trigger hit animation.
				if(!anim.IsInTransition(3) && anim.GetCurrentAnimatorStateInfo(3).IsName("No hit"))
					anim.SetTrigger("Hit");
				// Update FSM related references.
				controller.variables.feelAlert = true;
				controller.personalTarget = controller.LastTarget.position;
			}
			if (health <= 0)
			{
				if (!dead)
					Kill();

				// Shooting a dead body? Just apply shot force on the ragdoll part.
				bodyPart.GetComponent<Rigidbody>().AddForce(100f * direction.normalized, ForceMode.Impulse);
			}
		}

		public void Kill()
		{
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