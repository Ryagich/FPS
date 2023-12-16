//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections;
using EnemyAI;
using InfimaGames.LowPolyShooterPack;
using InfimaGames.LowPolyShooterPack.Interface;
using Random = UnityEngine.Random;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    public class Projectile : MonoBehaviour
    {
        [Range(5, 100)] [Tooltip("After how long time should the bullet prefab be destroyed?")]
        public float destroyAfter;
        
        [SerializeField] private LayerMask _mask;
        [Header("Impact Effect Prefabs")] public Transform[] bloodImpactPrefabs;

        public Transform[] metalImpactPrefabs;
        public Transform[] dirtImpactPrefabs;
        public Transform[] concreteImpactPrefabs;

        private float damage = 40f;

        public void SetDamage(float damage)
        {
            this.damage = damage;
        }

        private void Start()
        {
            //Grab the game mode service, we need it to access the player character!
            var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            //Ignore the main player character's collision. A little hacky, but it should work.
            Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(),
                GetComponent<Collider>());

            //Start destroy timer
            StartCoroutine(DestroyAfter());
        }

        //If the bullet collides with anything
        private void OnCollisionEnter(Collision collision)
        {
            //Ignore collisions with other projectiles.
            if (collision.gameObject.GetComponent<Projectile>() != null)
                return;

            // //Ignore collision if bullet collides with "Player" tag
            // if (collision.gameObject.CompareTag("Player")) 
            // {
            // 	//Physics.IgnoreCollision (collision.collider);
            // 	Debug.LogWarning("Collides with player");
            // 	//Physics.IgnoreCollision(GetComponent<Collider>(), GetComponent<Collider>());
            //
            // 	//Ignore player character collision, otherwise this moves it, which is quite odd, and other weird stuff happens!
            // 	Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            //
            // 	//Return, otherwise we will destroy with this hit, which we don't want!
            // 	return;
            // }
            //Чтобы не стреляли сами в себя
            if (collision.gameObject.CompareTag("Player") || (collision.collider && LayerMask.NameToLayer("Character") == collision.gameObject.layer))
            {
                return;
            }

            if (collision.transform.tag == "Blood")
            {
                Instantiate(bloodImpactPrefabs[Random.Range
                        (0, bloodImpactPrefabs.Length)], transform.position,
                    Quaternion.LookRotation(collision.contacts[0].normal));
            }

            if (collision.transform.tag == "Metal")
            {
                Instantiate(metalImpactPrefabs[Random.Range
                        (0, bloodImpactPrefabs.Length)], transform.position,
                    Quaternion.LookRotation(collision.contacts[0].normal));
            }

            if (collision.transform.tag == "Dirt")
            {
                Instantiate(dirtImpactPrefabs[Random.Range
                        (0, bloodImpactPrefabs.Length)], transform.position,
                    Quaternion.LookRotation(collision.contacts[0].normal));
            }

            if (collision.transform.tag == "Concrete")
            {
                Instantiate(concreteImpactPrefabs[Random.Range
                        (0, bloodImpactPrefabs.Length)], transform.position,
                    Quaternion.LookRotation(collision.contacts[0].normal));
            }

            if (collision.transform.tag == "Target")
            {
                collision.transform.gameObject.GetComponent
                    <TargetScript>().isHit = true;
            }

            if (collision.transform.tag == "ExplosiveBarrel")
            {
                collision.transform.gameObject.GetComponent
                    <ExplosiveBarrelScript>().explode = true;
            }

            if (collision.transform.tag == "GasTank")
            {
                collision.transform.gameObject.GetComponent
                    <GasTankScript>().isHit = true;
            }

            // if ((_mask.value & collision.gameObject.layer) != 0 && collision.collider)
            //TODO: very bad
            if (collision.collider && LayerMask.NameToLayer("Enemy") == collision.gameObject.layer)
            {
                collision.collider.SendMessageUpwards("HitCallback",
                    new HealthManager.DamageInfo(transform.position,
                        transform.forward, damage,
                        collision.collider, TargetPointer.Instance.gameObject));
            }
            Destroy(gameObject);
        }
        
        private IEnumerator DestroyAfter()
        {
            //Wait for set amount of time
            yield return new WaitForSeconds(destroyAfter);
            //Destroy bullet object
            Destroy(gameObject);
        }
    }
}