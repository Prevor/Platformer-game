using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    [SerializeField] private float targetFPS = 60.0f; // Цільовий FPS, який ми хочемо підтримувати
    private float cpuThreshold = 75.0f; // Поріг використання CPU, вище якого потрібно оптимізувати
    private long memoryThreshold = 500000000; // Поріг використання пам'яті (в байтах), вище якого потрібно оптимізувати

    [SerializeField] private Camera camera;
    [SerializeField] private float shadowDistanceIncreaseFactor = 1.1f;
    [SerializeField] private float shadowDistanceDecreaseFactor = 0.9f;
    [SerializeField] private float maxShadowDistance = 150.0f;
    [SerializeField] private float minShadowDistance = 10.0f;

    public void Optimize(float fps, long totalMemory, long reservedMemory, long monoMemory, float cpuUsage)
    {
        OptimizeForFPS(fps);

        if (totalMemory > memoryThreshold)
        {
            {
                OptimizeForMemory();
            }

            if (cpuUsage > cpuThreshold)
            {
                OptimizeForCPU();
            }
            else if (cpuUsage < cpuThreshold)
            {
                OptimizeForCPU(true); 
            }
        }

        void OptimizeForFPS(float fps)
        {
            if (fps < targetFPS)
            {
                QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel() - 1);
                Debug.Log("Optimizing for FPS: Reducing quality level");
            }
            else if (fps > targetFPS)
            {
                QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel() + 1);
                Debug.Log("Optimizing for FPS: Increasing quality level");
            }

            if (fps < 30)
            {
                DisableShadows();
                Debug.Log("Optimizing for FPS: Disabling shadows");
            }
            else if (fps >= 30 && QualitySettings.shadows == ShadowQuality.Disable)
            {
                EnableShadows();
                Debug.Log("Optimizing for FPS: Enabling shadows");
            }
        }

        void OptimizeForMemory()
        {
            // Вивільняємо невикористані ресурси для зменшення використання пам'яті
            Resources.UnloadUnusedAssets();
            Debug.Log("Optimizing for Memory: Unloading unused assets");
        }

        void OptimizeForCPU(bool increaseLoad = false)
        {
            // Оптимізуємо рівні деталізації (LOD) для зменшення або збільшення навантаження на CPU
            OptimizeLOD(increaseLoad);
            OptimizeShadowDistance(increaseLoad);
            if (increaseLoad)
            {
                Debug.Log("Optimizing for CPU: Increasing LOD levels and shadow distance");
            }
            else
            {
                Debug.Log("Optimizing for CPU: Reducing LOD levels and shadow distance");
            }
        }

        void OptimizeLOD(bool increaseLoad)
        {
            // Знаходимо всі LODGroup на сцені
            LODGroup[] lodGroups = FindObjectsOfType<LODGroup>();
            foreach (LODGroup lodGroup in lodGroups)
            {
                // Отримуємо всі рівні деталізації (LOD) для цього LODGroup
                LOD[] lods = lodGroup.GetLODs();
                for (int i = 0; i < lods.Length; i++)
                {
                    // Змінюємо висоту переходу для кожного рівня деталізації
                    if (lods[i].renderers.Length > 0)
                    {
                        if (increaseLoad)
                        {
                            lods[i].screenRelativeTransitionHeight /= 1.5f; // Зменшуємо висоту переходу для підвищення якості
                        }
                        else
                        {
                            lods[i].screenRelativeTransitionHeight *= 1.5f; // Збільшуємо висоту переходу для зниження якості
                        }
                    }
                }
                // Застосовуємо нові налаштування LOD до LODGroup
                lodGroup.SetLODs(lods);
                Debug.Log($"Optimized LOD for {lodGroup.gameObject.name}");
            }
        }

        void OptimizeShadowDistance(bool increaseLoad)
        {
            if (increaseLoad)
            {
                QualitySettings.shadowDistance = Mathf.Min(QualitySettings.shadowDistance * shadowDistanceIncreaseFactor, maxShadowDistance);
            }
            else
            {
                QualitySettings.shadowDistance = Mathf.Max(QualitySettings.shadowDistance * shadowDistanceDecreaseFactor, minShadowDistance);
            }
            Debug.Log($"Optimized Shadow Distance to {QualitySettings.shadowDistance}");
        }

        void DisableShadows()
        {
            QualitySettings.shadows = ShadowQuality.Disable;
        }

        void EnableShadows()
        {
            QualitySettings.shadows = ShadowQuality.All;
        }
    }
}
