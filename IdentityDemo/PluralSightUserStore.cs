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
    public class PluralSightUserStore : IUserStore<PluralSightUser>
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
                "[PasswordHash]" +
                "Values(@id,@userName,@normalizedUserName,@passwordHash)",
                new
                {
                    id = user.ID,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    passwordHash = user.PasswordHash
                } );

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync( PluralSightUser user, CancellationToken cancellationToken ) => throw new NotImplementedException( );

        public void Dispose( )
        {

        }

        public Task<PluralSightUser> FindByIdAsync( string userId, CancellationToken cancellationToken ) => throw new NotImplementedException( );

        public Task<PluralSightUser> FindByNameAsync( string normalizedUserName, CancellationToken cancellationToken ) => throw new NotImplementedException( );


        public Task<string> GetNormalizedUserNameAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( user.NormalizedUserName );
        }

        public Task<string> GetUserIdAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( user.ID );
        }

        public Task<string> GetUserNameAsync( PluralSightUser user, CancellationToken cancellationToken )
        {
            return Task.FromResult( user.UserName );
        }

        public Task SetNormalizedUserNameAsync( PluralSightUser user, string normalizedName, CancellationToken cancellationToken )
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync( PluralSightUser user, string userName, CancellationToken cancellationToken )
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync( PluralSightUser user, CancellationToken cancellationToken ) => throw new NotImplementedException( );
    }
}
