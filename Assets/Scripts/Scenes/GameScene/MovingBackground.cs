using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float _floatingSpeed = 0.001f;

    [SerializeField] private PlayerController _playerController;

    private Vector2 _currentOffset = new Vector2(0,0);
   
    private void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _material = GetComponent<SpriteRenderer>().material;
        _playerController.OnPlayerMove += MoveBackgound;
    }

    private void MoveBackgound(Vector2 dir, float moveSpeed)
    {
        _currentOffset += dir * _floatingSpeed * Time.deltaTime * moveSpeed;
        _material.SetTextureOffset("_MainTex", _currentOffset);
    }

    private void Reset()
    {
        _material = GetComponent<SpriteRenderer>().material;

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {

            _playerController = playerController;
        }
    }

    private void OnDestroy()
    {
        if(_playerController != null)
            _playerController.OnPlayerMove -= MoveBackgound;
    }
}