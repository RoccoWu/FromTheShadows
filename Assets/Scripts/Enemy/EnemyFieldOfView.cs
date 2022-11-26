using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;
    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public float minTimeToSeePlayer;
    [Range(0.0f, 0.3f)]
    public float currentTimeSeeingPlayer = 0.0f;

    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioClip windUpSFX;
    [SerializeField] private AudioClip bigAlertSFX;
    [SerializeField] private AudioSource enemyAudioSource2;

    [SerializeField] private Image foregroundImage;
    [SerializeField] private float updateSpeedSeconds = 0.5f;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        float fillValue = currentTimeSeeingPlayer / minTimeToSeePlayer;
        foregroundImage.fillAmount = fillValue;
        //Debug.Log(fillValue);
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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = target.position - transform.position;
            directionToTarget.Normalize();

            if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    float multiplier = radius / distanceToTarget;
                    currentTimeSeeingPlayer += Time.deltaTime * multiplier;
                    canSeePlayer = true;
                    if (!enemyAudioSource.isPlaying)
                    {
                        enemyAudioSource.PlayOneShot(windUpSFX, 1f);
                    }
                    if (currentTimeSeeingPlayer >= minTimeToSeePlayer)
                    {
                        if (!enemyAudioSource2.isPlaying)
                        {
                            enemyAudioSource.Stop();
                            enemyAudioSource2.Play();
                        }
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        gameManager.GameOver();
                    }  
                }
                else
                {
                    canSeePlayer = false;
                }

            }
            else
            {
                canSeePlayer = false;
            }

        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }

        if (!canSeePlayer)
        {
            enemyAudioSource.Stop();
            currentTimeSeeingPlayer -= Time.deltaTime;
            if(currentTimeSeeingPlayer <= 0.0f)
            {
                currentTimeSeeingPlayer = 0.0f;
            }
        }
    }
}
