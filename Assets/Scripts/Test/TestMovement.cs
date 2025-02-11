using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
   
    void Update()
    {
        float _inputX = Input.GetAxis("Horizontal");

        this.transform.position += new Vector3(_inputX, 0, 0) * Time.deltaTime * moveSpeed;
    }
}
