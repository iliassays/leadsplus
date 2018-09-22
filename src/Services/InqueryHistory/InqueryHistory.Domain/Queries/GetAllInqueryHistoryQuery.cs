namespace InqueryHistory.Domain.Query
{
    using Contact.Domain;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Threading.Tasks;
    using LeadsPlus.Core.Query;
    using LeadsPlus.Core;
    using MongoDB.Bson;
    using System.Collections.Generic;

    public class GetAllInqueryHistoryQuery
    {
        
    }

    public class GetAllInqueryHistoryHandler : IQueryHandler<GetAllInqueryHistoryQuery, List<InqueryHistory>>
    {
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;

        public GetAllInqueryHistoryHandler(IRepository<InqueryHistory> inqueryHistoryRepository)
        {
            this.inqueryHistoryRepository = inqueryHistoryRepository;
        }

        public async Task<List<InqueryHistory>> Handle(GetAllInqueryHistoryQuery query)
        {
            return await inqueryHistoryRepository.Collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
