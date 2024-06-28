using UnityEngine;

namespace FormationSystem
{
    public class Echelon : Formation
    {
        public override Vector3 GetMemberPosition(int memberIndex)
        {
            CheckIndexValidity(memberIndex);

            return new Vector3(0.707f * memberIndex, -1, -0.707f * memberIndex);
        }
    }
}
