using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class StartGameManager : MonoBehaviour
{
    [SerializeField]
    private Image PrevImage;
    [SerializeField]
    private GameObject PiecesBase;
    [SerializeField]
    private GameObject PazzleParent;

    private GameState _gameInfo;

    void Awake()
    {
        _gameInfo = AllServices.Instance.GetService<Boostraper>().StateMachine.GetGameState<GameState>();
    }

    private void Start()
    {
        PrevImage.sprite = _gameInfo.OrigImage;
        var pazzleSprites = _gameInfo.PazzleSprites;

        BuildPieces(pazzleSprites);
    }

    private void BuildPieces(List<Sprite> pazzleSprites)
    {
        Vector2 PazzlePieceSize = new(pazzleSprites[0].textureRect.width, pazzleSprites[0].textureRect.height);
        LoadLevelState loadInfo = AllServices.Instance.GetService<Boostraper>().StateMachine.GetGameState<LoadLevelState>();

        for (int i = 0; i < pazzleSprites?.Count; i++)
        {
            var piece = AllServices.Instance.GetService<FactoryPazzlePiece>().BuildPazzlePiece(
                new Vector2(Random.Range(-2, 2), Random.Range(-4, 4)),
                pazzleSprites[i], i, PiecesBase.transform, Quaternion.Euler(0f, 0f, 90f * Random.Range(0, 4)));

            Vector2 PazzleColliderSize = piece.GetComponent<BoxCollider2D>().size;
            int x = i % loadInfo.PazzleCountOnX;
            int y = i / loadInfo.PazzleCountOnY;
            Vector2 pos = new(x * PazzleColliderSize.x, y * PazzleColliderSize.y);

            AllServices.Instance.GetService<FactoryPiecePlace>().BuildPiecePlace(
               pos, i, (int)PazzlePieceSize.x, (int)PazzlePieceSize.y, PazzleParent.transform);
        }
    }
}
