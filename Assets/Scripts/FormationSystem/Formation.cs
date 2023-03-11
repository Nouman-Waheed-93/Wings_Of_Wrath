using UnityEngine;
using System.Collections.Generic;
using System;

namespace FormationSystem
{
    public abstract class Formation
    {
        public int MemberCount { get { return members.Count; } }

        protected HashSet<IFormationMember> members = new HashSet<IFormationMember>();

        public virtual void AddMember(IFormationMember member)
        {
            //Add the member's position index before adding the member in the list. 
            //Because, this member's positionIndex is equal to Count before adding the new member
            members.Add(member);
            member.PositionIndex = members.Count-1;
            member.Position = GetMemberPosition(member.PositionIndex);
        }

        public abstract Vector3 GetMemberPosition(int memberIndex);

        public virtual void RemoveMember(IFormationMember memberToRemove)
        {
            int removedMemberIndex = memberToRemove.PositionIndex;
            members.Remove(memberToRemove);
            foreach (IFormationMember member in members)
            {
                if (member.PositionIndex > removedMemberIndex)
                {
                    member.PositionIndex--;
                    member.Position = GetMemberPosition(member.PositionIndex);
                }
            }
        }

        protected void CheckIndexValidity(int memberIndex)
        {
            if (memberIndex < 0)
                throw new ArgumentException("Index cannot be a negative number");
        }
    }
}