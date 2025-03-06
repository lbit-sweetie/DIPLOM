// Базовый класс данных дрона (шаблон ScriptableObject для создания различных конфигураций)
using UnityEngine;

[CreateAssetMenu(menuName = "Drones/Drone Data")]
public class DroneData : ScriptableObject
{
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float rotationSpeed = 5f;
    public float weight = 1f;
    public Vector3 silhouetteScale = Vector3.one;
    public Material defaultMaterial;
}