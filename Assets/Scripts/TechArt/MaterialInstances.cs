using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstances : MonoBehaviour
{
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
         material = GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
