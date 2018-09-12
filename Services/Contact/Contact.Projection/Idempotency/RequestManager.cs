﻿
//namespace Contact.Projection.Idempotency
//{
//    using Contact.Domain.Exceptions;
//    using System;
//    using System.Threading.Tasks;
//    using Contact.Database.Context;

//    public class RequestManager : IRequestManager
//    {
//        private readonly ContactContext _context;

//        public RequestManager(ContactContext context)
//        {
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//        }


//        public async Task<bool> ExistAsync(Guid id)
//        {
//            var request = await _context.
//                FindAsync<ClientRequest>(id);

//            return request != null;
//        }

//        public async Task CreateRequestForCommandAsync<T>(Guid id)
//        {
//            var exists = await ExistAsync(id);

//            var request = exists ?
//                throw new ContactDomainException($"Request with {id} already exists") :
//                new ClientRequest()
//                {
//                    Id = id,
//                    Name = typeof(T).Name,
//                    Time = DateTime.UtcNow
//                };

//            _context.Add(request);

//            await _context.SaveChangesAsync();
//        }
//    }
//}
