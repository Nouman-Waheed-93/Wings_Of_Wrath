using UnityEngine;

namespace FormationSystem
{
    public interface IFormationMember
    {
        public int PositionIndex { get; set; }// position_0(leader).....

        public Vector3 Position { get; set; } //The relative position to formation leader.
    }
}
