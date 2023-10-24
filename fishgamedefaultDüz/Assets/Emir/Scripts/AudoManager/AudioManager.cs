using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] Transform gameCam;

    [SerializeField] GameObject soundOnBtn;
    [SerializeField] GameObject soundOffBtn;

    [SerializeField] AudioSource[] musicSources;
    [SerializeField] GameObject musicOnBtn;
    [SerializeField] GameObject musicOffBtn;

    [SerializeField] AudioClip uiClip;
    [SerializeField] AudioClip[] fishCollectClips;
    [SerializeField] AudioClip[] bagCollectClips;
    [SerializeField] AudioClip[] bagReleaseClips;
    [SerializeField] AudioClip moneyCollectClip;
    [SerializeField] AudioClip openUpgradeAreaClip;
    [SerializeField] AudioClip upgradeBtnClip;
    [SerializeField] AudioClip hookReleaseClip;
    [SerializeField] AudioClip negativeClip;
    [SerializeField] AudioClip[] openingClips;

    bool isSoundOn;

    protected override void Awake()
    {
        base.Awake();
        isSoundOn = PlayerPrefs.GetInt("soundOn", 1) == 1;

        soundOnBtn.SetActive(isSoundOn);
        soundOffBtn.SetActive(!isSoundOn);
    }

    void Start()
    {

        if (PlayerPrefs.GetInt("musicOn", 1) == 1)
        {
            foreach (var musicSource in musicSources)
            {
                musicSource.playOnAwake = true;
            }
            musicOnBtn.SetActive(true);
            musicOffBtn.SetActive(false);
            musicSources[0].Play();
        }
        else
        {
            foreach (var musicSource in musicSources)
            {
                musicSource.playOnAwake = false;
            }
            musicOnBtn.SetActive(false);
            musicOffBtn.SetActive(true);
            musicSources[0].Stop();
        }
    }

    public void TurnMusicOff()
    {
        PlayerPrefs.SetInt("musicOn", 0);
        foreach (var musicSource in musicSources)
        {
            musicSource.playOnAwake = false;
        }
        musicSources[0].Stop();
        musicOnBtn.SetActive(false);
        musicOffBtn.SetActive(true);
    }

    public void TurnMusicOn()
    {
        PlayerPrefs.SetInt("musicOn", 1);
        foreach (var musicSource in musicSources)
        {
            musicSource.playOnAwake = true;
        }
        musicSources[0].Play();
        musicOnBtn.SetActive(true);
        musicOffBtn.SetActive(false);
        PlayUISoundAtGameCam();
    }

    public void TurnSoundOff()
    {
        PlayerPrefs.SetInt("soundOn", 0);
        isSoundOn = false;
        soundOnBtn.SetActive(false);
        soundOffBtn.SetActive(true);
    }

    public void TurnSoundOn()
    {
        PlayerPrefs.SetInt("soundOn", 1);
        isSoundOn = true;
        soundOnBtn.SetActive(true);
        soundOffBtn.SetActive(false);
        PlayUISoundAtGameCam();
    }

    public void PlayUISoundAtGameCam()
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(uiClip, gameCam.position);
    }

    public void PlayUpgradeUIClip()
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(upgradeBtnClip, gameCam.position + new Vector3(0, 2, 0));
    }


    public void PlayFishCollectAt(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(fishCollectClips[Random.Range(0, fishCollectClips.Length)], pos);
    }

    public void PlayCollectBagAtPoint(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(bagCollectClips[Random.Range(0, bagCollectClips.Length)], pos);
    }

    public void PlayReleaseBagAtPoint(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(bagReleaseClips[Random.Range(0, bagReleaseClips.Length)], pos);
    }

    public void PlayMoneyCollectAt(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(moneyCollectClip, pos);
    }

    public void PlayOpenUpgradeAreaAt(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(openUpgradeAreaClip, gameCam.position);
    }

    public void PlayHookReleaseClipAt(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(hookReleaseClip, pos);
    }

    public void PlayNegativeClipAt(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        AudioSource.PlayClipAtPoint(negativeClip, pos);
    }

    float nextPlayTime;

    public void PlayOpeningClipAt(Vector3 pos)
    {
        if (!isSoundOn)
            return;

        if (Time.time >= nextPlayTime)
        {
            nextPlayTime = Time.time + 0.4f;
            AudioSource.PlayClipAtPoint(openingClips[Random.Range(0, openingClips.Length)], gameCam.position + new Vector3(0, 2, 0));
        }

    }


}
