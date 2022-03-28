using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject[] contents;
    [SerializeField] Transform spawnPoint;
    [SerializeField] bool openReady;
    [SerializeField] bool boxUsed;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            openReady = true;
        }
    }

    public void BoxOpen()
    {
        if (openReady && !boxUsed)
        {
            StartCoroutine(ItemPopup());
        }
        else return;
    }

    IEnumerator ItemPopup()
    {
        int randomNo = Random.Range(0, 2);
        boxUsed = true;
        anim.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        Instantiate(contents[randomNo], spawnPoint.position, Quaternion.identity);
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            BoxOpen();
        }
    }

}
