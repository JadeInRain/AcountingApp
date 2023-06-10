
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace todo_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //把DbContext加入到容器
            builder.Services.AddDbContext<TodoContext>(opt => opt.UseMySQL(
                "Server=localhost;Database=todoDB;User=root;Password=wyx20030218"));


            builder.Services.AddCors(c =>
            {
                c.AddPolicy("defaultCors", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .SetIsOriginAllowed(_ => true)
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            app.UseCors("defaultCors");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles(); //启用缺省静态页面（index.html或index.htm）
            app.UseStaticFiles(); //启用静态文件（页面、js、图片等各种前端文件）

            app.UseHttpsRedirection(); //启动http到https的重定向
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}