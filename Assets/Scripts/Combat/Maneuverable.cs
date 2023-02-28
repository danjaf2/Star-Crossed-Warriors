public interface Maneuverable {
    /// <summary>
    /// Controls the maneuverable entity.
    /// </summary>
    public void Maneuver(Maneuver maneuver);
}

public struct Maneuver {
    public float Yaw;
    public float Roll;
    public float Pitch;
    public float Thrust;

    public Maneuver(float pitch = 0f, float roll = 0f, float yaw = 0f, float thrust = 0f) {
        Yaw = yaw;
        Roll = roll;
        Pitch = pitch;
        Thrust = thrust;
    }
}