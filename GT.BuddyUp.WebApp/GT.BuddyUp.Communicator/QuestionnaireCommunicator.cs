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
    public class QuestionnaireCommunicator
    {
        IQuestionnaire Questionnaire;
        public QuestionnaireCommunicator(IQuestionnaire Questionnaire)
        {
            this.Questionnaire = Questionnaire;
        }

        public bool AddQuestionnaire(QuestionnaireAddRequest request)
        {
            try
            {
                Questionnaire.Add(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public QuestionnaireGetResponse GetQuestionnaire(string questionnaireCode)
        {
            try
            {
                return Questionnaire.Get(questionnaireCode).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
