using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public struct AddOneJob : IJob
{
    public NativeArray<float> result;

    public void Execute()
    {
        result[0] = result[0] + 1;
    }
}

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

    // Start is called before the first frame update
    private void Start()
    {
        result = new NativeArray<float>(1, Allocator.TempJob);

        MyJob jobData = new MyJob();
        jobData.a = 10;
        jobData.b = 10;
        jobData.result = result;

        AddOneJob IncJobData = new AddOneJob();
        IncJobData.result = result;

        JobHandle firstHandle = jobData.Schedule();
        // the result of AddOneJob has dependency on the result of MyJob;
        JobHandle secondHandle = IncJobData.Schedule(firstHandle);
        secondHandle.Complete();

        float aPlusB = result[0];
        Debug.Log(aPlusB);

        result.Dispose();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
