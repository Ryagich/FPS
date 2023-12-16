//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	public class GrenadeScript : MonoBehaviour
	{

		[Header("Timer")]
		//Time before the grenade explodes
		[Tooltip("Time before the grenade explodes")]
		public float grenadeTimer = 5.0f;

		[Header("Explosion Prefabs")]
		//Explosion prefab
		public Transform explosionPrefab;

		[Header("Explosion Options")]
		//Radius of the explosion
		[Tooltip("The radius of the explosion force")]
		public float radius = 25.0F;

		//Intensity of the explosion
		[Tooltip("The intensity of the explosion force")]
		public float power = 350.0F;

		[Header("Throw Force")]
		[Tooltip("Minimum throw force")]
		public float minimumForce = 1500.0f;

		[Tooltip("Maximum throw force")]
		public float maximumForce = 2500.0f;

		private float throwForce;

		[Header("Audio")]
		public AudioSource impactSound;

		private void Awake()
		{
			//Generate random throw force
			//based on min and max values
			throwForce = Random.Range
				(minimumForce, maximumForce);

			//Random rotation of the grenade
			GetComponent<Rigidbody>().AddRelativeTorque
			(Random.Range(500, 1500), //X Axis
				Random.Range(0, 0), //Y Axis
				Random.Range(0, 0) //Z Axis
				* Time.deltaTime * 5000);
		}

		private void Start()
		{
			//Launch the projectile forward by adding force to it at start
			GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * throwForce);

			//Start the explosion timer
			StartCoroutine(ExplosionTimer());
		}

		private void OnCollisionEnter(Collision collision)
		{
			//Play the impact sound on every collision
			impactSound.Play();
		}

		private IEnumerator ExplosionTimer()
		{
			//Wait set amount of time
			yield return new WaitForSeconds(grenadeTimer);

			//Raycast downwards to check ground
			RaycastHit checkGround;
			if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
			{
				//Instantiate metal explosion prefab on ground
				Instantiate(explosionPrefab, checkGround.point,
					Quaternion.FromToRotation(Vector3.forward, checkGround.normal));
			}

			//Explosion force
			var explosionPos = transform.position;
			//Use overlapshere to check for nearby colliders
			var colliders = Physics.OverlapSphere(explosionPos, radius);
			foreach (var с in colliders)
			{
				//Ignore the player character.
				if (с.CompareTag("Player"))
					continue;

				var rb = с.GetComponent<Rigidbody>();

				//Add force to nearby rigidbodies
				if (rb != null)
					rb.AddExplosionForce(power * 5, explosionPos, radius, 3.0F);

				//If the explosion hits "Target" tag and isHit is false
				if (с.GetComponent<Collider>().tag == "Target"
				    && с.gameObject.GetComponent<TargetScript>().isHit == false)
				{
					//Toggle "isHit" on target object
					с.gameObject.GetComponent<TargetScript>().isHit = true;
				}

				//If the explosion hits "ExplosiveBarrel" tag
				if (с.GetComponent<Collider>().tag == "ExplosiveBarrel")
				{
					//Toggle "explode" on explosive barrel object
					с.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
				}

				//If the explosion hits "GasTank" tag
				if (с.GetComponent<Collider>().tag == "GasTank")
				{
					//Toggle "isHit" on gas tank object
					с.gameObject.GetComponent<GasTankScript>().isHit = true;
					//Reduce explosion timer on gas tank object to make it explode faster
					с.gameObject.GetComponent<GasTankScript>().explosionTimer = 0.05f;
				}
				//TODO: Костыль
				var stats = с.GetComponent<StatsController>();
				if (stats)
				{
					var damage = 150 * ((radius - Vector3.Distance(с.transform.position, explosionPos)) / radius);
					stats.TakeDamage(damage);
				}

				if (LayerMask.NameToLayer("Enemy") == с.gameObject.layer)
				{
					var damage = 80 * ((radius - Vector3.Distance(с.transform.position, explosionPos)) / radius);
					с.SendMessageUpwards("HitCallback",
						new HealthManager.DamageInfo(transform.position,
							transform.forward, damage,
							с, TargetPointer.Instance.gameObject));
				}
			}

			//Destroy the grenade object on explosion
			Destroy(gameObject);
		}
	}
}