using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    private GameObject player;
    private bool canBePickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void AddSecurityBadge()
    {
        gm.AddSecurityBadge();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            canBePickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canBePickedUp = false;
        }
    }
}
