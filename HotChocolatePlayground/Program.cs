using HotChocolatePlayground;
using HotChocolatePlayground.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddProjections()
    .AddQueryType<Query>();

builder.Services
    .AddDbContext<ApplicationContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGraphQL();
app.MapGet("/", () => "Hello World!");
app.Run();