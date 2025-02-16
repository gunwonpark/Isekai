using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingImage : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float _floatingSpeed = 0.05f;
    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }
    private void Update()
    {
        _material.SetTextureOffset("_MainTex", Vector2.left * _floatingSpeed * Time.time);
    }

    private void Reset()
    {
        // 서로 다른 오브젝트에서 다른 값을 적용하고싶을 때
        _material = GetComponent<SpriteRenderer>().material;

        // 서로 다른 오브젝트에서 같은 값을 적용하고싶을 때
        // _material = GetComponent<SpriteRenderer>().sharedMaterial;

        _material.SetTextureOffset("_MainTex", Vector2.zero);
    }
}
