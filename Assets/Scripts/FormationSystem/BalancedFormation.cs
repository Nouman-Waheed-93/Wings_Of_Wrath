using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormationSystem
{
    /*
     * Balanced Formation is a type of formation that keeps it's leader at the center and 
     * the both sides left and right are symmetrical
     */
    public abstract class BalancedFormation : Formation
    {
        public override void RemoveMember(FormationMember memberToRemove)
        {
            int removedMemberIndex = memberToRemove.PositionIndex;
            bool isRemovedMemberIndexEven = removedMemberIndex % 2 == 0;
            members.Remove(memberToRemove);

            int evenMemberCount = 0;
            int oddMemberCount = 0;
            FormationMember memberWithGreatestEvenIndex = null;
            FormationMember memberWithGreatestOddIndex = null;

            foreach (FormationMember member in members)
            {
                bool isEven = member.PositionIndex % 2 == 0;

                if (member.PositionIndex > removedMemberIndex)
                {
                    //if the removedmemberIndex and the current Index are both even or are both odd
                    if (isRemovedMemberIndexEven == isEven)
                    {
                        member.PositionIndex -= 2;
                        member.Position = GetMemberPosition(member.PositionIndex);
                    }
                }

                if (member.PositionIndex != 0)
                {
                    if (isEven)
                    {
                        evenMemberCount++;
                        memberWithGreatestEvenIndex = GetMemberWithGreaterIndex(memberWithGreatestEvenIndex, member);
                    }
                    else
                    {
                        oddMemberCount++;
                        memberWithGreatestOddIndex = GetMemberWithGreaterIndex(memberWithGreatestOddIndex, member);
                    }
                }
            }

            EvenOutBothSides(evenMemberCount, oddMemberCount, memberWithGreatestEvenIndex, memberWithGreatestOddIndex);
        }

        private FormationMember GetMemberWithGreaterIndex(FormationMember memberWithGreaterIndex, FormationMember member)
        {
            if (memberWithGreaterIndex == null)
                memberWithGreaterIndex = member;
            if (member.PositionIndex > memberWithGreaterIndex.PositionIndex)
                memberWithGreaterIndex = member;

            return memberWithGreaterIndex;
        }

        private void EvenOutBothSides(int evenMemberCount, int oddMemberCount,
            FormationMember memberWithGreatestEvenIndex, FormationMember memberWithGreatestOddIndex)
        {
            //If the difference between left and right side is more than 1
            if (Mathf.Abs(evenMemberCount - oddMemberCount) > 1)
            {
                if (evenMemberCount > oddMemberCount)
                {
                    memberWithGreatestEvenIndex.PositionIndex--;
                    memberWithGreatestEvenIndex.Position = GetMemberPosition(memberWithGreatestEvenIndex.PositionIndex);
                }
                else
                {
                    memberWithGreatestOddIndex.PositionIndex--;
                    memberWithGreatestOddIndex.Position = GetMemberPosition(memberWithGreatestOddIndex.PositionIndex);
                }
            }
        }

        /* What is layer?
         * ---------------------------------------------------------
         * The layer can be called the distance from the leader. 
         * Here is a notation to make it simple: positionIndex(layer)
         *      example:
         *              Arrowhead-Formation
         *              0   -(0)
         *             1 2  -(1)
         *            3   4 -(2)
         *            
         *            BattleSpread-Formation
         *        3-(2) 1-(1) 0-(0) 2-(1) 4-(2)
         */
        protected int GetMemberLayer(int memberIndex, bool isEven)
        {
            if (isEven)
                return memberIndex / 2;
            else
                return (memberIndex + 1) / 2;
        }
    }
}
