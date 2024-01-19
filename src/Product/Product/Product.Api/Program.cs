using Product.Api.Middleware;
using Product.Application;
using Product.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();

    // Add services to the container.
    builder.Services.AddApplication(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);

    //builder.Services.AddProblemDetails();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        //app.UseExceptionHandler("/error-development");
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    //app.UseExceptionHandler("/error");

    //app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}

public partial class Startup { }
