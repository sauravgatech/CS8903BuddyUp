using Microsoft.Practices.Unity;

namespace GT.BuddyUp.WebApp
{
    public static class UnityConfig
    {
        private static IUnityContainer _container;

        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        public static void RegisterComponents()
        {
            _container = new UnityContainer();

            _container.RegisterType(typeof(GT.BuddyUp.Platform.Repository.IUnitOfWork), typeof(GT.BuddyUp.Platform.Repository.BuddyUpUnitOfWork), "buddyup");
            _container.RegisterType(typeof(GT.BuddyUp.Platform.Repository.IRepository<>), typeof(GT.BuddyUp.Platform.Repository.Repository<>));
            _container.RegisterType(typeof(GT.BuddyUp.DomainModel.ICourse), typeof(GT.BuddyUp.DomainModel.CourseMaintenance), "Course");
            _container.RegisterType(typeof(GT.BuddyUp.DomainModel.IGroup), typeof(GT.BuddyUp.DomainModel.GroupMaintenance), "Group");
            _container.RegisterType(typeof(GT.BuddyUp.DomainModel.IQuestionnaire), typeof(GT.BuddyUp.DomainModel.QuestionnaireMaintenance), "Questionnaire");
            _container.RegisterType(typeof(GT.BuddyUp.DomainModel.ICourseUser), typeof(GT.BuddyUp.DomainModel.CourseUserMaintenance), "CourseUser");
            _container.RegisterType(typeof(GT.BuddyUp.DomainModel.IPost), typeof(GT.BuddyUp.DomainModel.PostMaintenance), "Post");
            _container.RegisterType(typeof(GT.BuddyUp.DomainModel.IUser), typeof(GT.BuddyUp.DomainModel.UserMaintenance), "User");
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}