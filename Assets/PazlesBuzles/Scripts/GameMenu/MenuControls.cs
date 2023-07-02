using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour, IService
{
    public int CountSuccessPiece
    {
        get => _countSuccessPiece;
        set
        {
            _countSuccessPiece = value;
            CheckWinState();
        }
    }

    [SerializeField]
    private GameObject MenuPanel;
    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private GameObject StartPanel;
    [SerializeField]
    private TextMeshProUGUI TimeText;
    [SerializeField]
    private Animator PrevImageAnimator;
    [SerializeField]
    private AudioClip StartSound;
    [SerializeField]
    private AudioClip WinSound;

    private Coroutine _timerCorounite;
    private int _curTime;
    private int _scalePrevImgAnimHash;
    private int _unscalePrevImgAnimHash;
    private int _countSuccessPiece;
    private int _maxSuccessPiece;
    private bool PrevImgIsScaled;

    private void Awake()
    {
        Init();
        InitAnimHash();
    }

    private void Start()
    {
        _timerCorounite = StartCoroutine(Timer());
        StartCoroutine(ShowPreview());
    }

    public void SetPause(bool isPause)
    {
        if (isPause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        MenuPanel.SetActive(isPause);
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ViewPrevImage()
    {
        if (PrevImgIsScaled)
            PrevImageAnimator.Play(_unscalePrevImgAnimHash);
        else
            PrevImageAnimator.Play(_scalePrevImgAnimHash);
        PrevImgIsScaled = !PrevImgIsScaled;
    }

    public void ButtonClick()
    {
        AllServices.GetService<SoundManager>().PlayClickBtn();
    }

    private void Init()
    {
        AllServices.RegisterService(this);
        AllServices.GetService<Boostraper>().StateMachine.StateSwitch<GameState>();
        SoundManager.PlaySfx_(StartSound);
        _maxSuccessPiece = AllServices.GetService<GameState>().PazzleSprites.Count;
    }

    private void InitAnimHash()
    {
        _scalePrevImgAnimHash = Animator.StringToHash("ScaleIn");
        _unscalePrevImgAnimHash = Animator.StringToHash("ScaleOut");
    }

    private void CheckWinState()
    {
        if (CountSuccessPiece == _maxSuccessPiece)
        {
            WinPanel.SetActive(true);
            Time.timeScale = 0;
            SoundManager.PlaySfx_(WinSound);
        }
    }

    private string FormatTime(int seconds)
    {
        int hours = seconds / 3600;
        int minutes = (seconds % 3600) / 60;
        int remainingSeconds = seconds % 60;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, remainingSeconds);
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            TimeText.text = $"Time: {FormatTime(_curTime)}";
            yield return new WaitForSeconds(1f);
            _curTime++;
        }
    }

    private IEnumerator ShowPreview()
    {
        StartPanel.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        StartPanel.SetActive(false);
    }
}
