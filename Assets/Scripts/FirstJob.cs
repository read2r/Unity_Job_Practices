using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public struct MyJob : IJob
{
    public float a;
    public float b;
    public NativeArray<float> result;

    public void Execute()
    {
        result[0] = a + b;
    }
}

public class FirstJob : MonoBehaviour
{
    private NativeArray<float> result;

    private void Awake()
    {
        result = new NativeArray<float>(1, Allocator.TempJob);
    }

    // Start is called before the first frame update
    private void Start()
    {
        MyJob jobData = new MyJob();

        jobData.a = 10;
        jobData.b = 10;
        jobData.result = result;

        JobHandle handle = jobData.Schedule();
        handle.Complete();

        float aPlusB = result[0];
        result.Dispose();

        Debug.Log(aPlusB);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
