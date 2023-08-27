using UnityEngine;
using Common;

namespace FormationSystem
{
    public interface IFormationMember
    {
        public int PositionIndex { get; set; }// position_0(leader).....

        public Vector3 Position { get; set; } //The relative position to formation leader.

        public IRelativePositionProvider Transform { get; }

        public Formation Formation { get; set; }
    }
}
