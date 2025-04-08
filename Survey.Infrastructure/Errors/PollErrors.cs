using Survey.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Errors
{
    public static class PollErrors
    {
        public static Error PollNotFound => new Error("Poll Not Found", "The requested poll was not found.");
        public static Error PollAlreadyExists => new Error("Poll Already Exists", "A poll with the same name already exists.");
        public static Error PollCreationFailed => new Error("Poll Creation Failed", "Failed to create the poll. Please try again.");
        public static Error PollUpdateFailed => new Error("Poll Update Failed", "Failed to update the poll. Please try again.");
        public static Error PollDeletionFailed => new Error("Poll Deletion Failed", "Failed to delete the poll. Please try again.");
        public static Error PollNotActive => new Error("Poll Not Active", "The requested poll is not active or has expired.");
        public static Error PollAlreadyClosed => new Error("Poll Already Closed", "The requested poll has already been closed.");

    }
}
