using UnityEngine;

namespace FormationSystem
{
    public class BattleSpread : BalancedFormation
    {
        public override Vector3 GetMemberPosition(int memberIndex)
        {
            CheckIndexValidity(memberIndex);

            if (memberIndex == 0)
                return Vector3.zero;

            if (memberIndex % 2 == 0)
                return new Vector3(-1, 0, 0) * GetMemberLayer(memberIndex, true);
            else
                return new Vector3(1, 0, 0) * GetMemberLayer(memberIndex, false);
        }
    }
}
