namespace SunLine.Community.Common
{
    public class ActionConfirmation
    {
        public static string TempDataKey = "ActionConfirmationKey";
        public bool WasSuccessful { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }

        protected ActionConfirmation()
        { 
        }

        public static ActionConfirmation CreateSuccess(string message)
        {
            return CreateSuccess(message, null);
        }

        public static ActionConfirmation CreateSuccess(string message, object value)
        {
            return new ActionConfirmation
            { 
                Message = message, 
                WasSuccessful = true, 
                Value = value 
            };
        }

        public static ActionConfirmation CreateError(string message)
        {
            return new ActionConfirmation
            { 
                Message = message, 
                WasSuccessful = false 
            };
        }
    }
}
