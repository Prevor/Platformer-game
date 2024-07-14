using UnityEngine;
using UnityEngine.Profiling;
using System.IO;
using System.Collections.Generic;
using Unity.Profiling;

public class DataCollector : MonoBehaviour
{
    private float dataCollectionInterval = 30.0f; // ???????? ????? ????? ? ????????
    private float nextDataCollectionTime;
    private string logFilePath;

    private ProfilerRecorder cpuUsageRecorder;

    [System.Serializable]
    public class PerformanceData
    {
        public float timestamp;
        public float avgFps;
        public long totalMemory;
        public long reservedMemory;
        public long monoMemory;
        public float cpuUsage;
    }

    [System.Serializable]
    public class PerformanceDataList
    {
        public List<PerformanceData> data = new List<PerformanceData>();
    }

    private PerformanceDataList performanceDataList = new PerformanceDataList();

    // ?????? ??? ?????????? ??????? FPS
    private List<float> fpsValues = new List<float>();

    void Start()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "performance_log.json");
        Debug.Log("DataCollector log file path: " + logFilePath);
        if (File.Exists(logFilePath))
        {
            string json = File.ReadAllText(logFilePath);
            performanceDataList = JsonUtility.FromJson<PerformanceDataList>(json);
        }

        // ????????????? ?????????? ??? ????? ????? ??? ???????????? ?????????
       // cpuUsageRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Cpu, "Main Thread");
    }

    void Update()
    {
        // ??????? ??????? ???????? FPS ?? ??????
        fpsValues.Add(1.0f / Time.deltaTime);

        if (Time.time >= nextDataCollectionTime)
        {
            CollectData();
            nextDataCollectionTime = Time.time + dataCollectionInterval;
        }
    }

    void CollectData()
    {
        PerformanceData data = new PerformanceData();
        data.timestamp = Time.time;
        data.avgFps = CalculateAverageFps();
        data.totalMemory = Profiler.GetTotalAllocatedMemoryLong();
        data.reservedMemory = Profiler.GetTotalReservedMemoryLong();
        data.monoMemory = Profiler.GetMonoUsedSizeLong();
        data.cpuUsage = GetCPUUsage();

        performanceDataList.data.Add(data);
        SaveData();

        // ???????? ?????? ??????? FPS ????? ????? ?????
        fpsValues.Clear();
    }

    float CalculateAverageFps()
    {
        if (fpsValues.Count == 0) return 0.0f;
        float totalFps = 0.0f;
        foreach (float fps in fpsValues)
        {
            totalFps += fps;
        }
        return totalFps / fpsValues.Count;
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
