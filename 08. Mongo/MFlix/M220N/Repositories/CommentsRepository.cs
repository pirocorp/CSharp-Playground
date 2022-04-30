using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using M220N.Models;
using M220N.Models.Projections;
using M220N.Models.Responses;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace M220N.Repositories
{
    using System.Linq;

    public class CommentsRepository
    {
        private readonly IMongoCollection<Comment> _commentsCollection;
        private readonly MoviesRepository _moviesRepository;

        public CommentsRepository(IMongoClient mongoClient)
        {
            var camelCaseConvention = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _commentsCollection = mongoClient.GetDatabase("sample_mflix").GetCollection<Comment>("comments");
            _moviesRepository = new MoviesRepository(mongoClient);
        }

        /// <summary>
        ///     Adds a comment.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movieId"></param>
        /// <param name="comment"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The Movie associated with the comment.</returns>
        public async Task<Movie> AddCommentAsync(User user, ObjectId movieId, string comment,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var newComment = new Comment
                {
                    Date = DateTime.UtcNow,
                    Text = comment,
                    Name = user.Name,
                    Email = user.Email,
                    MovieId = movieId
                };

                await _commentsCollection.InsertOneAsync(newComment, cancellationToken: cancellationToken);

                return await _moviesRepository.GetMovieAsync(movieId.ToString(), cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Updates an existing comment. Only the comment owner can update the comment.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movieId"></param>
        /// <param name="commentId"></param>
        /// <param name="comment"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An UpdateResult</returns>
        public async Task<UpdateResult> UpdateCommentAsync(User user,
            ObjectId movieId, ObjectId commentId, string comment,
            CancellationToken cancellationToken = default)
        {
            var filter = Builders<Comment>.Filter.And(
                Builders<Comment>.Filter.Eq(f => f.Id, commentId),
                Builders<Comment>.Filter.Eq(f => f.MovieId, movieId),
                Builders<Comment>.Filter.Eq(f => f.Email, user.Email));

            var commentEntity = await (await _commentsCollection
                .FindAsync(filter, cancellationToken: cancellationToken))
                .SingleOrDefaultAsync(cancellationToken);

            var update = Builders<Comment>.Update
                .Set(c => c.Text, comment);

            var updateOptions = new UpdateOptions
            {
                IsUpsert = false
            };

            return await _commentsCollection.UpdateOneAsync(
                filter,
                update,
                updateOptions,
                cancellationToken);
        }

        /// <summary>
        ///     Deletes a comment. Only the comment owner can delete a comment.
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="commentId"></param>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The movie associated with the comment that is being deleted.</returns>
        public async Task<Movie> DeleteCommentAsync(ObjectId movieId, ObjectId commentId,
            User user, CancellationToken cancellationToken = default)
        {
            await _commentsCollection.DeleteOneAsync(
                Builders<Comment>.Filter.Where(c => c.MovieId == movieId && c.Id == commentId && c.Email == user.Email), 
                    cancellationToken);

            return await _moviesRepository.GetMovieAsync(movieId.ToString(), cancellationToken);
        }

        public async Task<TopCommentsProjection> MostActiveCommentersAsync()
        {
            try
            {
                var sortDefinition = Builders<ReportProjection>.Sort
                    .Descending(f => f.Count);

                var result = await _commentsCollection
                    .WithReadConcern(ReadConcern.Majority)
                    .Aggregate()
                    .Group(
                        c => c.Email,
                        g => new ReportProjection
                        {
                            Id = g.Key,
                            Count = g.Sum(r => 1)
                        })
                    .Sort(sortDefinition)
                    .Limit(20)
                    .ToListAsync();

                return new TopCommentsProjection(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
