using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Damage{get;set;}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Dragon")
        {
            other.gameObject.GetComponent<EnemyHealth>().SubtractHealth(Damage);
        }
    }

}
