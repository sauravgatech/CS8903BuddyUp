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
    public class CourseMaintenance : ICourse
    {
        private IRepository<Course> _repCourse;
        private IRepository<EntityModel.CourseUser> _repCourseUserRole;
        private IRepository<AspNetUsers> _repUserProfile;
        private IRepository<AspNetRoles> _repRole;
        private IRepository<AspNetUserRoles> _repUserRole;
        private IRepository<EntityModel.Questionnaire> _repQuestionnaire;
        private IRepository<GroupType> _repGroupType;
        
        private IUnitOfWork _uow;
        
        private DomainModelResponse _courseResponse = new DomainModelResponse();

        public CourseMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<Course> repCourse,
                                IRepository<EntityModel.CourseUser> repCourseUserRole,
                                IRepository<AspNetUsers> repUserProfile,
                                IRepository<AspNetRoles> repRole,
                                IRepository<EntityModel.Questionnaire> repQuestionnaire,
                                IRepository<GroupType> repGroupType, 
                                IRepository<AspNetUserRoles> repUserRole)
        {
            _repCourse = repCourse.Use(_uow);
            _repCourseUserRole = repCourseUserRole.Use(_uow);
            _repUserProfile = repUserProfile.Use(_uow);
            _repRole = repRole.Use(_uow);
            _repQuestionnaire = repQuestionnaire.Use(_uow);
            _repGroupType = repGroupType.Use(_uow);
            _repUserRole = repUserRole.Use(_uow);
        }

        public IEnumerable<CourseGetResponse> Get(string courseCode)
        {
            IEnumerable<Course> courses = _repCourse.Get(filter: u => (u.CourseCode == courseCode), includes: "Questionnaire,CourseUserRoles,GroupType");
            List<CourseGetResponse> CourseGetResponses = new List<CourseGetResponse>();
            foreach (Course course in courses)
            {
                CourseGetResponse dr = new CourseGetResponse()
                {
                    CourseCode = course.CourseCode,
                    CourseName = course.CourseName,
                    QuestionnaireCode = course.Questionnaire.QuestionnaireCode,
                    CourseDescription = course.CourseDescription,
                    DesiredSkillSets = course.DesiredSkillSets,
                    GroupSize = course.PrefGroupSize,
                    Roaster = course.Roaster,
                    Term = course.Term,
                    GroupType = course.GroupType.GroupTypeCode,
                    PreferSimiliarSkillSet = course.SimilarSkillSetPreffered,
                    UserList = new List<DomainDto.CourseUser>(),
                    CourseGroups = new List<CourseGroups>()
                };
                foreach(var cur in course.CourseUserRoles)
                {
                    string roleId = _repUserRole.Get(x => x.UserId == cur.UserId).FirstOrDefault().RoleId;
                    DomainDto.CourseUser cu = new DomainDto.CourseUser()
                    {
                        EmailID = cur.AspNetUsers.Email,
                        Name = cur.AspNetUsers.FirstName + " " + cur.AspNetUsers.LastName,
                        RoleCode = _repRole.Get(filter: x => (x.Id == roleId)).FirstOrDefault().Name,
                    };                    
                    if(cur.Group != null)
                    {
                        if (!dr.CourseGroups.Any(x => x.GroupCode == cur.Group.GroupCode))
                        {
                            CourseGroups cg = new CourseGroups()
                            {
                                GroupCode = cur.Group.GroupCode,
                                GroupName = cur.Group.GroupName,
                                Objective = cur.Group.Objective,
                                TimeZone = cur.Group.TimeZone,
                                GroupUsers = new List<DomainDto.CourseUser>() { cu }
                            };
                            dr.CourseGroups.Add(cg);
                        }
                        else
                        {
                            dr.CourseGroups.Where(x => x.GroupCode == cur.Group.GroupCode).First().GroupUsers.Add(cu);
                        }
                    }
                    dr.UserList.Add(cu);
                }
                CourseGetResponses.Add(dr);
            }
            return CourseGetResponses;
        }

        public IEnumerable<CourseGetResponse> Get()
        {
            IEnumerable<Course> courses = _repCourse.Get(includes: "Questionnaire");
            List<CourseGetResponse> CourseGetResponses = new List<CourseGetResponse>();
            foreach (Course course in courses)
            {
                CourseGetResponse dr = new CourseGetResponse()
                {
                    CourseCode = course.CourseCode,
                    CourseName = course.CourseName,
                    QuestionnaireCode = course.Questionnaire != null ? course.Questionnaire.QuestionnaireCode :  null
                };
                CourseGetResponses.Add(dr);
            }
            return CourseGetResponses;
        }

        public UserCourseGetReponse GetCourseForUser(string emailID)
        {
            IEnumerable<Course> courses = _repCourse.Get(includes: "Questionnaire");
            AspNetUsers user = _repUserProfile.Get(x => x.Email == emailID).FirstOrDefault();
            IEnumerable<EntityModel.CourseUser> courseUser = _repCourseUserRole.Get(x => x.UserId == user.Id);
            UserCourseGetReponse ugr = new UserCourseGetReponse();
            if (courseUser == null)
                ugr.RegisteredCourses = null;
            else
            {
                ugr.RegisteredCourses = new List<CourseForUser>();
                foreach(var cu in courseUser)
                {
                    var crs = courses.Where(x => x.CourseId == cu.CourseId).FirstOrDefault();
                    ugr.RegisteredCourses.Add(new CourseForUser() { CourseCode = crs.CourseCode, CourseNane = crs.CourseName, Term = crs.Term });
                }
            }
            var eligible = courses.Where(x => x.Roaster.Contains(user.GTID));
            if (eligible == null)
            {
                ugr.EligibleToRegisterCourses = null;
            }
            else
            {
                ugr.EligibleToRegisterCourses = new List<CourseForUser>();
                foreach (var el in eligible)
                {
                    ugr.EligibleToRegisterCourses.Add(new CourseForUser() { CourseCode = el.CourseCode, CourseNane = el.CourseName, Term = el.Term });
                }
            }
            return ugr;

        }


        public DomainModelResponse Add(CourseAddRequest request)
        {
            GroupType gt = _repGroupType.Get(filter: x => x.GroupTypeCode == request.GroupType).FirstOrDefault();

            Course course = new Course()
            {
                CourseCode = request.CourseCode,
                CourseName = request.CourseName,
                CourseDescription = request.CourseDescription,
                LastChangedTime = DateTime.UtcNow,
                DesiredSkillSets = request.DesiredSkillSets,
                GroupType = gt,
                Roaster = null,
                Term = request.Term,
                PrefGroupTypeId = gt.GroupTypeId,
                PrefGroupSize = request.GroupSize,
                SimilarSkillSetPreffered = request.PreferSimiliarSkillSet
            };

            _repCourse.Add(course);
            _uow.Commit();
            course = _repCourse.Get(filter: f => f.CourseCode == request.CourseCode).FirstOrDefault();

            if (request.userList != null)
            {
                List<string> roleCodes = request.userList.Select(x => x.roleCode).ToList();
                List<string> emailIds = request.userList.Select(x => x.emailId).ToList();

                List<AspNetRoles> roles = _repRole.Get(filter: f => roleCodes.Contains(f.Name)).ToList();
                List<AspNetUsers> userProfiles = _repUserProfile.Get(filter: f => emailIds.Contains(f.Email)).ToList();

                foreach(var user in request.userList)
                {
                    EntityModel.CourseUser cur = new EntityModel.CourseUser()
                    {
                        CourseId = course.CourseId,
                        Course = course,
                        UserId = userProfiles.Where(x => x.Email == user.emailId).FirstOrDefault().Id,
                        AspNetUsers = userProfiles.Where(x => x.Email == user.emailId).FirstOrDefault(),
                        LastChangedTime = DateTime.UtcNow
                    };
                    _repCourseUserRole.Add(cur);
                }
                _uow.Commit();
            }
            _courseResponse.addResponse("Add", MessageCodes.InfoCreatedSuccessfully, "Course : " + request.CourseCode);
            return _courseResponse;
        }

        public DomainModelResponse Update(CourseUpdateRequest request)
        {
            Course course = _repCourse.Get(filter: f => f.CourseCode == request.CourseCode).FirstOrDefault();
            bool updateCourse = false;
            if(request.CourseName != null) //Course name update
            {
                course.CourseName = request.CourseName;
                updateCourse = true;
            }
            if (request.CourseDescription != null) 
            {
                course.CourseDescription = request.CourseDescription;
                updateCourse = true;
            }

            if(request.QuestionnaireCode != null)
            {
                EntityModel.Questionnaire questionnaire = _repQuestionnaire.Get(filter: f => f.QuestionnaireCode == request.QuestionnaireCode).FirstOrDefault();
                course.QuestionnaireId = questionnaire.QuestionnaireId;
                course.Questionnaire = questionnaire;
                updateCourse = true;
            }

            if(!string.IsNullOrWhiteSpace(request.Roaster))
            {
                course.Roaster = request.Roaster;
                updateCourse = true;
            }

            if(updateCourse)
                _repCourse.Update(course);

            if(request.CourseNewUsers != null)
            {
                List<string> roleCodes = request.CourseNewUsers.Select(x => x.roleCode).ToList();
                List<string> emailIds = request.CourseNewUsers.Select(x => x.emailId).ToList();

                List<AspNetUsers> userProfiles = _repUserProfile.Get(filter: f => emailIds.Contains(f.Email)).ToList();

                foreach (var user in request.CourseNewUsers)
                {
                    EntityModel.CourseUser cur = new EntityModel.CourseUser()
                    {
                        CourseId = course.CourseId,
                        Course = course,
                        UserId = userProfiles.Where(x => x.Email == user.emailId).FirstOrDefault().Id,
                        AspNetUsers = userProfiles.Where(x => x.Email == user.emailId).FirstOrDefault(),
                        LastChangedTime = DateTime.UtcNow
                    };
                    _repCourseUserRole.Add(cur);
                }
            }

            if (request.CourseDeleteUsers != null)
            {
                List<string> roleCodes = request.CourseDeleteUsers.Select(x => x.roleCode).ToList();
                List<string> emailIds = request.CourseDeleteUsers.Select(x => x.emailId).ToList();

                List<AspNetUsers> userProfiles = _repUserProfile.Get(filter: f => emailIds.Contains(f.Email)).ToList();

                foreach (var user in request.CourseDeleteUsers)
                {
                    string UserId = userProfiles.Where(x => x.Email == user.emailId).FirstOrDefault().Id;

                    EntityModel.CourseUser cur = _repCourseUserRole.Get(filter: f => f.CourseId == course.CourseId && f.UserId == UserId).FirstOrDefault();
                    _repCourseUserRole.Delete(cur);
                }
            }

            _uow.Commit();
            _courseResponse.addResponse("Update", MessageCodes.InfoSavedSuccessfully, "Course : " + request.CourseCode);
            return _courseResponse;
        }

        public DomainModelResponse Delete(string courseCode)
        {
            return new DomainModelResponse();
        }
    }
}
