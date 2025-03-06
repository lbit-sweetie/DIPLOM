// Интерфейс для поведения полета
using UnityEngine;

public interface IFlightBehavior
{
    void UpdateFlight(DroneBase drone, Rigidbody rb, DroneData data);
}