using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    private GameState _gameInfo;

    void Awake()
    {
        _gameInfo = AllServices.Instance.GetService<Boostraper>().StateMachine.GetGameState<GameState>();
    }

    private void Start()
    {
        GameObject gameObject = new GameObject("OriginalImage");
        gameObject.AddComponent<SpriteRenderer>().sprite = _gameInfo.OrigImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
