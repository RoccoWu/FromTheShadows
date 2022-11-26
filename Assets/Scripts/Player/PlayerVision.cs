using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject[] enemies;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    private Transform playerEyes;
    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        StartCoroutine(FOVRoutine());
        playerEyes = transform.GetChild(8);
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(playerEyes.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            for(int i = 0; i<rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                Vector3 directionToTarget = target.position - playerEyes.position;
                directionToTarget.Normalize();

                if (Vector3.Angle(playerEyes.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(playerEyes.position, target.position);
                    if (!Physics.Raycast(playerEyes.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        Debug.Log("I am seen by player");
                        target.gameObject.GetComponent<EnemyController>().isPlayerDetected = true;

                    }
                    else
                    {
                        target.gameObject.GetComponent<EnemyController>().isPlayerDetected = false;

                    }

                }
                else
                {
                    target.gameObject.GetComponent<EnemyController>().isPlayerDetected = false;

                }
            }
        }
    }
}
