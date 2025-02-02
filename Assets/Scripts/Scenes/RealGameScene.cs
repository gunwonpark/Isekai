using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealGameScene : MonoBehaviour
{
    [SerializeField] private RealGameFactory _realGameFactory;

    private void Start()
    {
        RealWorldInfo realWorldInfo = new FirstWorldInfo();
        Init(realWorldInfo);
    }
    public void Init(RealWorldInfo realWorldInfo)
    {
        _realGameFactory.Init(realWorldInfo);
    }
}
