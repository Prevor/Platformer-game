using UnityEngine;
using UnityEngine.Profiling;
using System.IO;
using System.Collections.Generic;
using Unity.Profiling;

public class DataCollector : MonoBehaviour
{
    private float dataCollectionInterval = 30.0f;
    private float nextDataCollectionTime;
    private string logFilePath;

    private ProfilerRecorder cpuUsageRecorder;

    [System.Serializable]
    public class PerformanceData
    {
        public float timestamp;
        public float avgFps;
        public long avgTotalMemory;
        public long avgReservedMemory;
        public long avgMonoMemory;
        public float avgCpuUsage;
    }

    [System.Serializable]
    public class PerformanceDataList
    {
        public List<PerformanceData> data = new List<PerformanceData>();
    }

    private PerformanceDataList performanceDataList = new PerformanceDataList();

    private List<float> fpsValues = new List<float>();
    private List<long> totalMemoryValues = new List<long>();
    private List<long> reservedMemoryValues = new List<long>();
    private List<long> monoMemoryValues = new List<long>();
    private List<float> cpuUsageValues = new List<float>();

    void Start()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "performance_log.json");
        Debug.Log("DataCollector log file path: " + logFilePath);
        if (File.Exists(logFilePath))
        {
            string json = File.ReadAllText(logFilePath);
            performanceDataList = JsonUtility.FromJson<PerformanceDataList>(json);
        }

        cpuUsageRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", (int)dataCollectionInterval);
    }

    void Update()
    {
        fpsValues.Add(1.0f / Time.deltaTime);
        totalMemoryValues.Add(Profiler.GetTotalAllocatedMemoryLong());
        reservedMemoryValues.Add(Profiler.GetTotalReservedMemoryLong());
        monoMemoryValues.Add(Profiler.GetMonoUsedSizeLong());
        cpuUsageValues.Add(GetCPUUsage());

        if (Time.time >= nextDataCollectionTime)
        {
            CollectData();
            nextDataCollectionTime = Time.time + dataCollectionInterval;
        }
    }

    void OnDisable()
    {
        cpuUsageRecorder.Dispose();
    }

    void CollectData()
    {
        PerformanceData data = new PerformanceData();
        data.timestamp = Time.time;
        data.avgFps = CalculateAverage(fpsValues);
        data.avgTotalMemory = (long)CalculateAverage(totalMemoryValues);
        data.avgReservedMemory = (long)CalculateAverage(reservedMemoryValues);
        data.avgMonoMemory = (long)CalculateAverage(monoMemoryValues);
        data.avgCpuUsage = CalculateAverage(cpuUsageValues);

        performanceDataList.data.Add(data);
        SaveData();

        fpsValues.Clear();
        totalMemoryValues.Clear();
        reservedMemoryValues.Clear();
        monoMemoryValues.Clear();
        cpuUsageValues.Clear();
    }

    float CalculateAverage(List<float> values)
    {
        if (values.Count == 0) return 0.0f;
        float total = 0.0f;
        foreach (float value in values)
        {
            total += value;
        }
        return total / values.Count;
    }

    long CalculateAverage(List<long> values)
    {
        if (values.Count == 0) return 0;
        long total = 0;
        foreach (long value in values)
        {
            total += value;
        }
        return total / values.Count;
    }

    float GetCPUUsage()
    {
        if (cpuUsageRecorder.Valid)
        {
            return cpuUsageRecorder.LastValue / (float)SystemInfo.processorCount;
        }
        return 0.0f;
    }

    void SaveData()
    {
        string json = JsonUtility.ToJson(performanceDataList, true);
        File.WriteAllText(logFilePath, json);
        Debug.Log("Data Saved: " + json);
    }
}
