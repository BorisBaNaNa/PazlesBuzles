using UnityEngine;

public class FactoryPiecePlace : IService
{
    public PiecePlace PiecePlacePrefab;

    public FactoryPiecePlace(PiecePlace piecePlacePrefab)
    {
        PiecePlacePrefab = piecePlacePrefab;
    }

    public PiecePlace BuildPiecePlace(Vector3 at, int id, int width, int height, Transform parent = null)
    {
        var piecePlace = Object.Instantiate(PiecePlacePrefab, parent);
        piecePlace.transform.localPosition = at;
        piecePlace.transform.rotation = Quaternion.identity;
        piecePlace.Init(id, width, height);
        return piecePlace;
    }
}
