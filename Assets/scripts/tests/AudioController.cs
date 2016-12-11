using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioController : MonoBehaviour {

    [SerializeField]
    private AudioClip bgmGame;
    [SerializeField]
    private AudioClip bgmMenu;
    [SerializeField]
    private AudioClip sndTap;
    [SerializeField]
    private AudioClip sndAttack;

    private static AudioSource _sndSource;
    private static AudioSource _bgmSource;

    private static AudioClip _bgmGame;
    private static AudioClip _bgmMenu;
    private static AudioClip _sndTap;
    private static AudioClip _sndAttack;

    // Use this for initialization
    void Start () {
        _sndSource = gameObject.AddComponent<AudioSource>();
        _bgmSource = gameObject.AddComponent<AudioSource>();

        _bgmSource.loop = true;

        _bgmGame = bgmGame;
        _bgmMenu = bgmMenu;
        _sndTap = sndTap;
        _sndAttack = sndAttack;

        switch (SceneManager.GetActiveScene().name) {
            case "Game":
                PlayGameBgm(1f);
                break;
            case "Menu":
                PlayMenuBgm(1f);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void PlayTapSnd(float v) {
        _sndSource.clip = _sndTap;
        _sndSource.volume = v;
        _sndSource.Play();
    }

    public static void PlayAttackSnd(float v)
    {
        _sndSource.clip = _sndAttack;
        _sndSource.volume = v;
        _sndSource.Play();
    }

    public static void PlayGameBgm(float v)
    {
        _bgmSource.clip = _bgmGame;
        _bgmSource.volume = v;
        _bgmSource.Play();
    }

    public static void PlayMenuBgm(float v)
    {
        _bgmSource.clip = _bgmMenu;
        _bgmSource.volume = v;
        _bgmSource.Play();
    }

    public static void StopBgm() {
        _bgmSource.Stop();
    }

    public static void ContinueBgm() {
        if (_bgmSource.isPlaying) return;

        _bgmSource.Play();
    }
}
