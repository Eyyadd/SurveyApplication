using Survey.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Errors
{
    public static class QuestionErrors
    {
        public static Error QuestionNotFound => new Error("Question Not Found", "The requested question was not found.");
        public static Error QuestionAlreadyExists => new Error("Question Already Exists", "A question with the same name already exists.");
        public static Error QuestionCreationFailed => new Error("Question Creation Failed", "Failed to create the question. Please try again.");
        public static Error QuestionUpdateFailed => new Error("Question Update Failed", "Failed to update the question. Please try again.");
        public static Error QuestionDeletionFailed => new Error("Question Deletion Failed", "Failed to delete the question. Please try again.");
        public static Error QuestionNotActive => new Error("Question Not Active", "The requested question is not active or has expired.");
        public static Error QuestionStatusChangeFailed => new Error("Question Status Change Failed", "Failed to change the status of the question. Please try again.");


    }
}
