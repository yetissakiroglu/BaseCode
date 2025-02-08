namespace Economy.Core.Models
{
    public class ResultValidationError
    {
        public Dictionary<string, List<string>> ValidationError { get; private set; }
        public bool IsShow { get; private set; }

        public ResultValidationError(Dictionary<string, List<string>> ValidationError, bool isShow)
        {
            this.ValidationError = ValidationError;
            IsShow = isShow;
        }
        public ResultValidationError(Dictionary<string, List<string>> ValidationError)
        {
            this.ValidationError = ValidationError;
            IsShow = false;
        }

    }
}
