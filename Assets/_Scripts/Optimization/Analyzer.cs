using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Analyzer : MonoBehaviour
{
    private float analysisInterval = 30.0f;
    private float nextAnalysisTime = 0.0f;

    private string logFilePath;
    private Optimizer optimizer;

    void Start()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "performance_log.json");
        Debug.Log("Analyzer log file path: " + logFilePath);
        optimizer = GetComponent<Optimizer>();
    }

    void Update()
    {
        if (Time.time >= nextAnalysisTime)
        {
            AnalyzeData();
            nextAnalysisTime = Time.time + analysisInterval;
        }
    }

    void AnalyzeData()
    {
        if (File.Exists(logFilePath))
        {
            string json = File.ReadAllText(logFilePath);
            var performanceDataList = JsonUtility.FromJson<DataCollector.PerformanceDataList>(json);

            if (performanceDataList.data.Count > 0)
            {
                var lastData = performanceDataList.data[performanceDataList.data.Count - 1];
                AnalyzePerformance(lastData.avgFps, lastData.avgTotalMemory, lastData.avgReservedMemory, lastData.avgMonoMemory, lastData.avgCpuUsage);
            }
        }
    }   

    void AnalyzePerformance(float avgFps, long avgTotalMemory, long avgReservedMemory, long avgMonoMemory, float avgCpuUsage)
    {
        optimizer.Optimize(avgFps, avgTotalMemory, avgReservedMemory, avgMonoMemory, avgCpuUsage);
    }
}
