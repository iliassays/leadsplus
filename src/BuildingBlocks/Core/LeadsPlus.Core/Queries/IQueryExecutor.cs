namespace LeadsPlus.Core.Query
{
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Core;
    using MediatR;

    public interface IQueryExecutor
    {
        Task<Result> Execute<Query, Result>(Query query);
    }

    public class QueryExecutor : IQueryExecutor
    {
        private readonly IComponentContext container;
        public QueryExecutor(IComponentContext container)
        {
            this.container = container;
        }

        public Task<Result> Execute<Query, Result>(Query query)
        {
            var s = container.Resolve<IMediator>();

            var queryHandler = container.Resolve<IQueryHandler<Query, Result>>();// as IQueryHandler<Query, Result>;

            var response = queryHandler.Handle(query);

            return response;
        }
    }
}
