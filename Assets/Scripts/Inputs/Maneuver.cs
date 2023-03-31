public struct Maneuver {
    public float Yaw;
    public float Roll;
    public float Pitch;
    public float Thrust;

    //public bool Shoot;
    //public bool Missile;
    //public bool Ability;

    public Maneuver(float pitch, float roll, float yaw, float thrust) {
        Yaw = yaw;
        Roll = roll;
        Pitch = pitch;
        Thrust = thrust;

        //Shoot = false;
        //Missile = false;
        //Ability = false;
    }

    //public Maneuver(float pitch, float roll, float yaw, float thrust, bool shoot, bool missile, bool ability) {
    //    Yaw = yaw;
    //    Roll = roll;
    //    Pitch = pitch;
    //    Thrust = thrust;

    //    Shoot = shoot;
    //    Missile = missile;
    //    Ability = ability;
    //}
}