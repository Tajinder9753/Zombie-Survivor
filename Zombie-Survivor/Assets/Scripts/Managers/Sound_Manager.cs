using UnityEngine;
using UnityEngine.UIElements;

public class Sound_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource soundObject;


    public void PlaySoundEffect(AudioClip audioClip, Transform spawnTransform, float volume)
    {         
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void MovementSounds(AudioSource audioSource)
    {
        
    }

}
