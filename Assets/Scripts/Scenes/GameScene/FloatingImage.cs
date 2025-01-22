using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingImage : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float _floatingSpeed = 0.05f;
    
    private void Update()
    {
        _material.SetTextureOffset("_MainTex", Vector2.left * _floatingSpeed * Time.time);
    }

    private void Reset()
    {
        _material = GetComponent<SpriteRenderer>().material;
        _material.SetTextureOffset("_MainTex", Vector2.zero);
    }
}
