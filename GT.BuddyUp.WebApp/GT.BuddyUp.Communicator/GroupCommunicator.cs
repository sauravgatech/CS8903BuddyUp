using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.DomainModel;
using GT.BuddyUp.Platform.Repository;
using GT.BuddyUp.Platform.Common;

namespace GT.BuddyUp.Communicator
{
    public class GroupCommunicator
    {
        IGroup group;

        public GroupCommunicator(IGroup grp)
        {
            group = grp;
        }

        public GroupSummaryForUser GetGroupSummary(string userEmail, string courseCode)
        {
            try
            {
                return group.GetGroupSummary(userEmail, courseCode);
            }
            catch
            {
                return null;
            }
        }

        public GroupGetResponse GetGroup(string groupCode)
        {
            try
            {
                return group.Get(groupCode).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public bool AddGroup(GroupAddRequest request)
        {
            try
            {
                group.Add(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
