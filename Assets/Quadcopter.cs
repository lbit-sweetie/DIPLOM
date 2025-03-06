using UnityEngine;

// Пример конкретной реализации дрона
public class Quadcopter : DroneBase
{
    [Header("Quadcopter Specific")]
    [SerializeField] private float rotorSpeed = 1000f;
    [SerializeField] private Transform[] rotors;

    protected override void Awake()
    {
        base.Awake();
        flightBehavior = new ForwardFlight();
    }

    private void Update()
    {
        if (rotors.Length != 0)
            UpdateRotors();
    }

    private void UpdateRotors()
    {
        foreach (var rotor in rotors)
        {
            rotor.Rotate(Vector3.up, rotorSpeed * Time.deltaTime);
        }
    }
}