using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityDemo
{
    public class PluralSightUserStore : IUserStore<PluralSightUser>, IUserPasswordStore<PluralSightUser>
    {
        public static DbConnection GetOpenConnection( )
        {
            var connection = new SqlConnection( @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PluralSightDemo;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" );
            connection.Open( );
            return connection;
        }

        public async Task<IdentityResult> CreateAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            using var connection = GetOpenConnection( );
            await connection.ExecuteAsync(
                "insert into PluralSightUsers([Id]," +
                    "[UserName]," +
                    "[NormalizedUserName]," +
                    "[PasswordHash]) " +
                    "Values(@id,@userName,@normalizedUserName,@passwordHash)",
                new
                {
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    passwordHash = user.PasswordHash
                } );

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return IdentityResult.Success;
        }

        public void Dispose( )
        {

        }

        public async Task<PluralSightUser> FindByIdAsync( string userId, CancellationToken cancellationToken )
        {
            using var connection = GetOpenConnection( );
            return await connection.QueryFirstOrDefaultAsync<PluralSightUser>(
                "select * From PluralSightUsers where Id = @id", new { id = userId } );
        }

        public async Task<PluralSightUser> FindByNameAsync( string normalizedUserName, CancellationToken cancellationToken )
        {
            using var connection = GetOpenConnection( );
            return await connection.QueryFirstOrDefaultAsync<PluralSightUser>(
                "select * From PluralSightUsers where NormalizedUserName = @name", 
                new { name = normalizedUserName } );
        }


        public Task<string> GetNormalizedUserNameAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( user.NormalizedUserName );
        }

        public Task<string> GetPasswordHashAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( user.PasswordHash );
        }

        public Task<string> GetUserIdAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( user.Id );
        }

        public Task<string> GetUserNameAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( user.UserName );
        }

        public Task<bool> HasPasswordAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( !string.IsNullOrWhiteSpace( user.PasswordHash ) );
        }

        public Task SetNormalizedUserNameAsync( PluralSightUser user, string normalizedName, CancellationToken cancellationToken )
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync( PluralSightUser user, string passwordHash, CancellationToken cancellationToken )
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync( PluralSightUser user, string userName, CancellationToken cancellationToken )
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            using var connection = GetOpenConnection( );
            await connection.ExecuteAsync(
                "update PluralSightUsers " +
                "set [Id] = @id," +
                "[UserName] = @userName," +
                "[NormalizedUserName] = @normalizedUserName," +
                "[PasswordHash] = @passwordHash" +
                "where [Id] = @id",
                new
                {
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    passwordHash = user.PasswordHash
                } );

            return IdentityResult.Success;
        }
    }
}
