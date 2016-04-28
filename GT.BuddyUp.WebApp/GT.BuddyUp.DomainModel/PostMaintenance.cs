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
    public class PostMaintenance : IPost
    {
        private IRepository<Group> _repGroup;
        private IRepository<GroupType> _repGroupType;
        private IRepository<Course> _repCourse;
        private IRepository<AspNetUsers> _repUserProfile;
        private IRepository<EntityModel.CourseUser> _repCourseUserRole;
        private IRepository<EntityModel.Post> _repPost;
        
        private IUnitOfWork _uow;
        
        private DomainModelResponse _postResponse = new DomainModelResponse();

        public PostMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<Group> repGroup,
                                IRepository<GroupType> repGroupType,
                                IRepository<Course> repCourse,
                                IRepository<AspNetUsers> repUserProfile,
                                IRepository<EntityModel.CourseUser> repCourseUserRole,
                                IRepository<EntityModel.Post> repPost)
        {
            _repCourse = repCourse.Use(_uow);
            _repGroup = repGroup.Use(_uow);
            _repGroupType = repGroupType.Use(_uow);
            _repUserProfile = repUserProfile.Use(_uow);
            _repCourseUserRole = repCourseUserRole.Use(_uow);
            _repPost = repPost.Use(_uow);
        }

        public DomainModelResponse AddPost(PostAddRequest request)
        {
            EntityModel.Post parent = null;
            if(request.ParentPostTime != null && !string.IsNullOrWhiteSpace(request.ParentPostUserName))
            {
                parent = _repPost.Get(filter: f => f.TimePosted == request.ParentPostTime && f.UserName == request.ParentPostUserName).FirstOrDefault();
            }

            Group grp = _repGroup.Get(filter: f => f.GroupCode == request.GroupCode).FirstOrDefault();

            EntityModel.Post post = new EntityModel.Post()
            {
                UserName = request.UserName,
                TimePosted = request.TimePosted,
                PostText = request.PostText,
                Group = grp,
                GroupId = grp.GroupId,
                LastChangedTime = DateTime.UtcNow
            };
            if(parent!= null)
            {
                post.ParentId = parent.PostId;
            }
            else
            {
                post.ParentId = null;
            }

            _repPost.Add(post);
            _uow.Commit();
            _postResponse.addResponse("Add", MessageCodes.InfoCreatedSuccessfully, "Post");
            return _postResponse;
        }
    }
}
