using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] AudioClip switchItemSound;
    [SerializeField] AudioClip selectItemSound;

    [SerializeField] AudioClip errorSound;
    AudioSource audioSource;

    public static SoundManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySwitchSound()
    {
        audioSource.PlayOneShot(switchItemSound, 1f);
    }

    public void PlaySelectSound()
    {
        audioSource.PlayOneShot(selectItemSound, 1f);
    }

    public void PlayErrorSound()
    {
        audioSource.PlayOneShot(errorSound, 1f);
    }
}
