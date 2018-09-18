namespace Contact.Projection.Query
{
    using Contact.Domain;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Query;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using System.Threading.Tasks;

    public class GetContactByEmailAndOwnerQuery
    {
        public string Email { get; set; }
        public string OwnerId { get; set; }
    }

    public class GetContactByEmailAndOwnerQueryHandler : IQueryHandler<GetContactByEmailAndOwnerQuery, Contact>
    {
        private readonly IRepository<Contact> contactRepository;

        public GetContactByEmailAndOwnerQueryHandler(IRepository<Contact> contactQueries)
        {
            this.contactRepository = contactQueries;
        }

        public Task<Contact> Handle(GetContactByEmailAndOwnerQuery query)
        {
            var filter = Builders<Contact>.Filter.Where(x => x.Email == query.Email && x.CreatedBy == query.OwnerId);

            return contactRepository.Collection
                                .Find(filter)
                                .FirstOrDefaultAsync();
        }
    }
}
