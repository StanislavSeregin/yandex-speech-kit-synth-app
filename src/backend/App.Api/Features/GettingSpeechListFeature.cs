using App.Api.Models;
using App.Data;
using LiteDB;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace App.Api.Features;

public static class GettingSpeechListFeature
{
    public record Response(
        SpeechModel[] Models,
        int Total);

    public record Request(
        int Skip,
        int Take,
        string Text,
        string Transcription,
        OrderQuery[] OrderQueries
    ) : IRequest<Response>;

    public enum OrderDirections
    {
        Ascending = 0,
        Descending = 1
    }

    public enum OrderFields
    {
        Transcription = 1,
        Text = 2
    }

    public record OrderQuery(
        OrderFields QueryingField,
        OrderDirections OrderDirection);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILiteDbContext _liteDbContext;

        public Handler(ILiteDbContext liteDbContext)
        {
            _liteDbContext = liteDbContext;
        }

        public Task<Response> Handle(Request request, CancellationToken _)
        {
            var query = _liteDbContext.GetCollection<Speech>().Query();
            var filteredQuery = GetFilteredQuery(query, request);
            var total = filteredQuery.Count();
            var orderedQuery = GetOrderedQuery(filteredQuery, request);
            var pagedQuery = GetPagedQuery(orderedQuery, request);
            var voices = pagedQuery.ToArray();
            var voiceModels = voices.Select(SpeechModel.Map).ToArray();
            var response = new Response(voiceModels, total);
            return Task.FromResult(response);
        }

        private static ILiteQueryable<Speech> GetFilteredQuery(
            ILiteQueryable<Speech> query,
            Request request)
        {
            query = !string.IsNullOrWhiteSpace(request.Text)
                ? query.Where(x => x.Text.ToLower().Contains(request.Text.ToLower()))
                : query;

            query = !string.IsNullOrWhiteSpace(request.Transcription)
                ? query.Where(x => x.Transcription.ToLower().Contains(request.Transcription.ToLower()))
                : query;

            return query;
        }

        private static ILiteQueryable<Speech> GetOrderedQuery(
            ILiteQueryable<Speech> query,
            Request request)
        {
            static Expression<Func<Speech, object>> GetOrderExpr(OrderFields orderField) => orderField switch
            {
                OrderFields.Text => entity => entity.Text,
                OrderFields.Transcription => entity => entity.Transcription,
                _ => throw new ArgumentOutOfRangeException(nameof(orderField))
            };

            var items = request.OrderQueries
                .Select((orderQuery, index) => (
                    orderQuery.OrderDirection,
                    OrderExpr: GetOrderExpr(orderQuery.QueryingField)
                ));

            return items.Aggregate(
                query,
                (acc, item) => item switch
                {
                    (OrderDirections.Ascending, var expr) => acc.OrderBy(expr),
                    (OrderDirections.Descending, var expr) => acc.OrderByDescending(expr),
                    _ => throw new InvalidOperationException()
                });
        }

        private static ILiteQueryableResult<Speech> GetPagedQuery(
            ILiteQueryable<Speech> query,
            Request request)
            => query
                .Skip(request.Skip)
                .Limit(request.Take);
    }
}
