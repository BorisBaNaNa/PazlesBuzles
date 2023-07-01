using UnityEngine;

public class FactoryPazzlePiece : IService
{
    public PazzlePiece BuildPazzlePiece(Vector3 at, Sprite sprite, int id, Transform parent, Quaternion rotation)
    {
        var piece = new GameObject($"PazzlePiece_{id}").AddComponent<PazzlePiece>();
        piece.transform.SetParent(parent);
        piece.transform.localPosition = at;
        piece.transform.localRotation = rotation;
        piece.Image.sprite = sprite;
        piece.ID = id;
        piece.gameObject.AddComponent<BoxCollider2D>();
        return piece;
    }
}
