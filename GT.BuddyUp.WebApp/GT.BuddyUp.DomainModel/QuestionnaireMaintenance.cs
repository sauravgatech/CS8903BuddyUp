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
    public class QuestionnaireMaintenance : IQuestionnaire
    {
        private IRepository<EntityModel.Question> _repQuestion;
        private IRepository<QuestionType> _repQuestionType;
        private IRepository<EntityModel.Questionnaire> _repQuestionnaire;
        private IRepository<EntityModel.AnswerChoice> _repAnswerChoice;
        
        private IUnitOfWork _uow;

        private DomainModelResponse _questionnaireResponse = new DomainModelResponse();

        public QuestionnaireMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<EntityModel.Question> repQuestion,
                                IRepository<QuestionType> repQuestionType,
                                IRepository<EntityModel.Questionnaire> repQuestionnaire,
                                IRepository<EntityModel.AnswerChoice> repAnswerChoice)
        {
            _repQuestion = repQuestion.Use(_uow);
            _repQuestionType = repQuestionType.Use(_uow);
            _repQuestionnaire = repQuestionnaire.Use(_uow);
            _repAnswerChoice = repAnswerChoice.Use(_uow);
        }

        public IEnumerable<QuestionnaireGetResponse> Get(string questionnaireCode = "")
        {
            IEnumerable<Questionnaire> questionnaires = _repQuestionnaire.Get(filter: u => (u.QuestionnaireCode == questionnaireCode || questionnaireCode == ""));
            List<QuestionnaireGetResponse> QuestionnaireGetResponses = new List<QuestionnaireGetResponse>();
            foreach (Questionnaire questionnaire in questionnaires)
            {
                QuestionnaireGetResponse qr = new QuestionnaireGetResponse()
                {
                    QuestionnaireCode = questionnaire.QuestionnaireCode
                };
                List<string> questions = questionnaire.QuestionSet.Split(',').ToList();
                IEnumerable<EntityModel.Question> questionSet = _repQuestion.Get(filter: f => questions.Contains(f.QuestionId.ToString()), includes: "QuestionType,AnswerChoices");
                qr.Questions = new List<DomainDto.Question>();
                foreach(var q in questionSet)
                {
                    DomainDto.Question qq = new DomainDto.Question()
                        {
                            questionId = q.QuestionId,
                            questionText = q.QuestionText,
                            questionType = q.QuestionType.Type,
                            answerChoices = new List<string>()
                        };
                    foreach(var ans in q.AnswerChoices)
                    {
                        qq.answerChoices.Add(ans.Answer);
                    }
                    qr.Questions.Add(qq);
                }
                QuestionnaireGetResponses.Add(qr);
            }
            return QuestionnaireGetResponses;
        }

        public DomainModelResponse Add(QuestionnaireAddRequest request)
        {
            Questionnaire Questionnaire = new Questionnaire();
            Questionnaire.QuestionnaireCode = request.QuestionnaireCode;
            Questionnaire.IsATemplate = request.IsATemplate;
            Questionnaire.LastChangedTime = DateTime.UtcNow;
            StringBuilder qIds = new StringBuilder();
            if(request.QuestionIds != null)
            {
                foreach(int i in request.QuestionIds)
                {
                    qIds.Append(i.ToString());
                    qIds.Append(",");
                }
                qIds.Remove(qIds.Length - 1, 1);
            }
            if(request.Questions != null)
            {
                List<QuestionType> questionTypes = _repQuestionType.Get().ToList();
                foreach(var q in request.Questions)
                {
                    EntityModel.Question eQ = new EntityModel.Question()
                    {
                        QuestionText = q.questionText,
                        LastChangedTime = DateTime.UtcNow,
                        QuestionType = questionTypes.Where(x => x.Type.Equals(q.questionType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault(),
                        QuestionTypeId = questionTypes.Where(x => x.Type.Equals(q.questionType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().QuestionTypeId,
                        Priority = q.Priority.ToString()
                    };
                    _repQuestion.Add(eQ);
                    _uow.Commit();
                    eQ = _repQuestion.Get(filter: f => f.QuestionText == q.questionText).FirstOrDefault();
                    qIds.Append(eQ.QuestionId.ToString());
                    qIds.Append(",");
                    foreach(var ans in q.answerChoices)
                    {
                        AnswerChoice ac = new AnswerChoice()
                        {
                            Answer = ans,
                            Question = eQ,
                            QuestionId = eQ.QuestionId
                        };
                        _repAnswerChoice.Add(ac);
                    }
                }
                qIds.Remove(qIds.Length - 1, 1);
            }
            Questionnaire.QuestionSet = qIds.ToString();
            _repQuestionnaire.Add(Questionnaire);
            _uow.Commit();
            _questionnaireResponse.addResponse("Add", MessageCodes.InfoCreatedSuccessfully, "questionnaire : " + request.QuestionnaireCode);
            return _questionnaireResponse;
        }

        public DomainModelResponse Update(QuestionnaireUpdateRequest request)
        {
            Questionnaire Questionnaire = _repQuestionnaire.Get(filter: f => f.QuestionnaireCode == request.QuestionnaireCode).FirstOrDefault();
            if (request.IsATemplate.HasValue)
                Questionnaire.IsATemplate = (bool)request.IsATemplate;
            List<string> qIds = Questionnaire.QuestionSet.Split(',').ToList();
            if(request.QuestionIds != null) //List of QuestionIds to be deleted
            {
                foreach(int id in request.QuestionIds)
                {
                    qIds.Remove(id.ToString());
                }
            }

            if (request.Questions != null)
            {
                List<QuestionType> questionTypes = _repQuestionType.Get().ToList();
                foreach (var q in request.Questions)
                {
                    EntityModel.Question eQ = new EntityModel.Question()
                    {
                        QuestionText = q.questionText,
                        LastChangedTime = DateTime.UtcNow,
                        QuestionType = questionTypes.Where(x => x.Type.Equals(q.questionType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault(),
                        QuestionTypeId = questionTypes.Where(x => x.Type.Equals(q.questionType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().QuestionTypeId,
                        Priority = q.Priority.ToString()
                    };
                    _repQuestion.Add(eQ);
                    _uow.Commit();
                    eQ = _repQuestion.Get(filter: f => f.QuestionText == q.questionText).FirstOrDefault();
                    qIds.Add(eQ.QuestionId.ToString());
                    foreach (var ans in q.answerChoices)
                    {
                        AnswerChoice ac = new AnswerChoice()
                        {
                            Answer = ans,
                            Question = eQ,
                            QuestionId = eQ.QuestionId
                        };
                        _repAnswerChoice.Add(ac);
                    }
                }
            }

            StringBuilder fQIds = new StringBuilder();
            foreach(string id in qIds)
            {
                fQIds.Append(id);
                fQIds.Append(",");
            }
            fQIds.Remove(fQIds.Length - 1, 1);
            Questionnaire.QuestionSet = fQIds.ToString();
            _repQuestionnaire.Update(Questionnaire);
            _uow.Commit();
            _questionnaireResponse.addResponse("Update", MessageCodes.InfoSavedSuccessfully, "questionnaire : " + request.QuestionnaireCode);
            return _questionnaireResponse;
        }

        public DomainModelResponse Delete(string questionnaireCode)
        {
            return new DomainModelResponse();
        }
    }
}
