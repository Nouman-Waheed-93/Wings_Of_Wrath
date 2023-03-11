using UnityEngine;

namespace FormationSystem
{
    /* Formation Design
     *      0   -layer 0
     *     1 2  -layer 1
     *    3   4 -layer 2
     */
    public class ArrowHead : BalancedFormation
    {
        public override Vector3 GetMemberPosition(int memberIndex)
        {
            CheckIndexValidity(memberIndex);

            if (memberIndex == 0)
                return Vector3.zero;

            if (memberIndex % 2 == 0)
                return new Vector3(-0.707f, 0, -0.707f) * GetMemberLayer(memberIndex, true);
            else
                return new Vector3(0.707f, 0, -0.707f) * GetMemberLayer(memberIndex, false);
        }
    }
}
