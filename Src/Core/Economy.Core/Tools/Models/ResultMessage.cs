namespace Economy.Core.Tools.Models
{

    public class ResultMessage
    {
        public List<string> Messages { get; private set; } = [];
        public bool IsShow { get; private set; }

        public ResultMessage(string message, bool isShow)
        {
            Messages.Add(message);
            IsShow = isShow;
        }
        public ResultMessage(string message)
        {
            Messages.Add(message);
            IsShow = true;
        }

        public ResultMessage(List<string> messages, bool isShow)
        {
            Messages = messages ?? [];
            IsShow = isShow;
        }

        public ResultMessage(List<string> messages)
        {
            Messages = messages ?? [];
            IsShow = true;
        }
    }
}
