using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.Platform.Common;
using GT.BuddyUp.Platform.Repository;
using GT.BuddyUp.EntityModel;
using Microsoft.Practices.Unity;

namespace GT.BuddyUp.DomainModel
{
    public class CourseUserMaintenance : ICourseUser
    {
        private IRepository<Course> _repCourse;
        private IRepository<EntityModel.CourseUser> _repCourseUserRole;
        private IRepository<AspNetUsers> _repUserProfile;
        private IRepository<AspNetRoles> _repRole;
        private IRepository<Group> _repGroup;
        
        private IUnitOfWork _uow;
        
        private DomainModelResponse _courseUserResponse = new DomainModelResponse();

        public CourseUserMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<Course> repCourse,
                                IRepository<EntityModel.CourseUser> repCourseUserRole,
                                IRepository<AspNetUsers> repUserProfile,
                                IRepository<AspNetRoles> repRole,
                                IRepository<Group> repGroup)
        {
            _repCourse = repCourse.Use(_uow);
            _repCourseUserRole = repCourseUserRole.Use(_uow);
            _repUserProfile = repUserProfile.Use(_uow);
            _repRole = repRole.Use(_uow);
            _repGroup = repGroup.Use(_uow);
        }

        public DomainModelResponse UpdateCourseUserAnswer(CourseUserUpdateRequest request)
        {
            AspNetUsers up = _repUserProfile.Get(filter: f => f.Email == request.email).FirstOrDefault();
            Course course = _repCourse.Get(filter: f => f.CourseCode == request.courseCode).FirstOrDefault();

            EntityModel.CourseUser cur = _repCourseUserRole.Get(filter: f => f.UserId == up.Id && f.CourseId == course.CourseId).FirstOrDefault();
            bool isAdd = false;
            if(cur == null) //User is nto added
            {
                isAdd = true;
                cur = new EntityModel.CourseUser()
                {
                    CourseId = course.CourseId,
                    Course = course,
                    UserId = up.Id,
                    AspNetUsers = up                    
                };
            }
            if(!string.IsNullOrWhiteSpace(request.answerSet))
            {
                cur.AnswerSet = request.answerSet;
            }
            if(!string.IsNullOrWhiteSpace(request.GroupCode))
            {
                Group grp = _repGroup.Get(filter: f => f.GroupCode == request.GroupCode).FirstOrDefault();
                cur.GroupId = grp.GroupId;
                cur.Group = grp;
            }
            if (isAdd)
            {
                _repCourseUserRole.Add(cur);
                _courseUserResponse.addResponse("Add", MessageCodes.InfoCreatedSuccessfully, "Course User Role for User : " + request.email);
            }
            else
            {
                _repCourseUserRole.Update(cur);
                _courseUserResponse.addResponse("Update", MessageCodes.InfoSavedSuccessfully, "Course User Role for User : " + request.email);
            }
            _uow.Commit();
            return _courseUserResponse;
        }
    }
}
