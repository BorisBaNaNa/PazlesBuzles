using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class LoadLevelState : IGameState
{
    private int _pazzleCountOnX= 0;
    private int _pazzleCountOnY = 0;
    private Sprite _pazzleImage;

    private readonly GameStateMachine _stateMachine;

    public LoadLevelState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void InitLoadInfo(int pazzleCountOnDiagonal, Sprite pazzleImage) 
        => InitLoadInfo(pazzleCountOnDiagonal, pazzleCountOnDiagonal, pazzleImage);
    public void InitLoadInfo(int pazzleCountOnX, int pazzleCountOnY, Sprite pazzleImage)
    {
        _pazzleCountOnX = pazzleCountOnX;
        _pazzleCountOnY = pazzleCountOnY;
        _pazzleImage = pazzleImage;
    }

    public void Enter()
    {
        var game = _stateMachine.GetGameState<GameState>();
        game.InitGameInfo(_pazzleImage, SlicePazzleImage());

        SceneManager.LoadScene("Game");
        _stateMachine.StateSwitch<GameState>();
    }

    private List<Sprite> SlicePazzleImage()
    {
        List<Sprite> sprites = new List<Sprite>();
        Texture2D origTexture = _pazzleImage.texture;
        float pazzleWidth = origTexture.width / _pazzleCountOnX;
        float pazzleHeight = origTexture.height / _pazzleCountOnY;

        for (float y = 0; y < origTexture.height; y += pazzleHeight)
        {
            for (float x = 0; x < origTexture.width; x += pazzleWidth)
            {
                Sprite pazzleSprite = Sprite.Create(origTexture, new Rect(x, y, pazzleWidth, pazzleHeight), Vector2.one * 0.5f);
                sprites.Add(pazzleSprite);
            }
        }
        return sprites;
    }

    public void Exit()
    {
    }

}
