
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
            //��DbContext���뵽����
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

            app.UseDefaultFiles(); //����ȱʡ��̬ҳ�棨index.html��index.htm��
            app.UseStaticFiles(); //���þ�̬�ļ���ҳ�桢js��ͼƬ�ȸ���ǰ���ļ���

            app.UseHttpsRedirection(); //����http��https���ض���
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}