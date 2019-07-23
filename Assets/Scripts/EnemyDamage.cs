using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {

    [SerializeField] Collider collisionMesh;
    [SerializeField] int hitPoints = 10;

    [SerializeField] ParticleSystem hitParticlePrefab;
    [SerializeField] ParticleSystem deathParticlePrefab;

    [SerializeField] AudioClip enemyHitSFX;
    [SerializeField] AudioClip enemyDeathSFX;
    AudioSource myAudioSource;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
        if(hitPoints < 1)
        {
            KillEnemy();
        }
    }

    private void ProcessHit()
    {
        hitPoints = hitPoints - 1;
        hitParticlePrefab.Play();

        myAudioSource.PlayOneShot(enemyHitSFX);
    }

    private void KillEnemy()
    {
        var vfx = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        vfx.Play();

        //destroy particle after delay
        float destroyDelay = vfx.main.duration;
        Destroy(vfx.gameObject, vfx.main.duration);

        AudioSource.PlayClipAtPoint(enemyDeathSFX, Camera.main.transform.position);

        Destroy(gameObject); //destroys the enemy only, not particle
    }
}
