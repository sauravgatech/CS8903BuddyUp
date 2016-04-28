using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Question Class
    /// </summary>
    public class Question
    {
        public Question()
        {
            Priority = QuestionPriority.High;
        }

        /// <summary>
        /// Question Id
        /// </summary>
        public int questionId { get; set; }
        /// <summary>
        /// Question Text
        /// </summary>
        [Required(ErrorMessage="QuestionText is required"), StringLength(128, ErrorMessage="String length can not be greater than 128 characters")]
        public string questionText { get; set; }

        /// <summary>
        /// Type of Question - MultipleChoice / FillUpTheBlank
        /// </summary>
        public string questionType { get; set; }

        /// <summary>
        /// List of Answer Choices
        /// </summary>
        public virtual List<string> answerChoices { get; set; }

        /// <summary>
        /// Question Priority
        /// </summary>
        public QuestionPriority Priority { get; set; }
    }


    public enum QuestionPriority
    {
        High,
        Medium,
        Low
    }
}
