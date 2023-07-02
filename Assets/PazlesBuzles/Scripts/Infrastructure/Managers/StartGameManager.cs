using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    [SerializeField]
    private Image PrevImage;
    [SerializeField]
    private GameObject PiecesParent;
    [SerializeField]
    private GameObject PazzleParent;

    private GameState _gameInfo;

    void Awake()
    {
        _gameInfo = AllServices.GetService<GameState>();
    }

    private void Start()
    {
        PrevImage.sprite = _gameInfo.OrigImage;
        var pazzleSprites = _gameInfo.PazzleSprites;
        BuildPiecesAndPlaces(pazzleSprites);
    }

    private void BuildPiecesAndPlaces(List<Sprite> pazzleSprites)
    {
        Rect textureSize = pazzleSprites[0].textureRect;
        Vector2 PazzlePieceSize = new(textureSize.width, textureSize.height);
        LoadLevelState loadInfo = AllServices.GetService<LoadLevelState>();

        for (int i = 0; i < pazzleSprites?.Count; i++)
        {
            PazzlePiece piece = BuildPiece(pazzleSprites[i], i);
            Vector2 PazzleColliderSize = piece.GetComponent<BoxCollider2D>().size;
            BuildPlace(PazzlePieceSize, loadInfo, i, PazzleColliderSize);
        }
    }

    private void BuildPlace(Vector2 PazzlePieceSize, LoadLevelState loadInfo, int id, Vector2 PazzleColliderSize)
    {
        int x = id % loadInfo.PazzleCountOnX;
        int y = id / loadInfo.PazzleCountOnY;
        Vector2 pos = new(x * PazzleColliderSize.x, y * PazzleColliderSize.y);

        AllServices.GetService<FactoryPiecePlace>().BuildPiecePlace(
           pos, id, (int)PazzlePieceSize.x, (int)PazzlePieceSize.y, PazzleParent.transform);
    }

    private PazzlePiece BuildPiece(Sprite pazzleSprite, int id)
        => AllServices.GetService<FactoryPazzlePiece>().BuildPazzlePiece(
            new Vector2(Random.Range(-2, 2), Random.Range(-4, 4)),
            pazzleSprite, id, PiecesParent.transform, Quaternion.Euler(0f, 0f, 90f * Random.Range(0, 4)));
}
