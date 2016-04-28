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
    public class CourseUserCommunicator
    {
        ICourseUser CourseUser;

        public CourseUserCommunicator(ICourseUser CourseUser)
        {
            this.CourseUser = CourseUser;
        }

        public bool UpdateCourseUser(CourseUserUpdateRequest request)
        {
            try
            {
                CourseUser.UpdateCourseUserAnswer(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
