using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour
{

    private AudioSource audioSource;
    private void Awake()
    {
        gameObject.name = "AsteroidExplosion";
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
    public void SetAudio(float volumeLevel, float pitchLevel)
    {
        audioSource.volume = volumeLevel;
        audioSource.pitch = pitchLevel;
    }
}
