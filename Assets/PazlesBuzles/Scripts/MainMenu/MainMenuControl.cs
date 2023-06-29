using System.Runtime.InteropServices;
using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    private GameStateMachine _gameStateMachine;
    private Sprite _pazzleImage;

    private void Awake()
    {
        _gameStateMachine = AllServices.Instance.GetService<Boostraper>().StateMachine;
    }

    public void SelectImage(Sprite image)
    {
        _pazzleImage = image;
    }

    public void LoadImageFile()
    {
        OpenFileName openFileName = new OpenFileName();
        if (LocalDialog.GetOpenFileName(openFileName))
        {
            byte[] imageData = System.IO.File.ReadAllBytes(openFileName.file);
            Texture2D texture = new Texture2D(1080, 1080);
            texture.LoadImage(imageData);
            _pazzleImage = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(1080, 1080)), Vector2.one * 0.5f);
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
}
