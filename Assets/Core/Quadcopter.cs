using UnityEngine;

public class Quadcopter : DroneBase
{
    [Header("Quadcopter Specific")]
    [SerializeField] private Transform[] rotors;
    [SerializeField] private Vector3 directionRotors;
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
            rotor.Rotate(directionRotors, droneData.maxSpeed * 1000 * Time.deltaTime);
    }
}