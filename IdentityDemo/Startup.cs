using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityDemo
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PluralSightUser;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            var migrationAssembly = typeof( Startup ).GetTypeInfo( ).Assembly.GetName( ).Name;
            services.AddDbContext<PluralSightUserDbContext>( options => options.UseSqlServer( connectionString, 
                sql => sql.MigrationsAssembly( migrationAssembly ) ) );

            services.AddControllersWithViews( );            
            services.AddIdentityCore<PluralSightUser>( OptionsBuilderConfigurationExtensions => { } );
            services.AddScoped<IUserStore<PluralSightUser>, UserOnlyStore<PluralSightUser, PluralSightUserDbContext>>( );

            services.AddAuthentication( "Cookies" ).AddCookie( 
                "Cookies", options => options.LoginPath = "/Home/Login"  );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment( ) )
            {
                app.UseDeveloperExceptionPage( );
            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts( );
            }
            app.UseHttpsRedirection( );
            app.UseStaticFiles( );

            app.UseRouting( );

            app.UseAuthentication( );
            app.UseAuthorization( );

            app.UseEndpoints( endpoints =>
             {
                 endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}" );
             } );
        }
    }
}
