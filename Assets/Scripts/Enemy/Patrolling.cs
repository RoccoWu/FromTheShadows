using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public Transform[] points;
    int currentPoint;
    public float speed;
    public float rotationSpeed;
    public float waitDelay;
    // Start is called before the first frame update
    void Start()
    {
        if(points.Length != 0)
        {
            currentPoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (points.Length != 0)
        {
            if (transform.position != points[currentPoint].position)
            {

                var targetRotation = Quaternion.LookRotation(points[currentPoint].position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                Vector3 dirFromEnemyToPoint = (points[currentPoint].position - transform.position).normalized;
                float dot = Vector3.Dot(dirFromEnemyToPoint, transform.forward);
                if(dot > 0.98)
                {
                    transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, speed * Time.deltaTime);
                }
            }
            else
            {
                currentPoint = (currentPoint + 1) % points.Length;
            }
        }
    }
}
