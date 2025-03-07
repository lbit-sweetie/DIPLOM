// Базовый класс данных дрона (шаблон ScriptableObject для создания различных конфигураций)

using UnityEngine;

[CreateAssetMenu(menuName = "Drones/Drone Data")]
public class DroneData : ScriptableObject
{
    [Header("Movement Settings")] public DroneMovementType movementType = DroneMovementType.Straight;
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float rotationSpeed = 5f;
    public float weight = 1f;

    [Header("ZigZag Settings")] public float zigZagAmplitude = 2f; // Амплитуда отклонений
    public float zigZagFrequency = 1f; // Частота отклонений
}