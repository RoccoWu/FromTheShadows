using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public bool isSonarDetected = false;
    public bool isPlayerDetected = false;
    [SerializeField] private SkinnedMeshRenderer characterRenderer;
    [SerializeField] private float dissolveValue;
    [SerializeField] private int dissolveTime = 1;
    public float visibleDuration = 5f;

   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        characterRenderer.material.SetFloat("_DissolveValue", dissolveValue);

        //Player sees enemy
        if (isPlayerDetected)
        {
            dissolveValue = Mathf.Lerp(dissolveValue, -0.5f, 3.0f * Time.deltaTime);
            GetComponent<FadingUI>().FadeIn(0.6f);
        }

        else
        {
            if (!isSonarDetected)
            {
                dissolveValue = Mathf.Lerp(dissolveValue, 1f, 3.0f * Time.deltaTime);
                GetComponent<FadingUI>().FadeOut(0.6f);
            }
        }

        //Sonar
        if (isSonarDetected)
        {
            dissolveValue = Mathf.Lerp(dissolveValue, 0f,  dissolveTime * Time.deltaTime);
            GetComponent<FadingUI>().FadeIn(0.6f);
        }
        
        else
        {
            dissolveValue = Mathf.Lerp(dissolveValue, 1f,  dissolveTime * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sonar")
        {
            isSonarDetected = true;
        }
    }

    public IEnumerator enemyVisibilityTimer()
{
        while(visibleDuration > 0 )
        {
            yield return new WaitForSeconds(visibleDuration);
            visibleDuration--;
        }
        isSonarDetected = false;
        visibleDuration = 5f;
    }

    public void MoveTowards(GameObject g)
    {
        StartCoroutine(MoveTowardsCoroutine(g.transform));
    }

    IEnumerator MoveTowardsCoroutine(Transform target)
    {
        while(Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.5f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(3f);
    }
}
