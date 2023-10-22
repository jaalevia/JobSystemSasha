using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct LogJob : IJob
{

    public NativeArray<int> RndNum;
    public void Execute()
    {
        for (int i = 0; i < RndNum.Length; i++)
        {
            Debug.Log(Math.Log(RndNum[i]));
        }
    }

}
