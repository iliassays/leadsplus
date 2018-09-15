namespace Agent.Domain.Query
{
    using Contact.Domain;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Threading.Tasks;
    using LeadsPlus.Core.Query;
    using LeadsPlus.Core;

    public class GetAgentByIntegrationEmailQuery
    {
        public string AgentIntegrationEmail { get; set; }
    }

    public class GetAgentByIntegrationEmailQueryHandler : IQueryHandler<GetAgentByIntegrationEmailQuery, Agent>
    {
        private readonly IRepository<Agent> contactRepository;

        public GetAgentByIntegrationEmailQueryHandler(IRepository<Agent> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public async Task<Agent> Handle(GetAgentByIntegrationEmailQuery query)
        {
            var filter = Builders<Agent>.Filter.Eq("IntegrationEmail", query.AgentIntegrationEmail);

            return await contactRepository.Collection
                                .Find(filter)
                                .FirstOrDefaultAsync();
        }
    }
}
