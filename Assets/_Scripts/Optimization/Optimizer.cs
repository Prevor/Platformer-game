using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    private float targetFPS = 60.0f;
    private float cpuThreshold = 75.0f;
    private long memoryThreshold = 500000000;

    public void Optimize(float fps, long totalMemory, long reservedMemory, long monoMemory, float cpuUsage)
    {
        if (fps < targetFPS)
        {
            OptimizeForFPS();
        }

        if (totalMemory > memoryThreshold)
        {
            OptimizeForMemory();
        }

        if (cpuUsage > cpuThreshold)
        {
            OptimizeForCPU();
        }
    }

    void OptimizeForFPS()
    {
        QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel() - 1);
        Debug.Log("Optimizing for FPS: Reducing quality level");
    }

    void OptimizeForMemory()
    {
        Resources.UnloadUnusedAssets();
        Debug.Log("Optimizing for Memory: Unloading unused assets");
    }

    void OptimizeForCPU()
    {
        ReduceActiveEffects();
        Debug.Log("Optimizing for CPU: Reducing active effects");
    }

    void ReduceActiveEffects()
    {
        // Реалізація зменшення кількості активних ефектів
        // Наприклад, вимкнення частини частинкових систем або інших ресурсомістких об'єктів
    }
}
