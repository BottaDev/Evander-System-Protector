using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip bossTheme;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (bossTheme == null)
            Debug.LogError("The level does not have a song assigned!");
        else
            PlayTheme();
    }

    public void PlayTheme()
    {
        source.clip = bossTheme;
        source.Play();
    }

    public void StopTheme()
    {
        source.Stop();
    }
}
