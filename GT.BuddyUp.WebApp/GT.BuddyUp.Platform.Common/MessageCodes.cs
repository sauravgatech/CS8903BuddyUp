using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GT.BuddyUp.Platform.Common
{
    public enum MessageCodes
    {
        //Info

        [Description("Record was saved successfully")]
        InfoSavedSuccessfully = 200,

        [Description("New record was created successfully")]
        InfoCreatedSuccessfully = 201,

        [Description("Existing record was deleted successfully")]
        InfoDeletedSuccessfully = 202,

        [Description("New record to be added")]
        InfoNewRecordToAdd = 203,

        [Description("Existing record to be updated")]
        InfoExistingRecordToUpdate = 204,

        [Description("Existing record to be deleted")]
        InfoRecordToDelete = 205,

        [Description("System is processing the action")]
        InfoProcessingAction = 206,

        [Description("Record already exists in the system")]
        InfoAlreadyExist = 207,

        [Description("Action was successful")]
        InfoActionSuccessful = 208,

        [Description("Record was saved successfully and published to queue")]
        InfoSavedAndPublishedSuccessfully = 209,

        //Warning

        [Description("Critical fields are missing; processing will continue")]
        WarnMissingFields = 300,

        [Description("Record does not exist in the system; Inserting is not allowed so, processing will continue")]
        WarnPostForUpdate = 301,

        [Description("Validation failed; Skipping this record, processing will continue")]
        WarnValidationFailed = 302,
        //Error

        [Description("Validation has failed")]
        ErrValidationFailed = 400,

        [Description("Record already exists in the system.  Please use PUT to update")]
        ErrPostForUpdate = 401,

        [Description("Record does not exist in the system")]
        ErrDoesnotExist = 402,

        [Description("The server encountered an unexpected condition which prevented it from fulfilling the request")]
        ErrInternalServerError = 403,

        [Description("System is unable to complete the requested action")]
        ErrUnableToCompleteAction = 404,

        [Description("Record already exists in the system")]
        ErrAlreadyExist = 405,

        [Description("System is unable to complete the requested action because the current record is being referenced by other records")]
        ErrReferencesExists = 406,

        [Description("The requested action cannot be completed because the taxonomy of the record(s) does not belong to the hierarchy of taxonomy you are currently logged into")]
        ErrInvalidOrg = 407,

        [Description("Password has expired. Please reset your password.")]
        ErrPasswordExpired = 408
    }

    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        public static String GetDescription(this Enum value)
        {
            var description = GetAttribute<DescriptionAttribute>(value);
            return description != null ? description.Description : null;
        }
    }
}
