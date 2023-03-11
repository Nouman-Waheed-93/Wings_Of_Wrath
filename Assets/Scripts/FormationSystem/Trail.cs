using UnityEngine;

namespace FormationSystem
{
    public class Trail : Formation
    {
        public override Vector3 GetMemberPosition(int memberIndex)
        {
            return new Vector3(0, 0, -memberIndex);
        }
    }
}
