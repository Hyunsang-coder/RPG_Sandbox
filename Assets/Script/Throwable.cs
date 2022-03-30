using System.Collections;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float timeToDestory = 4f;
    [SerializeField] float timeToExplode = 1.5f;
    [SerializeField] float stunTime = 4f;
    ParticleSystem particleSystem;
    Rigidbody rigidbody;
    SphereCollider collider;
    MeshRenderer meshRendere;
    SoundManager soundManager;
    void Awake()
    {
        meshRendere = GetComponent<MeshRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        rigidbody = GetComponentInChildren<Rigidbody>();
        collider = GetComponentInChildren<SphereCollider>();
        particleSystem.Pause();
        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnEnable()
    {
        collider.enabled = false;
        Destroy(gameObject, timeToDestory);
        StartCoroutine(Explosion());
    }
    IEnumerator Explosion() 
    {   yield return new WaitForSeconds(timeToExplode);
        meshRendere.enabled = false ;
        rigidbody.freezeRotation = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        particleSystem.Play();
        soundManager.PlayAudio("FlashBang");

        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dragon" || other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<MonsterAI>().GetStunned(stunTime);
        }
    }
}
