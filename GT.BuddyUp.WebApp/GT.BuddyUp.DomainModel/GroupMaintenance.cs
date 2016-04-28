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
    public class GroupMaintenance : IGroup
    {
        private IRepository<Group> _repGroup;
        private IRepository<GroupType> _repGroupType;
        private IRepository<Course> _repCourse;
        private IRepository<AspNetUsers> _repUserProfile;
        private IRepository<EntityModel.CourseUser> _repCourseUserRole;
        
        private IUnitOfWork _uow;
        
        private DomainModelResponse _groupResponse = new DomainModelResponse();

        public GroupMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<Group> repGroup,
                                IRepository<GroupType> repGroupType,
                                IRepository<Course> repCourse,
                                IRepository<AspNetUsers> repUserProfile,
                                IRepository<EntityModel.CourseUser> repCourseUserRole)
        {
            _repCourse = repCourse.Use(_uow);
            _repGroup = repGroup.Use(_uow);
            _repGroupType = repGroupType.Use(_uow);
            _repUserProfile = repUserProfile.Use(_uow);
            _repCourseUserRole = repCourseUserRole.Use(_uow);
        }

        public IEnumerable<GroupGetResponse> Get(string groupCode = "")
        {
            List<Group> groups = _repGroup.Get(filter: f => (f.GroupCode == groupCode || groupCode == ""), includes: "GroupType,CourseUserRoles,Course,Posts").ToList();
            List<GroupGetResponse> groupGetResponses = new List<GroupGetResponse>();
            foreach(Group grp in groups)
            {
                GroupGetResponse ggr = new GroupGetResponse()
                {
                    GroupCode = grp.GroupCode,
                    GroupName = grp.GroupName,
                    Objective = grp.Objective,
                    TimeZone = grp.TimeZone,
                    UserList = new List<GroupUser>(),
                    GroupPosts = new List<DomainDto.Post>(),
                    GroupTypeCode = grp.GroupType.GroupTypeCode,
                    CourseCode = grp.Course.CourseCode
                };
                foreach(var cur in grp.CourseUserRoles)
                {
                    ggr.UserList.Add(new GroupUser()
                    {
                       emailId = cur.AspNetUsers.Email,
                       name = cur.AspNetUsers.FirstName + " " + cur.AspNetUsers.LastName
                    });
                }
                Dictionary<int, DomainDto.Post> postBuilder = new Dictionary<int, DomainDto.Post>();
                List<EntityModel.Post> posts = grp.Posts.OrderBy(x => x.TimePosted).ToList();
                foreach(EntityModel.Post post in posts)
                {
                    if(post.ParentId.HasValue)
                    {
                        postBuilder[post.PostId].ChildPosts.Add(new DomainDto.Post()
                            {
                                PostText = post.PostText,
                                TimePosted = post.TimePosted,
                                UserName = post.UserName
                            });
                    }
                    else
                    {
                        postBuilder.Add(post.PostId, new DomainDto.Post()
                            {
                                PostText = post.PostText,
                                TimePosted = post.TimePosted,
                                 UserName = post.UserName,
                                 ChildPosts = new List<DomainDto.Post>()
                            });
                    }
                }
                ggr.GroupPosts = postBuilder.Values.ToList();
                groupGetResponses.Add(ggr);
            }
            return groupGetResponses;
        }

        public DomainModelResponse Add(GroupAddRequest request)
        {
            Group grp = new Group()
            {
                GroupCode = request.GroupCode,
                GroupName = request.GroupName,
                Objective = request.Objective,
                TimeZone = request.TimeZone
            };

            GroupType gt = _repGroupType.Get(filter: f => f.GroupTypeCode == request.GroupTypeCode).FirstOrDefault();
            Course crs = _repCourse.Get(filter: f => f.CourseCode == request.CourseCode).FirstOrDefault();
            grp.GroupTypeId = gt.GroupTypeId;
            grp.GroupType = gt;
            grp.CourseId = crs.CourseId;
            grp.Course = crs;
            _repGroup.Add(grp);
            _uow.Commit();
            grp = _repGroup.Get(filter: f => f.GroupCode == request.GroupCode).FirstOrDefault();

            
            
            if (request.userList != null)
            {
                List<string> users = _repUserProfile.Get(filter: f => request.userList.Contains(f.Email)).Select(x => x.Id).ToList();
                List<EntityModel.CourseUser> Curs = _repCourseUserRole.Get(filter: f => f.CourseId == crs.CourseId && users.Contains(f.UserId)).ToList();
                foreach (EntityModel.CourseUser cur in Curs)
                {
                    cur.GroupId = grp.GroupId;
                    cur.Group = grp;
                    _repCourseUserRole.Update(cur);
                }
            }
            _uow.Commit();
            _groupResponse.addResponse("Add", MessageCodes.InfoCreatedSuccessfully, "Group : " + request.GroupCode);
            return _groupResponse;
        }

        public DomainModelResponse Update(GroupUpdateRequest request)
        {
            Group grp = _repGroup.Get(filter: f => f.GroupCode == request.GroupCode).FirstOrDefault();

            if(string.IsNullOrWhiteSpace(request.GroupName))
            {
                grp.GroupName = request.GroupName;
            }

            if (string.IsNullOrWhiteSpace(request.Objective))
            {
                grp.Objective = request.Objective;
            }

            if (string.IsNullOrWhiteSpace(request.TimeZone))
            {
                grp.TimeZone = request.TimeZone;
            }

            if (string.IsNullOrWhiteSpace(request.GroupTypeCode))
            {
                GroupType gt = _repGroupType.Get(filter: f => f.GroupTypeCode == request.GroupTypeCode).FirstOrDefault();
                grp.GroupTypeId = gt.GroupTypeId;
                grp.GroupType = gt;
            }

            Course crs = grp.CourseUserRoles.FirstOrDefault().Course;

            if (request.newUserList != null)
            {
                List<string> users = _repUserProfile.Get(filter: f => request.newUserList.Contains(f.Email)).Select(x => x.Id).ToList();
                List<EntityModel.CourseUser> Curs = _repCourseUserRole.Get(filter: f => f.CourseId == crs.CourseId && users.Contains(f.UserId)).ToList();
                foreach (EntityModel.CourseUser cur in Curs)
                {
                    cur.GroupId = grp.GroupId;
                    cur.Group = grp;
                    _repCourseUserRole.Update(cur);
                }
            }

            if (request.removeUserList != null)
            {
                List<string> users = _repUserProfile.Get(filter: f => request.removeUserList.Contains(f.Email)).Select(x => x.Id).ToList();
                List<EntityModel.CourseUser> Curs = _repCourseUserRole.Get(filter: f => f.CourseId == crs.CourseId && users.Contains(f.UserId)).ToList();
                foreach (EntityModel.CourseUser cur in Curs)
                {
                    cur.GroupId = null;
                    cur.Group = null;
                    _repCourseUserRole.Update(cur);
                }
            }
            _uow.Commit();
            _groupResponse.addResponse("Update", MessageCodes.InfoSavedSuccessfully, "Group : " + request.GroupCode);
            return _groupResponse;
        }

        public DomainModelResponse Delete(string groupCode)
        {
            return new DomainModelResponse();
        }

        public GroupSummaryForUser GetGroupSummary(string userEmail, string courseCode)
        {
            GroupSummaryForUser gsfu = new GroupSummaryForUser();
            AspNetUsers up = _repUserProfile.Get(filter: f => f.Email == userEmail).FirstOrDefault();
            Course course = _repCourse.Get(filter: f => f.CourseCode == courseCode, includes: "Groups,CourseUserRoles").FirstOrDefault();

            EntityModel.CourseUser cur = _repCourseUserRole.Get(filter: f => f.CourseId == course.CourseId && f.UserId == up.Id, includes:"Group").FirstOrDefault();
            if(cur.GroupId.HasValue)
            {
                gsfu.registeredGroup = new GroupSummary()
                    {
                        GroupCode = cur.Group.GroupCode,
                        GroupName = cur.Group.GroupName,
                        activeTimeZone = cur.Group.TimeZone,
                        Objective = cur.Group.Objective
                    };
            }
            else
            {
                gsfu.registeredGroup = null;
            }
            List<string> answers = cur.AnswerSet.Split(',').ToList();
            gsfu.suggestedGroups = new List<GroupSummary>();
            foreach (EntityModel.CourseUser cu in course.CourseUserRoles)
            {
                if (cu.UserId != up.Id && !string.IsNullOrWhiteSpace(cu.AnswerSet))
                {
                    int matchPercentage = getMatchPercentage(answers, cu.AnswerSet.Split(',').ToList());
                    if (matchPercentage >= 50)
                    {
                        if (cu.GroupId.HasValue)
                        {
                            if (!cur.GroupId.HasValue || cur.GroupId != cu.GroupId)
                            {
                                Group grp = _repGroup.Get(filter: f => f.GroupId == cu.GroupId).FirstOrDefault();
                                if (!gsfu.suggestedGroups.Any(x => x.GroupCode == grp.GroupCode))
                                {
                                    gsfu.suggestedGroups.Add(new GroupSummary()
                                        {
                                            GroupName = grp.GroupName,
                                            GroupCode = grp.GroupCode,
                                            Objective = grp.Objective,
                                            activeTimeZone = grp.TimeZone
                                        });
                                }
                            }
                        }
                    }
                }
            }
            gsfu.AllGroups = new List<GroupSummary>();
            foreach(var grp in course.Groups)
            {
                if (!cur.GroupId.HasValue || cur.GroupId != grp.GroupId)
                {
                    gsfu.AllGroups.Add(new GroupSummary()
                    {
                        GroupCode = grp.GroupCode,
                        GroupName = grp.GroupName,
                        Objective = grp.Objective,
                        activeTimeZone = grp.TimeZone
                    });
                }
            }
            return gsfu;
        }

        private int getMatchPercentage(List<string> currentAnswers, List<string> givenAnswers)
        {
            if(currentAnswers.Count == givenAnswers.Count)
            {
                int total = currentAnswers.Count;
                int match = 0;
                for(int i = 0; i< currentAnswers.Count; i++)
                {
                    if(currentAnswers[i].Equals(givenAnswers[i], StringComparison.OrdinalIgnoreCase))
                        match++;
                }
                return (match * 100) / total;
            }
            return 0;
        }
    }
}
