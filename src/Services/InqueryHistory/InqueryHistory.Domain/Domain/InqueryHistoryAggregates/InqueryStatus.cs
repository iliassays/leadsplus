namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using LeadsPlus.Core.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class InqueryStatus
        : Enumeration
    {
        public static InqueryStatus Submitted = new InqueryStatus(1, nameof(Submitted).ToLowerInvariant());
        public static InqueryStatus SentForParsing = new InqueryStatus(2, nameof(SentForParsing).ToLowerInvariant());
        public static InqueryStatus Parsed = new InqueryStatus(3, nameof(Parsed).ToLowerInvariant());
        public static InqueryStatus AutoresponderSending = new InqueryStatus(4, nameof(AutoresponderSending).ToLowerInvariant());
        public static InqueryStatus AutoresponderSendingCompleted = new InqueryStatus(5, nameof(AutoresponderSendingCompleted).ToLowerInvariant());
        public static InqueryStatus Processed = new InqueryStatus(6, nameof(Processed).ToLowerInvariant());
        public static InqueryStatus Failed = new InqueryStatus(7, nameof(Failed).ToLowerInvariant());

        protected InqueryStatus()
        {
        }

        public InqueryStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<InqueryStatus> List() =>
            new[] { Submitted, SentForParsing, Parsed, AutoresponderSending, Processed, Failed };

        public static InqueryStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new DomainException($"Possible values for InqueryStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static InqueryStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new DomainException($"Possible values for InqueryStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
