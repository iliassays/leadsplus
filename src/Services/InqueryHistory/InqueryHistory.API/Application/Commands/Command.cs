namespace InqueryHistory.Command
{
    public abstract class Command
    {
        public string AggregateId { get; set; }
        public string UserId { get; set; }

        public virtual string[] Validate()
        {
            return new string[] { };
        }
    }
}
