using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsPlayer : MonoBehaviour
{
    public AudioClip successSound;
    public AudioClip failureSound;
    public List<AudioClip> clickSounds = new List<AudioClip>();

    public void PlayClick()
    {
        if (clickSounds.Count == 0)
        {
            return;
        }

        int index = Random.Range(0, clickSounds.Count);
        audioSource.PlayOneShot(clickSounds[index]);
    }

    public void PlaySuccess()
    {
        audioSource.PlayOneShot(successSound);
    }

    public void PlayFailure()
    {
        audioSource.PlayOneShot(failureSound);
    }

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
