using LabEF.Domain;

namespace LabEF.DataAccess;

public class VehiculoRepositorio
{
    private readonly SqlContext _context;

    public VehiculoRepositorio(SqlContext context)
    {
        _context = context;
    }

    public List<Vehiculo> Listar()
    {
        return _context.Set<Vehiculo>().OrderBy(v => v.Id).ToList();
    }

    public void Agregar(Vehiculo vehiculo)
    {
        _context.Add(vehiculo);
        _context.SaveChanges();
    }
}
