using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class PazzlePiece : MonoBehaviour
{
    public int ID { get; set; }
    public SpriteRenderer Image => _image;

    private SpriteRenderer _image;
    private BaseControls _baseControls;
    private LayerMask _tableMask;
    private GameState _gameInfo;

    private void Awake()
    {
        _image = GetComponent<SpriteRenderer>();
        _baseControls = new BaseControls();
        _tableMask = AllServices.Instance.GetService<Boostraper>().TableMask;
        _gameInfo = AllServices.Instance.GetService<Boostraper>().StateMachine.GetGameState<GameState>();

        _baseControls.Player.Rotate.performed += _ => Rotate();
    }

    private void OnMouseEnter()
    {

    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        _baseControls.Enable();
        GetComponent<BoxCollider2D>().enabled = false;
        _gameInfo.PieceIsTaken = true;
        StartCoroutine(DisconnectCorutine());
    }

    private void OnMouseUp()
    {
        _baseControls.Disable();
        GetComponent<BoxCollider2D>().enabled = true;
        _gameInfo.PieceIsTaken = false;
        ConnectSelf();
    }

    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(_baseControls.Player.MousePos.ReadValue<Vector2>());
        mousePos.z = 0;
        transform.localPosition = mousePos - transform.parent.position;
    }

    private void ConnectSelf()
    {
        if (_gameInfo.SelectedPlace != null && _gameInfo.SelectedPlace.ConnectedPiece == null)
        {
            _gameInfo.SelectedPlace.ConnectedPiece = this;
            var pos = _gameInfo.SelectedPlace.transform.position;
            pos.z = transform.position.z;
            transform.position = pos;
        }
    }

    private void DisconnectSelf()
    {
        if (_gameInfo.SelectedPlace != null && _gameInfo.SelectedPlace.ConnectedPiece == this)
        {
            _gameInfo.SelectedPlace.ConnectedPiece = null;
        }
    }

    private IEnumerator DisconnectCorutine()
    {
        //yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.05f);
        DisconnectSelf();
    }

    private void Rotate()
    {
        float mouseScroll = Mathf.Clamp(_baseControls.Player.Rotate.ReadValue<float>(), -1, 1);
        transform.Rotate(new Vector3(0, 0, 90 * mouseScroll));
    }
}
