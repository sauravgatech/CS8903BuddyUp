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
    public class CourseCommunicator
    {
        private ICourse Course;
        public CourseCommunicator(ICourse course)
        {
            Course = course;
        }
        public IEnumerable<CourseGetResponse> GetAllCourses()
        {
            try
            {
                return Course.Get();
            }
            catch
            {
                return null;
            }
        }

        public bool AddCourse(CourseAddRequest request)
        {
            try
            {
                Course.Add(request);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool UpdateCourse(CourseUpdateRequest request)
        {
            try
            {
                Course.Update(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CourseGetResponse GetCourse(string courseCode)
        {
            try
            {
                return Course.Get(courseCode).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public UserCourseGetReponse GetCourseForUser(string emailId)
        {
            try
            {
                return Course.GetCourseForUser(emailId);
            }
            catch
            {
                return null;
            }
        }
    }
}
