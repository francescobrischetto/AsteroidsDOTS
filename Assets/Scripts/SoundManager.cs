using UnityEngine;

public static class SoundManager
{

    public enum Sound
    {
        Shoot,
        LoseLife,
        ObjectDestroyed,
        PowerUpPicked,
        PowerUpLost,
        HyperSpaceTravel
    }

    public static void PlaySound(Sound sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        AudioClip audioClip = GetAudioClip(sound);
        audioSource.PlayOneShot(audioClip);
        MonoBehaviour.Destroy(gameObject, audioClip.length);
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundsPrefab.SoundAudioClip soundAudioClip in SoundsPrefab.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

}

