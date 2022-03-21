using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathProjectile : MonoBehaviour
{

    int breathDamage = 30;
    public int BreathDamage {get; private set; }

    void OnEnable()
    {
        Destroy(gameObject, 3);
        BreathDamage = 30;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime *2, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().SubtractHealth(BreathDamage);
            Destroy(gameObject, 1);
            Debug.Log("collided");
        }
    }

}
