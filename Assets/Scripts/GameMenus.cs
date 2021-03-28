using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameMenus : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject menuStanding;

    public Controller_WithFocusPoint quetzal;

    public Image blackFadeImage;
    public Image blackBackground;

    [Header("HomeTitles")]
    public TextMeshProUGUI settingsHome;
    public TextMeshProUGUI creditsHome;
    public GameObject quitButton;
    public GameObject playButton;

    [Header("MainTitles")]
    public TextMeshProUGUI settingsMain;
    Button settingHomeButton;
    public TextMeshProUGUI creditsMain;
    Button creditsHomeButton;

    [Header("Bodies & return")]
    public GameObject settingsBody;
    public GameObject creditsBody;
    public GameObject returnSettings;
    public GameObject returnCredits;
    Button returnSettingsButton;
    Button returnCreditsButton;

    [Header("Sound Effects")]
    public AudioSource UIAudioSource;

    public Controller_WithFocusPoint player;

    /*
        PS : J'ai fait ce script en bourrin.
     */

    // Start is called before the first frame update
    void Start()
    {
        settingsMain.gameObject.SetActive(false);
        creditsMain.gameObject.SetActive(false);
        settingsBody.SetActive(false);
        creditsBody.SetActive(false);
        returnCredits.SetActive(false);
        returnSettings.SetActive(false);
        blackBackground.DOFade(0f, 0f);

        settingHomeButton = settingsHome.GetComponent<Button>();
        creditsHomeButton = creditsHome.GetComponent<Button>();
        returnCreditsButton = returnCredits.GetComponent<Button>();
        returnSettingsButton = returnSettings.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainMenu.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void BlackFade(float fadeTime)
    {
        blackFadeImage.color = new Color(0f, 0f, 0f, 0f);
        blackFadeImage.DOFade(1f, fadeTime).SetEase(Ease.InSine).OnComplete(
            () => blackFadeImage.DOFade(0f, fadeTime).SetEase(Ease.OutSine));
        
    }

    public void ShowCredits()
    {
        Vector3 oldMainPosition = creditsMain.transform.position;
        creditsHome.gameObject.SetActive(false);
        creditsMain.gameObject.SetActive(true);
        settingsHome.gameObject.SetActive(false);
        playButton.SetActive(false);
        quitButton.SetActive(false);
        creditsHomeButton.interactable = false;

        creditsBody.transform.DOScaleY(0f, 0f);
        returnCredits.transform.DOScaleY(0f, 0f);

        creditsMain.transform.DOMove(creditsHome.transform.position, 0f);
        creditsMain.transform.DOMove(oldMainPosition, 2f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            blackBackground.DOFade(0.4f, 1f);
            creditsBody.SetActive(true);
            creditsBody.transform.DOScaleY(1f, 1f).SetEase(Ease.OutSine);
            returnCredits.SetActive(true);
            returnCredits.transform.DOScaleY(1f, 1f).SetEase(Ease.OutSine);
            returnCreditsButton.interactable = true;
        });

    }

    public void HideCredits()
    {
        Vector3 oldHomePosition = creditsHome.transform.position;
        creditsHome.gameObject.SetActive(true);
        creditsMain.gameObject.SetActive(false);
        returnCreditsButton.interactable = false;

        creditsBody.transform.DOScaleY(0f, 1f).OnComplete(()=>creditsBody.SetActive(false));
        returnCredits.transform.DOScaleY(0f, 1f).OnComplete(() => returnCredits.SetActive(false));
        blackBackground.DOFade(0f, 1f);

        creditsHome.transform.DOMove(creditsMain.transform.position, 0f);
        creditsHome.transform.DOMove(oldHomePosition, 2f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            settingsHome.gameObject.SetActive(true);
            playButton.SetActive(true);
            quitButton.SetActive(true);
            creditsHomeButton.interactable = true;
        });
    }

    public void ShowSettings()
    {
        Vector3 oldMainPosition = settingsMain.transform.position;
        settingsHome.gameObject.SetActive(false);
        settingsMain.gameObject.SetActive(true);
        creditsHome.gameObject.SetActive(false);
        playButton.SetActive(false);
        quitButton.SetActive(false);
        settingHomeButton.interactable = false;

        creditsBody.transform.DOScaleY(0f, 0f);
        returnCredits.transform.DOScaleY(0f, 0f);

        settingsMain.transform.DOMove(settingsHome.transform.position, 0f);
        settingsMain.transform.DOMove(oldMainPosition, 2f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            blackBackground.DOFade(0.4f, 1f);
            settingsBody.SetActive(true);
            settingsBody.transform.DOScaleY(1f, 1f).SetEase(Ease.OutSine);
            returnSettings.SetActive(true);
            returnSettings.transform.DOScaleY(1f, 1f).SetEase(Ease.OutSine);
            returnSettingsButton.interactable = true;
        });

    }

    public void HideSettings()
    {
        Vector3 oldHomePosition = settingsHome.transform.position;
        settingsHome.gameObject.SetActive(true);
        settingsMain.gameObject.SetActive(false);
        returnSettingsButton.interactable = false;

        settingsBody.transform.DOScaleY(0f, 1f).OnComplete(() => settingsBody.SetActive(false));
        returnSettings.transform.DOScaleY(0f, 1f).OnComplete(() => returnSettings.SetActive(false));
        blackBackground.DOFade(0f, 1f);

        settingsHome.transform.DOMove(settingsMain.transform.position, 0f);
        settingsHome.transform.DOMove(oldHomePosition, 2f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            creditsHome.gameObject.SetActive(true);
            playButton.SetActive(true);
            quitButton.SetActive(true);
            settingHomeButton.interactable = true;
        });
    }

    public void PlayUISoundEffect(AudioClip clipToPlay)
    {
        if (!UIAudioSource.isPlaying)
        {
            UIAudioSource.clip = clipToPlay;
            UIAudioSource.Play();
        }
    }

    public void PlayGame()
    {
        BlackFade(1.5f);
        playButton.SetActive(false);
        quitButton.SetActive(false);
        creditsHome.gameObject.SetActive(false);
        settingsHome.gameObject.SetActive(false);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            menuStanding.SetActive(false);
        });
        DOVirtual.DelayedCall(3f, () =>
        {
            mainMenu.SetActive(false);
        });

        player.GameRunning = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
