using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class PiecePlace : MonoBehaviour
{
    public int ID { get; set; }
    public PazzlePiece ConnectedPiece
    {
        get => _connectedPiece;
        set
        {
            UpdateCountOnConnectionChange(value);
            _connectedPiece = value;
        }
    }

    private Color _baseColor;
    private PazzlePiece _connectedPiece;
    private SpriteRenderer _renderer;
    private GameState _gameInfo;

    private void Awake()
    {
        _gameInfo = AllServices.GetService<GameState>();
        _renderer = GetComponent<SpriteRenderer>();
        _baseColor = _renderer.color;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_gameInfo.PieceIsTaken)
            _renderer.color = Color.green;
        _gameInfo.SelectedPlace = this;
    }

    private void OnMouseExit()
    {
        _renderer.color = _baseColor;
        _gameInfo.SelectedPlace = null;
    }

    public void Init(int id, int width, int height)
    {
        ID = id;
        SetSize(width, height);
    }

    public void SetSize(int width, int height)
    {
        _renderer.sprite = Sprite.Create(new Texture2D(width, height), new Rect(0, 0, width, height), Vector2.one * 0.5f);
        //_renderer.size = new Vector2(width, height);

        BoxCollider2D collider;
        if ((collider = gameObject.GetComponent<BoxCollider2D>()) != null)
            Destroy(collider);
        gameObject.AddComponent<BoxCollider2D>();
    }

    private void UpdateCountOnConnectionChange(PazzlePiece value)
    {
        var menuControl = AllServices.GetService<MenuControls>();
        if (value == null)
        {
            if (ID == _connectedPiece.ID && ApproximatelyWithZero(_connectedPiece.transform.rotation.z))
                menuControl.CountSuccessPiece--;
        }
        else if (ID == value.ID && ApproximatelyWithZero(value.transform.rotation.z))
            menuControl.CountSuccessPiece++;
    }

    private bool ApproximatelyWithZero(float val) => val > -0.00001f && val < 0.00001f;

}
