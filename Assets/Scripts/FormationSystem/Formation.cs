using UnityEngine;
using System.Collections.Generic;

namespace FormationSystem
{
    public abstract class Formation
    {
        protected HashSet<FormationMember> members = new HashSet<FormationMember>();
         
        public virtual void AddMember(FormationMember member)
        {
            //Add the member's position index before adding the member in the list. 
            //Because, this member's positionIndex is equal to Count before adding the new member
            member.PositionIndex = members.Count;
            member.Position = GetMemberPosition(member.PositionIndex);
            members.Add(member);
        }

        public abstract Vector3 GetMemberPosition(int memberIndex);

        public virtual void RemoveMember(FormationMember memberToRemove)
        {
            int removedMemberIndex = memberToRemove.PositionIndex;
            members.Remove(memberToRemove);
            foreach(FormationMember member in members)
            {
                if(member.PositionIndex > removedMemberIndex)
                {
                    member.PositionIndex--;
                    member.Position = GetMemberPosition(member.PositionIndex);
                }
            }
        }
    }
}