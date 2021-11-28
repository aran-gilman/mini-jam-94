using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSfx : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();

    public void Play()
    {
        if (clips.Count == 0)
        {
            return;
        }

        int index = Random.Range(0, clips.Count);
        audioSource.PlayOneShot(clips[index]);
    }

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<AudioSource>();
    }
}
