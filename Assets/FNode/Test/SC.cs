using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SC",menuName ="SCC")]
public class SC : ScriptableObject
{
    public MyType tp;
}


[Serializable]
public class MyType
{
    public TextAsset cur;
}