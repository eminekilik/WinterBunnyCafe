using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio")]
    public AudioSource bgmSource;

    [Header("UI")]
    public Image musicIcon;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    bool musicOn;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        if (musicOn)
            bgmSource.Play();
        else
            bgmSource.Pause();

        UpdateIcon();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        if (musicOn)
            bgmSource.UnPause();
        else
            bgmSource.Pause();

        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (musicIcon == null) return;
        musicIcon.sprite = musicOn ? musicOnSprite : musicOffSprite;
    }
}
