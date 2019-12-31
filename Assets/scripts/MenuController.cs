using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private AudioSource AudioSource;

    public void Start()
    {
        AudioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        DontDestroyOnLoad(AudioSource); // Keeps the audio source, so the music stays on.
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Mute()
    {
        if (AudioSource.isPlaying)
        {
            AudioSource.Pause();
        }
        else
        {
            AudioSource.UnPause();
        }
    }
}
