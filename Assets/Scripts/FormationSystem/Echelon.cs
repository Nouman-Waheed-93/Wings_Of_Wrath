using UnityEngine;

namespace FormationSystem
{
    public class Echelon : Formation
    {
        public override Vector3 GetMemberPosition(int memberIndex)
        {
            return new Vector3(0.707f * memberIndex, 0, -0.707f * memberIndex);
        }
    }
}
