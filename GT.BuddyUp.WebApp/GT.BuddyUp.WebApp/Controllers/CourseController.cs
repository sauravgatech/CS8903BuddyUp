using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GT.BuddyUp.WebApp.Models;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.Communicator;
using GT.BuddyUp.WebApp;

namespace GT.BuddyUp.WebApp.Controllers
{
    public class CourseController : Controller
    {
        private CourseCommunicator _courseCom = new CourseCommunicator((GT.BuddyUp.DomainModel.ICourse)UnityConfig.Container.Resolve(typeof(GT.BuddyUp.DomainModel.ICourse), "Course"));
        private QuestionnaireCommunicator _questionnaireCom = new QuestionnaireCommunicator((GT.BuddyUp.DomainModel.IQuestionnaire)UnityConfig.Container.Resolve(typeof(GT.BuddyUp.DomainModel.IQuestionnaire), "Questionnaire"));
        private CourseUserCommunicator _courseUserCom = new CourseUserCommunicator((GT.BuddyUp.DomainModel.ICourseUser)UnityConfig.Container.Resolve(typeof(GT.BuddyUp.DomainModel.ICourseUser), "CourseUser"));
        private UserCommunicator _userCom = new UserCommunicator((GT.BuddyUp.DomainModel.IUser)UnityConfig.Container.Resolve(typeof(GT.BuddyUp.DomainModel.IUser), "User"));
        // GET: Course
        public ActionResult JoinACourse(string courseCode)
        {
            CourseGetResponse cgr = _courseCom.GetCourse(courseCode);
            DisplayCourseModel dcm = new DisplayCourseModel();
            if (cgr != null)
            {
                dcm.CourseCode = cgr.CourseCode;
                dcm.CourseDescription = cgr.CourseDescription;
                dcm.CourseName = cgr.CourseName;
                dcm.DesiredSkillSets = cgr.DesiredSkillSets;
                dcm.GroupSize = (int)cgr.GroupSize;
                dcm.GroupType = cgr.GroupType;
                dcm.PreferSimiliarSkillSet = (bool)cgr.PreferSimiliarSkillSet;
                dcm.Users = new List<GT.BuddyUp.WebApp.Models.DisplayCourseUser>();
                dcm.Questions = new List<GT.BuddyUp.WebApp.Models.DsiplayQuestion>();
            }
            foreach (var user in cgr.UserList)
            {
                dcm.Users.Add(new DisplayCourseUser()
                {
                    emailId = user.EmailID,
                    name = user.Name,
                    role = user.RoleCode
                });
            }

            QuestionnaireGetResponse qgr = _questionnaireCom.GetQuestionnaire(cgr.QuestionnaireCode);

            if (qgr != null)
            {
                foreach (var q in qgr.Questions)
                {
                    dcm.Questions.Add(new GT.BuddyUp.WebApp.Models.DsiplayQuestion()
                    {
                        QuestionText = q.questionText,
                        QuestionType = q.questionType,
                        AnswerChoices = q.answerChoices
                    });
                }
            }
            return View(dcm);
        }

        public ActionResult UnregisteredTeacher()
        {
            UserGetResponse ugr = _userCom.GetUser(User.Identity.Name);
            ViewBag.User = ugr.firstName + " " + ugr.lastName;
            return View();
        }

        public ActionResult Student()
        {
            UserCourseGetReponse ucgr = _courseCom.GetCourseForUser(User.Identity.Name);
            StudentDashboardModel sdm = new StudentDashboardModel();
            if (ucgr != null)
            {
                sdm.AllCourseDropDown = new List<string>();
                sdm.selectedCourse = null;
                sdm.selectedRegisteredCourse = null;
                sdm.RegisteredCourseDropDown = new List<string>();

                if (ucgr.RegisteredCourses != null)
                {
                    foreach (var crs in ucgr.RegisteredCourses)
                    {
                        sdm.RegisteredCourseDropDown.Add(crs.CourseCode + " :: " + crs.CourseNane + " (" + crs.Term + ")");
                    }
                }
                if(ucgr.EligibleToRegisterCourses != null)
                {
                    foreach(var crs in ucgr.EligibleToRegisterCourses)
                    {
                        if(ucgr.RegisteredCourses != null && !ucgr.RegisteredCourses.Any(x => x.CourseCode == crs.CourseCode && x.Term == crs.Term))
                            sdm.AllCourseDropDown.Add(crs.CourseCode + " :: " + crs.CourseNane + " (" + crs.Term + ")");
                    }
                }
            }
            return View(sdm);
        }


        [HttpPost]
        public async Task<ActionResult> Student(StudentDashboardModel model)
        {
            if(ModelState.IsValid)
            {
                string[] stringSeparators = new string[] {" :: "};
                if(!string.IsNullOrWhiteSpace(model.selectedRegisteredCourse))
                {
                    string courseCode = model.selectedRegisteredCourse.Split(stringSeparators, StringSplitOptions.None).FirstOrDefault();
                    return RedirectToAction("GroupSummary", "Group", routeValues: new { courseCode = courseCode });
                }
            }
            ModelState.AddModelError("", "Oops! Something wrong happened! Please try again.");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> JoinACourse(DisplayCourseModel Model)
        {
            if(ModelState.IsValid)
            {
                StringBuilder answerSet = new StringBuilder();
                foreach(var question in Model.Questions)
                {
                    if(!string.IsNullOrWhiteSpace(question.selectedAnswer))
                    {
                        answerSet.Append(question.selectedAnswer + ",");
                    } 
                }
                answerSet.Remove(answerSet.Length - 1, 1);
                CourseUserUpdateRequest cuur = new CourseUserUpdateRequest()
                {
                    email = User.Identity.Name,
                    courseCode = Model.CourseCode,
                    RoleCode = "Student",
                    answerSet = answerSet.ToString()
                };
                bool result = _courseUserCom.UpdateCourseUser(cuur);
                if(result)
                {
                    //MvcApplication.courses.Add(Model.CourseCode, Model.CourseName);
                    //MvcApplication.courseDescription.Add(Model.CourseCode, Model.CourseDescription);
                    return RedirectToAction("Student", "Course");
                }
                else
                {
                    ModelState.AddModelError("", "Oops! Something wrong happened! Please try again.");
                    return View(Model);
                }
            }
            ModelState.AddModelError("", "Oops! Something wrong happened! Please try again.");
            return View(Model);
        }

        public ActionResult Register(StudentDashboardModel model)
        {
            if(ModelState.IsValid)
            {
                string[] stringSeparators = new string[] {" :: "};
                if(!string.IsNullOrWhiteSpace(model.selectedCourse))
                {
                    string courseCode = model.selectedCourse.Split(stringSeparators, StringSplitOptions.None).FirstOrDefault();
                    return RedirectToAction("JoinACourse", "Course", routeValues: new { courseCode = courseCode });
                }
            }
            ModelState.AddModelError("", "Oops! Something wrong happened! Please try again.");
            return View(model);
        }

        public ActionResult Create()
        {
            UserGetResponse ugr = _userCom.GetUser(User.Identity.Name);
            ViewBag.User = ugr.firstName + " " + ugr.lastName;
            ///ViewBag.User = MvcApplication.userName;
            CreateCourseModel ccm = new CreateCourseModel();
            ccm.Users = new List<GT.BuddyUp.WebApp.Models.CourseUser>() { new GT.BuddyUp.WebApp.Models.CourseUser() { emailId = User.Identity.Name, role = GT.BuddyUp.WebApp.Models.Role.Teacher } };
            //ccm.DesiredSkillSets = new List<Skill>() {new Skill() { skill = "Java", requiredSkillLevel = SkillLevel.Beginner} };
            ccm.AvailableTerms = getAvailableTerms();
            ccm.Questions = new List<GT.BuddyUp.WebApp.Models.Question>();
            return View(ccm);
        }

        private List<string> getAvailableTerms()
        {
            List<string> availableTerms = new List<string>();
            int curMonth = DateTime.Now.Month;
            int curYear = DateTime.Now.Year;
            if(curMonth <= 4)
            {
                availableTerms.Add("Spring " + curYear);
            }
            if(curMonth <= 8)
            {
                availableTerms.Add("Summer " + curYear);
            }
            if (curMonth <= 12)
            {
                availableTerms.Add("Fall " + curYear);
            }
            availableTerms.Add("Spring " + (curYear + 1).ToString());
            availableTerms.Add("Summer " + (curYear + 1).ToString());
            availableTerms.Add("Fall " + (curYear + 1).ToString());
            if (curMonth <= 4)
            {
                availableTerms.Add("Spring " + (curYear + 2).ToString());
            }
            if (curMonth <= 8)
            {
                availableTerms.Add("Summer " + (curYear + 2).ToString());
            }
            if (curMonth <= 12)
            {
                availableTerms.Add("Fall " + (curYear + 2).ToString());
            }
            return availableTerms;
        }

        public PartialViewResult BlankEditorRow()
        {
            return PartialView("_CourseUsers", new GT.BuddyUp.WebApp.Models.CourseUser());
        }

        public PartialViewResult BlankSkillRow()
        {
            return PartialView("_CourseSkill", new Skill());
        }

        public PartialViewResult BlankQuestionRow()
        {
            return PartialView("_CourseQuestion", new GT.BuddyUp.WebApp.Models.Question());
        }

        public ActionResult AddSkillSet()
        {
            return PartialView("_CourseSkill", new Skill());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCourseModel model)
        {
            if (ModelState.IsValid)
            {
                CourseAddRequest car = new CourseAddRequest()
                {
                    CourseCode = model.CourseCode,
                    CourseName = model.CourseName,
                    //DesiredSkillSets = model.DesiredSkillSets,
                    GroupSize = model.GroupSize,
                    CourseDescription = model.CourseDescription,
                    PreferSimiliarSkillSet = model.PreferSimiliarSkillSet,
                    Term = model.Term,
                    userList = new List<CourseNewUser>()
                };
                switch(model.GroupType)
                {
                    case "Study Group":
                        car.GroupType = "Study";
                        break;
                    case "Project Group (Open Projects)":
                        car.GroupType = "OpenProject";
                        break;
                    case "Project Group (Closed Projects)":
                        car.GroupType = "ClosedProject";
                        break;
                }

                if(model.Users != null && model.Users.Count > 0)
                {
                    foreach(var user in model.Users)
                    {
                        car.userList.Add(new CourseNewUser()
                        {
                            emailId = user.emailId,
                            roleCode = user.role.ToString()
                        });
                    }
                }
                
                bool result = _courseCom.AddCourse(car);
                if (result) //Course is added, now generate intelligent question set and add questionnaire
                {
                    QuestionnaireAddRequest qar = new QuestionnaireAddRequest()
                    {
                         IsATemplate = false,
                         QuestionnaireCode = DateTime.UtcNow.ToString("MMddyyHmmss"),
                         Questions = new List<DomainDto.Question>()
                    };
                    List<string> timeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => x.DisplayName).ToList();
                    qar.Questions.Add(new DomainDto.Question()
                    {
                         questionText = "In what timezone are you mostly available?",
                         questionType = "MultipleChoice",
                         answerChoices = timeZones
                    });
                    List<string> timeSlots = new List<string>()
                    {
                        "6:00 AM - 9:00 AM",
                        "9:00 AM - 12:00 PM",
                        "12:00 PM - 3:00 PM",
                        "3:00 PM - 6:00 PM",
                        "6:00 PM - 9:00 PM",
                        "9:00 PM - 12:00 AM",
                        "Anytime",
                        "I am not available"

                    };
                    qar.Questions.Add(new DomainDto.Question()
                    {
                        questionText = "During weekday, what time are you available for group calls?",
                        questionType = "MultipleChoice",
                        answerChoices = timeSlots
                    });
                    qar.Questions.Add(new DomainDto.Question()
                    {
                        questionText = "During weekends, what time are you available for group calls?",
                        questionType = "MultipleChoice",
                        answerChoices = timeSlots
                    });

                    if (model.GenerateIntelligentQuestionnaire)
                    {
                        if (model.DesiredSkillSets != null)
                        {
                            foreach (var skl in model.DesiredSkillSets.Split(','))
                            {
                                qar.Questions.Add(new DomainDto.Question()
                                    {
                                        questionText = "What is your expertise in " + skl.Trim(),
                                        questionType = "MultipleChoice",
                                        answerChoices = new List<string>() { "Beginner", "Intermediate", "Expert" }
                                    });
                            }
                        }
                    }

                    if(model.Questions != null && model.Questions.Count > 0)
                    {
                        foreach(var q in model.Questions)
                        {
                            qar.Questions.Add(new DomainDto.Question()
                                {
                                    questionType = q.QuestionType,
                                    questionText  = q.QuestionText,
                                    answerChoices = q.AnswerChoices.Split(',').ToList(),
                                    Priority = getPriority(q.Priority)
                                });
                        }
                    }
                    bool res = _questionnaireCom.AddQuestionnaire(qar);
                    if(res)//Questionnaire is added, update course with questionnaire
                    {
                        CourseUpdateRequest cur = new CourseUpdateRequest()
                        {
                            CourseCode = model.CourseCode,
                            QuestionnaireCode = qar.QuestionnaireCode
                        };
                        bool resp = _courseCom.UpdateCourse(cur);
                        if(!resp)
                        {
                            ModelState.AddModelError("", "Oops! Course was added, but someting wrong happened while adding questionnaire to course");
                            return View(model);
                        }
                    }
                    //MvcApplication.courses.Add(model.CourseCode, model.CourseName);
                    //MvcApplication.courseDescription.Add(model.CourseCode, model.CourseDescription);
                    return RedirectToAction("Teacher", "Course");
                }
            }
            ModelState.AddModelError("", "Oops! Something wrong happened! Please try again.");
            return View(model);
        }

        public ActionResult Teacher()
        {
            UserGetResponse ugr = _userCom.GetUser(User.Identity.Name);
            if (ugr.UserCourseDetails == null || ugr.UserCourseDetails.Count == 0)
            {
                return RedirectToAction("UnregisteredTeacher");
            }
            else
            {
                TeacherDashboardModel tdm = new TeacherDashboardModel();
                tdm.Courses = new Dictionary<string, string>();
                tdm.CourseDescription = new Dictionary<string, string>();
                foreach (var ucd in ugr.UserCourseDetails)
                {
                    tdm.Courses.Add(ucd.courseCode, ucd.CourseName);
                    tdm.CourseDescription.Add(ucd.courseCode, ucd.CourseDescription);
                }
                return View(tdm);
            }
        }

        public ActionResult CourseTeacher(string courseId)
        {
            CourseGetResponse cgr = _courseCom.GetCourse(courseId);
            DisplayCourseModel dcm = new DisplayCourseModel();
            if(cgr != null)
            {
                dcm.CourseCode = cgr.CourseCode;
                dcm.CourseDescription = cgr.CourseDescription;
                dcm.CourseName = cgr.CourseName;
                dcm.DesiredSkillSets = cgr.DesiredSkillSets;
                dcm.GroupSize = (int)cgr.GroupSize;
                dcm.GroupType = cgr.GroupType;
                if (!string.IsNullOrWhiteSpace(cgr.Roaster))
                    dcm.Roaster = cgr.Roaster.Split(',').ToList();
                else
                    dcm.Roaster = null;
                dcm.PreferSimiliarSkillSet = (bool)cgr.PreferSimiliarSkillSet;
                dcm.Users = new List<GT.BuddyUp.WebApp.Models.DisplayCourseUser>();
                dcm.Questions = new List<GT.BuddyUp.WebApp.Models.DsiplayQuestion>();
                dcm.Groups = new List<DisplayGroup>();
            }

            foreach(var user in cgr.UserList)
            {
                dcm.Users.Add(new DisplayCourseUser()
                    {
                        emailId = user.EmailID,
                        name = user.Name,
                        role = user.RoleCode
                    });
            }

            foreach(var grp in cgr.CourseGroups)
            {
                var dg = new DisplayGroup()
                    {
                        GroupCode = grp.GroupCode,
                        GroupName = grp.GroupName,
                        Objective = grp.Objective,
                        TimeZone = grp.TimeZone,
                        GroupMembers = new List<DisplayCourseUser>()
                    };
                foreach(var gm in grp.GroupUsers)
                {
                    dg.GroupMembers.Add(new DisplayCourseUser()
                        {
                            emailId = gm.EmailID,
                            name = gm.Name,
                            role = gm.RoleCode
                        });
                }
                dcm.Groups.Add(dg);
            }

            QuestionnaireGetResponse qgr = _questionnaireCom.GetQuestionnaire(cgr.QuestionnaireCode);

            if(qgr != null)
            {
                foreach(var q in qgr.Questions)
                {
                    dcm.Questions.Add(new GT.BuddyUp.WebApp.Models.DsiplayQuestion()
                        {
                             QuestionText = q.questionText,
                              QuestionType = q.questionType,
                               AnswerChoices = q.answerChoices
                        });
                }
            }
            return View(dcm);
        }

        private QuestionPriority getPriority(Priority priority)
        {
            if (priority == Priority.High)
                return QuestionPriority.High;
            else if (priority == Priority.Medium)
                return QuestionPriority.Medium;
            else
                return QuestionPriority.Low;
        }

        public ActionResult Modify(string courseCode)
        {
            CourseGetResponse cgr = _courseCom.GetCourse(courseCode);
            ModifyGroupsModel mgm = new ModifyGroupsModel();
            mgm.courseCode = cgr.CourseCode;
            mgm.courseName = cgr.CourseName;
            mgm.courseDescription = cgr.CourseDescription;
            mgm.UserAndGroups = new List<GroupModifiableModel>();
            List<string> groups = null;
            if (cgr.CourseGroups != null)
                groups = cgr.CourseGroups.Select(x => x.GroupName).ToList();
            foreach (var grp in cgr.CourseGroups)
            {
                foreach (var gm in grp.GroupUsers)
                {
                    GroupModifiableModel gmm = new GroupModifiableModel();
                    gmm.GroupName = grp.GroupName;
                    gmm.Objective = grp.Objective;
                    gmm.NewGroups = groups;
                    gmm.SuggestedGroups = ""; //_courseUserCom.getSuggestedGroups(gm.EmailID, cgr.CourseCode);
                    gmm.MemberName = gm.Name;
                    gmm.EmailId = gm.EmailID;
                    mgm.UserAndGroups.Add(gmm);
                }
            }
            return View(mgm);
        }

        [HttpPost]
        public async Task<ActionResult> Modify(ModifyGroupsModel model)
        {
            if(ModelState.IsValid)
            {
                return RedirectToAction("Modify");
            }
            return View(model);
        }

        public ActionResult ModifyRoaster(string courseCode, string message = null)
        {
            CourseGetResponse cgr = _courseCom.GetCourse(courseCode);
            ModifyRoasterModel mrm = new ModifyRoasterModel();
            mrm.courseCode = cgr.CourseCode;
            mrm.message = message;
            mrm.courseName = cgr.CourseName;
            if(string.IsNullOrWhiteSpace(cgr.Roaster))
                mrm.roasterIds = null;
            else
            {
                mrm.roasterIds = new List<RoasterID>();
                foreach(var gtid in cgr.Roaster.Split(','))
                {
                    mrm.roasterIds.Add(new RoasterID() { gtId = gtid, isMarkedForDeletion =false  });
                }
            }
            return View(mrm);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyRoaster(ModifyRoasterModel model)
        {
            if (ModelState.IsValid)
            {
                StringBuilder roaster = new StringBuilder();
                if (model.roasterIds != null)
                {
                    roaster.Append(string.Join(",", model.roasterIds.Select(x => x.isMarkedForDeletion == false)));
                    
                }
                if(!string.IsNullOrWhiteSpace(model.newRoasterIds))
                {
                    if (model.roasterIds != null)
                        roaster.Append(",");
                    foreach(var id in model.newRoasterIds.Split(','))
                    {
                        roaster.Append(id.Trim());
                        roaster.Append(",");
                    }
                    roaster.Remove(roaster.Length - 1, 1);
                }
                CourseUpdateRequest cur = new CourseUpdateRequest();
                cur.CourseCode = model.courseCode;
                cur.Roaster = roaster.ToString();
                bool resp = _courseCom.UpdateCourse(cur);
                if (resp)
                {
                    return RedirectToAction("ModifyRoaster", "Course", routeValues: new { courseCode = model.courseCode, message = "Success" });
                }
            }
            return RedirectToAction("ModifyRoaster", "Course", routeValues: new { courseCode = model.courseCode, message = "Failure" });
        }
    }
}