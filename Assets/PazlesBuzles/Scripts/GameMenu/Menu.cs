using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour, IService
{
    public int CountSuccessPiece
    {
        get => _countSuccessPiece;
        set
        {
            _countSuccessPiece = value;
            if (CountSuccessPiece == _maxSuccessPiece)
            {
                WinPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    [SerializeField]
    private GameObject MenuPanel;
    [SerializeField]
    private GameObject WinPanel;
    [SerializeField]
    private TextMeshProUGUI TimeText;
    [SerializeField]
    private Animator _prevImageAnimator;

    private Coroutine _timerCorounite;
    private int _curTime;
    private int _scalePrevImgAnimHash;
    private int _unscalePrevImgAnimHash;
    private int _countSuccessPiece;
    private int _maxSuccessPiece;
    private bool PrevImgIsScaled;


    private void Awake()
    {
        AllServices.Instance.RegisterService(this);
        _scalePrevImgAnimHash = Animator.StringToHash("ScaleIn");
        _unscalePrevImgAnimHash = Animator.StringToHash("ScaleOut");
        _maxSuccessPiece = AllServices.Instance.GetService<Boostraper>().StateMachine.GetGameState<GameState>().PazzleSprites.Count;
    }

    private void Start()
    {
        _timerCorounite = StartCoroutine(Timer());
    }

    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            //StopCoroutine(_timerCorounite);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            //_timerCorounite = StartCoroutine(Timer());
        }
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
        {
            _prevImageAnimator.Play(_unscalePrevImgAnimHash);
        }
        else
        {
            _prevImageAnimator.Play(_scalePrevImgAnimHash);
        }
        PrevImgIsScaled = !PrevImgIsScaled;
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            TimeText.text = $"Time: {_curTime}";
            yield return new WaitForSeconds(1f);
            _curTime++;
        }
    }
}
