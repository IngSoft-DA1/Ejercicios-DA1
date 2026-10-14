using LabEF.Domain;
using Microsoft.EntityFrameworkCore;

namespace LabEF.DataAccess;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
    }

    // TODO 1: agregá acá las propiedades que le dicen a EF qué clases tiene que guardar en la base.
}
