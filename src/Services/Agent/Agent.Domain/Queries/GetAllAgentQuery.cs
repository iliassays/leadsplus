namespace Agent.Domain.Query
{
    using Contact.Domain;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Threading.Tasks;
    using LeadsPlus.Core.Query;
    using LeadsPlus.Core;
    using MongoDB.Bson;
    using System.Collections.Generic;

    public class GetAllAgentQuery
    {
        
    }

    public class GetAllAgentQueryHandler : IQueryHandler<GetAllAgentQuery, List<Agent>>
    {
        private readonly IRepository<Agent> contactRepository;

        public GetAllAgentQueryHandler(IRepository<Agent> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task<List<Agent>> Handle(GetAllAgentQuery query)
        {
            return await contactRepository.Collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
