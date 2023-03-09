using UnityEngine;

namespace FormationSystem
{
    public class FormationMember
    {
        public int PositionIndex { get; set; }// position_0(leader).....

        public Vector3 Position { get; set; } //The relative position to formation leader.
    }
}
