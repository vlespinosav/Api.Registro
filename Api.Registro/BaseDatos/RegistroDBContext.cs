using Api.Registro.Modelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Registro.BaseDatos
{
    public class RegistroDBContext : DbContext
    {

        public RegistroDBContext(DbContextOptions options) : base(options)
        {
        }
       
        public DbSet<Compania> DetalleCompanias { get; set; }
    }
}
