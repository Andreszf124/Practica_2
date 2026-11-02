namespace Practica_2.Application.Dtos
{
    public class Respuesta<T>
    {
        public bool EsCorrecto { get; set; } = true;

        public string Mensaje { get; set; } = string.Empty;

        public int Codigo { get; set; } = 200;

        public T? Dato { get; set; }
    }
}
