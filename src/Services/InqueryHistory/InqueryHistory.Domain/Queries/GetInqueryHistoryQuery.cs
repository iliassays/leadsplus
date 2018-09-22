namespace InqueryHistory.Domain.Query
{
    using Contact.Domain;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Threading.Tasks;
    using LeadsPlus.Core.Query;
    using LeadsPlus.Core;

    public class GetInqueryHistoryQuery
    {
        public string InqueryHistoryId { get; set; }
    }

    public class GetInqueryHistoryQueryHandler : IQueryHandler<GetInqueryHistoryQuery, InqueryHistory>
    {
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;

        public GetInqueryHistoryQueryHandler(IRepository<InqueryHistory> inqueryHistoryRepository)
        {
            this.inqueryHistoryRepository = inqueryHistoryRepository;
        }

        public async Task<InqueryHistory> Handle(GetInqueryHistoryQuery query)
        {
            var filter = Builders<InqueryHistory>.Filter.Eq("Id", query.InqueryHistoryId);

            return await inqueryHistoryRepository.Collection
                                .Find(filter)
                                .FirstOrDefaultAsync();
        }
    }
}
