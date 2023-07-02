using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectDifficultyPanel;
    [SerializeField]
    private GameObject ImagesPanel;
    [SerializeField]
    private GameObject ImageBtnPrefab;
    [SerializeField]
    private Settings Settings;

    private GameStateMachine _gameStateMachine;
    private Sprite _pazzleImage;

    private void Awake()
    {
        _gameStateMachine = AllServices.GetService<Boostraper>().StateMachine;
        _gameStateMachine.StateSwitch<MainMenuState>();
    }

    private void Start()
    {
        Settings.LoadSettings();
    }

    public void SelectImage(Sprite image)
    {
        SelectDifficultyPanel.SetActive(true);
        _pazzleImage = image;
    }

    public void LoadImageFile()
    {
        OpenFileName openFileName = new OpenFileName();
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            byte[] imageData = System.IO.File.ReadAllBytes(openFileName.file);
            Texture2D texture = new(2, 2);
            texture.LoadImage(imageData);

            int textureWidth = texture.width;
            int textureHeight = texture.height;

            texture = new Texture2D(textureWidth, textureHeight);
            texture.LoadImage(imageData);

            BuildButton(texture);
        };
    }

    public void StartGame(int count)
    {
        var loadState = _gameStateMachine.GetGameState<LoadLevelState>();
        loadState.InitLoadInfo(count, _pazzleImage);
        _gameStateMachine.StateSwitch<LoadLevelState>();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ButtonClick()
    {
        AllServices.GetService<SoundManager>().PlayClickBtn();
    }

    private void BuildButton(Texture2D texture)
    {
        GameObject imageBtn = Instantiate(ImageBtnPrefab, Vector2.zero, Quaternion.identity, ImagesPanel.transform);
        Image btnImage = imageBtn.GetComponent<Image>();

        btnImage.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.one * 0.5f);
        imageBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            SelectImage(btnImage.sprite);
            ButtonClick();
        });
    }
}
