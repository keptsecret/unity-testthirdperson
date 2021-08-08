using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapGenerator
{
    public void GenerateMap();

    // does c# have pointers?
    public int[,] GetMap();
}
