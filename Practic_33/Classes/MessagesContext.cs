using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Practic_33.Classes.Common;
using Practic_33.Models;

namespace Practic_33.Classes
{
    public class MessagesContext : DbContext
    {
        /// <summary> Данные из БД </summary>
        public DbSet<Messages> Messages { get; set; }

        /// <summary> Конструктор контекста </summary>
        public MessagesContext() =>
            Database.EnsureCreated();

        /// <summary> Конфигурация подключения </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            // Говорим что используем SQL Server со следующей конфигурацией
            optionsBuilder.UseSqlServer(Config.config);
    }
}
