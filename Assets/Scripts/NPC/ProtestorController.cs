using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtestorController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string [] angryGestures;
    [SerializeField] private string angryGestureChosen;
    public float angryTimer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ProtestOutburstTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ProtestOutburstTimer()
    {
        angryTimer = Random.Range (5, 10);
        yield return new WaitForSeconds(angryTimer);
         angryGestureChosen = angryGestures[Random.Range(0, angryGestures.Length)];
         DoAngryGesture(angryGestureChosen);
    }

    private void DoAngryGesture(string gesture)
    {
        anim.SetTrigger(angryGestureChosen);
    }

    public void StartGestureTimer()
    {
        StartCoroutine(ProtestOutburstTimer());
    }
}
