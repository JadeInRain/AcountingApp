using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
namespace TodoApi.Models
{
    public class TodoDbContext : DbContext
    {

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<CashFlow> CashFlow { get; set; }

        // 重写此方法以指定表名
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashFlow>(entity =>
            {
                entity.ToTable("cashflow"); // 指定表名为 cashflow
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.Amount).HasColumnName("amount");
            });
        }
    }
}
