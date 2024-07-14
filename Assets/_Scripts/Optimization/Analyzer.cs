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
        logFilePath = Path.Combine(Application.persistentDataPath, "performance_log.txt");
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
        string[] logLines = File.ReadAllLines(logFilePath);
        string lastLogLine = logLines[logLines.Length - 1];
        string[] logData = lastLogLine.Split(',');

        float fps = float.Parse(logData[1]);
        long totalMemory = long.Parse(logData[2]);
        long reservedMemory = long.Parse(logData[3]);
        long monoMemory = long.Parse(logData[4]);
        float cpuUsage = float.Parse(logData[5]);

        AnalyzePerformance(fps, totalMemory, reservedMemory, monoMemory, cpuUsage);
    }

    void AnalyzePerformance(float fps, long totalMemory, long reservedMemory, long monoMemory, float cpuUsage)
    {
        optimizer.Optimize(fps, totalMemory, reservedMemory, monoMemory, cpuUsage);
    }
}
