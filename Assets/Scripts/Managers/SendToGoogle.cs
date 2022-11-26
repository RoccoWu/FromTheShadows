using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToGoogle : MonoBehaviour
{
    private GameObject player;

    public GameObject frequencySonar;
    public GameObject frequencyDistract;
    public GameObject frequencyTeleport;

    public string FrequencySonar;
    public string FrequencyDistract;
    public string FrequencyTeleport;

    [SerializeField] string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScXTFoTxvsPx9qmMX2ZgRV73xcwWpFw2ELbM_ZjiP8KCTNsbQ/formResponse";
    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    IEnumerator Post(string fs, string fd, string ft)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1477733559", fs);
        form.AddField("entry.1087762430", fd);
        form.AddField("entry.1428830447", ft);
        byte[] rawData = form.data;
        WWW www = new WWW(BASE_URL, rawData);
        yield return www;


    }
    public void Send()
    {
        FrequencySonar = player.GetComponent<PlayerController>().freqSonar.ToString();
        FrequencyDistract = player.GetComponent<PlayerController>().freqDistract.ToString();
        FrequencyTeleport = player.GetComponent<PlayerController>().freqTeleport.ToString();

        StartCoroutine(Post(FrequencySonar, FrequencyDistract, FrequencyTeleport));
        print(FrequencySonar);
    }

    public void OnApplicationQuit()
    {
        Send();
    }
}
