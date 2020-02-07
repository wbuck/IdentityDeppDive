using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo
{
    public class PluralSightUserDbContext : IdentityDbContext<PluralSightUser>
    {
        public PluralSightUserDbContext( DbContextOptions<PluralSightUserDbContext> options ) 
            : base( options)
        {

        }

        protected override void OnModelCreating( ModelBuilder builder ) 
        {            
            base.OnModelCreating( builder );
            builder.Entity<PluralSightUser>( user => 
                user.HasIndex( p => p.Locale ).IsUnique( false ) );

            builder.Entity<Organization>( o => 
            {
                o.ToTable( "Organizations" );
                o.HasKey( x => x.Id );
                o.HasMany<PluralSightUser>( ).WithOne( )
                    .HasForeignKey( x => x.OrganizationId ).IsRequired( false );
            } );
        }
    }
}
