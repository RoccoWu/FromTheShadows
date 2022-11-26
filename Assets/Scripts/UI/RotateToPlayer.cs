using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        transform.LookAt(player.transform);
    }
}
