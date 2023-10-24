using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;
using DG.Tweening;

public class InGameUI : Singleton<InGameUI>
{
    [SerializeField] GameObject turnBackBtn;
    [SerializeField] GameObject shopUI;

    [SerializeField] GameObject GameMap;

    [SerializeField] GameObject inGamePanel;
    [SerializeField] GameObject fishGamePanel;

    [SerializeField] GameObject settings;

    [SerializeField] TextMeshProUGUI idTmp;

    string playerId;

    bool isPaused;

    bool canTurnBack;
    GameObject activeUI;

    public GameObject ActiveUI { set { activeUI = value; } }

    public void DisableTurnBack()
    {
        canTurnBack = false;
    }

    public void EnableTurnBack()
    {
        canTurnBack = true;
    }

    private void OnEnable()
    {
        // Subscribe to the focusChanged event
        Application.focusChanged += HandleFocusChange;
    }

    private void OnDisable()
    {
        // Unsubscribe from the focusChanged event when this script is disabled or destroyed
        Application.focusChanged -= HandleFocusChange;
    }


    private void HandleFocusChange(bool isFocused)
    {
        if (!isGameStarted)
            return;
        // This method is called when the game window loses or regains focus
        if (isFocused)
        {
            if (!isPaused)
            {
                Time.timeScale = 1;
                AdManager.Instance.TapToPlay();
            }
        }
        else
        {
            Time.timeScale = 0;
            AdManager.Instance.OnPause();
        }
    }

    bool isAlreadyOpen;
    [SerializeField] FirstTutorial firstTutorial;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPaused)
        {
            if (GameMap.activeSelf == false)
            {
                if (canTurnBack)
                {
                    if (firstTutorial)
                    {
                        firstTutorial.OnTutorialLeavePressed();
                    }
                    OnTurnBackBtnPressed();
                }
            }
            else if (activeUI)
            {
                activeUI.SetActive(false);
                PlayerMovement.Instance.EnableMovement();
                AudioManager.Instance.PlayUISoundAtGameCam();
                activeUI = null;
            }

        }
        if (Input.GetButtonDown("Pause"))
        {
            if (!isPaused)
                OpenSettings();
            else
                CloseSettings();
        }
    }
    bool isGameStarted;
    [SerializeField] CanvasGroup tapToPlay;
    public void StartGameNow()
    {
        AudioManager.Instance.PlayUISoundAtGameCam();
        Time.timeScale = 1f;
        tapToPlay.DOFade(0f, 2f).OnComplete(AfterComplete);
    }

    private void AfterComplete()
    {
        tapToPlay.gameObject.SetActive(false);
        AdManager.Instance.TapToPlay();
        isGameStarted = true;
    }

    private void Start()
    {
        playerId = PlayerPrefs.GetString("playerId", "1");
        if (playerId == "1")
        {
            playerId = GenerateRandomString();
            PlayerPrefs.SetString("playerId", playerId);
        }
        idTmp.SetText(playerId);
        if (!isGameStarted)
            Time.timeScale = 0f;

        tapToPlay.gameObject.SetActive(true);
    }

    const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    const int stringLength = 5;

    public string GenerateRandomString()
    {
        StringBuilder stringBuilder = new StringBuilder(stringLength);
        System.Random random = new System.Random();

        for (int i = 0; i < stringLength; i++)
        {
            int index = random.Next(characters.Length);
            stringBuilder.Append(characters[index]);
        }

        return stringBuilder.ToString();
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
        AudioManager.Instance.PlayUISoundAtGameCam();
        Time.timeScale = 0;
        AdManager.Instance.OnPause();
        isPaused = true;
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        AudioManager.Instance.PlayUISoundAtGameCam();
        Time.timeScale = 1;
        AdManager.Instance.TapToPlay();
        isPaused = false;
    }

    public void ShowTurnBackBtn()
    {
        turnBackBtn.SetActive(true);
    }

    public void CloseTurnBackBtn()
    {
        turnBackBtn.SetActive(false);

    }

    [SerializeField] GameObject tutorialParent;
    bool prefsLoaded;

    public void ShowFishGameUI()
    {
        fishGamePanel.SetActive(true);
        if (tutorialParent)
        {
            isAlreadyOpen = tutorialParent.activeSelf;
            if (prefsLoaded)
            {
                tutorialParent.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("pointerPhase", 0) > 3)
            {
                prefsLoaded = true;
                tutorialParent.SetActive(false);
            }
        }



        // inGamePanel.SetActive(false);
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void OnTurnBackBtnPressed()
    {
        AudioManager.Instance.PlayUISoundAtGameCam();
        AreaManager.Instance.gameObject.SetActive(false);
        GameMap.SetActive(true);
        PlayerMovement.Instance.EnableMovement();
        fishGamePanel.SetActive(false);
        if (tutorialParent)
            tutorialParent.SetActive(isAlreadyOpen);
        // inGamePanel.SetActive(true);
    }

    public void ShowShopUI()
    {
        shopUI.SetActive(true);
    }
}
