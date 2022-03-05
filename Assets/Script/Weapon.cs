using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] int weaponDamage = 10;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().SubtractHealth(weaponDamage);
        }

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<MonsterHealth>().SubtractHealth(weaponDamage);
        }


    }

}
