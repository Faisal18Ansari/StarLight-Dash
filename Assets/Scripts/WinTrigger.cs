using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject Area_fire_red; // Your celebration VFX GameObject
    public GameObject winUI;
    public AudioSource bgmAudioSource;        // Your Win UI Panel/Text

    private bool hasWon = false;     // This field is used properly

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return; // Prevent multiple triggers

        if (other.CompareTag("Player"))
        {
            hasWon = true;
            if (bgmAudioSource != null && bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Pause();
            }

            // Play VFX
            if (Area_fire_red != null)
            {
                Area_fire_red.SetActive(true);

                var ps = Area_fire_red.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Clear();
                    ps.Play();
                }

                var audio = Area_fire_red.GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                }
            }

            // Show Win UI
            if (winUI != null)
            {
                winUI.SetActive(true);
            }

            Debug.Log("YOU WON!");

            // Resume background music after 11 seconds
            Invoke(nameof(ResumeBGM), 11f);
        }
    }
    private void ResumeBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.UnPause();
        }
    }
}
