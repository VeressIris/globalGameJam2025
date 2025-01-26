using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioManager()
    {
        instance = this;
    }

    public void Play(string effectName)
    {
        transform.Find(effectName).GetComponent<AudioSource>().Play();
    }
    public void PlayOnly(string effectName)
    {
        foreach(var comp in transform.GetComponentsInChildren<AudioSource>())
        {
            comp.Stop();
        }
        transform.Find(effectName).GetComponent<AudioSource>().Play();
    }

    public void SetVolume(string effectName, float amount)
    {
        transform.Find(effectName).GetComponent<AudioSource>().volume = amount;
    }
    public void SetPitch(string effectName, float amount)
    {
        transform.Find(effectName).GetComponent<AudioSource>().pitch = amount;
    }
}
