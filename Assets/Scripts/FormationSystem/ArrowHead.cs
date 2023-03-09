using UnityEngine;
using System;

namespace FormationSystem
{
    /* Formation Design
     *      0   -layer 0
     *     1 2  -layer 1
     *    3   4 -layer 2
     */
    public class ArrowHead : Formation
    {
        public override Vector3 GetMemberPosition(int memberIndex)
        {
            if (memberIndex < 0)
                throw new ArgumentException("Index cannot be a negative number");

            if (memberIndex == 0)
                return Vector3.zero;

            if (memberIndex % 2 == 0)
                return new Vector3(-0.707f, 0, -0.707f) * GetMemberLayer(memberIndex, true);
            else
                return new Vector3(0.707f, 0, -0.707f) * GetMemberLayer(memberIndex, false);
        }

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
                if (member.PositionIndex > removedMemberIndex)
                {
                    bool isEven = member.PositionIndex % 2 == 0;
                    
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

                    //if the removedmemberIndex and the current Index are both even or are both odd
                    if (isRemovedMemberIndexEven == isEven) 
                    {
                        member.PositionIndex -= 2;
                        member.Position = GetMemberPosition(member.PositionIndex);
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

        private int GetMemberLayer(int memberIndex, bool isEven)
        {
            if (isEven)
                return memberIndex / 2;
            else
                return (memberIndex + 1)/ 2;
        }
    }
}
