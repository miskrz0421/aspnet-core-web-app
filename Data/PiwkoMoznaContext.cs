using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiwkoMozna.Models;

namespace PiwkoMozna.Data
{
    public class PiwkoMoznaContext : DbContext
    {
        public PiwkoMoznaContext (DbContextOptions<PiwkoMoznaContext> options)
            : base(options)
        {
        }

        public DbSet<PiwkoMozna.Models.UzytkownikModel> UzytkownikModel { get; set; } = default!;
        public DbSet<PiwkoMozna.Models.BrowarModel> BrowarModel { get; set; } = default!;
        public DbSet<PiwkoMozna.Models.PiwoModel> PiwoModel { get; set; } = default!;
        public DbSet<PiwkoMozna.Models.RecenzjaModel> RecenzjaModel { get; set; } = default!;
        
    }
}
