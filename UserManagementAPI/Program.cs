using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Helpers;
using UserManagementAPI.Persistence;
using UserManagementAPI.Services.Interfaces;
using UserManagementAPI.Services;
using FluentValidation;
using UserManagementAPI.Dto;
using UserManagementAPI.Validators;
using FluentValidation.AspNetCore;
using UserManagementAPI.Middlewares;

namespace UserManagementAPI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddTransient<IValidator<CreateUserDto>, CreateUserDtoValidator>();
            builder.Services.AddTransient<IValidator<UpdateUserDto>, UpdateUserDtoValidator>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<UserManagementDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}