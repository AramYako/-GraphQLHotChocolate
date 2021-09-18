using GraphQLHotChocolate.Data;
using GraphQLHotChocolate.GraphQl;
using GraphQLHotChocolate.GraphQl.Platforms;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLHotChocolate
{
    public class Startup
    {
        public IConfiguration _config { get; }

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region DB
            ////DbConnection
            services.AddPooledDbContextFactory<AppDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("CommandConStr")));
            #endregion

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddType<PlatformType>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddType<GraphQLHotChocolate.GraphQl.Commands.CommandType>()
                .AddFiltering()
                .AddSorting()
                .AddInMemorySubscriptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseWebSockets();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

            app.UseGraphQLVoyager(new GraphQL.Server.Ui.Voyager.GraphQLVoyagerOptions()
            {
                GraphQLEndPoint = "/graphql",
                Path = "/graphql-voyager"
            });

        }
    }
}
