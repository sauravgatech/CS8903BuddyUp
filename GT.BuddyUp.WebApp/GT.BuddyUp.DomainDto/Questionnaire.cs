using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Questionnaire
    /// </summary>
    public class QuestionnaireAddRequest
    {
        /// <summary>
        /// Questionnaire Code
        /// </summary>
        public string QuestionnaireCode { get; set; }
        /// <summary>
        /// List of existing questions
        /// </summary>
        public List<int> QuestionIds { get; set; }

        /// <summary>
        /// If the questionnaire is a template
        /// </summary>
        public bool IsATemplate { get; set; }

        /// <summary>
        /// List of new Questions
        /// </summary>
        public List<Question> Questions { get; set; }
    }


    public class QuestionnaireUpdateRequest
    {
        /// <summary>
        /// Questionnaire Code
        /// </summary>
        public string QuestionnaireCode { get; set; }

        /// <summary>
        /// If the questionnaire is a template
        /// </summary>
        public bool? IsATemplate { get; set; }

        /// <summary>
        /// List of existing questions to be Deleted
        /// </summary>
        public List<int> QuestionIds { get; set; }

        /// <summary>
        /// List of new Questions
        /// </summary>
        public List<Question> Questions { get; set; }
    }


    public class QuestionnaireGetResponse
    {
        /// <summary>
        /// Questionnaire Code
        /// </summary>
        public string QuestionnaireCode { get; set; }

        /// <summary>
        /// List of new Questions
        /// </summary>
        public List<Question> Questions { get; set; }
    }
}
