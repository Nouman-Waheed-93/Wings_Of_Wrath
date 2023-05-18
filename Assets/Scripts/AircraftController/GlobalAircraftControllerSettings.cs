namespace AircraftController
{
    public static class GlobalAircraftControllerSettings
    {
        public const float finalApproachAcceptableDeviation = 200f;
        public const float touchDownAcceptableDeviation = 200f;
        public const float wayPointReachedDistance = 20f;
        public const float retractGearAltitude = 20f; //The altitude at which the landing gear will be retracted
        public const float airborneAltitude = 30f; //The altitude at which the aircraft will be considered airborne
        public const float flightAltitude = 100f;
        public const float finalApproachSpeed = 50f;
        public const float touchDownSpeed = 30f;
        // if the player reaches initial approach with higher angle error than this, Landing procedure will not start.
        //Angle Error means the Angle between the correct initial approach direction and aircraft direction.
        public const float maxAngleErrorOnInitialApproach = 45f; 
    }
}
