namespace LabEF.Domain;

public abstract class Vehiculo
{
    public int Id { get; set; }
    public string Marca { get; set; } = "";
    public string Modelo { get; set; } = "";
    public int Anio { get; set; }
}
