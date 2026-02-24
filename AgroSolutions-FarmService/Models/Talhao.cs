namespace AgroSolutions_FarmService.Models
{
    public class Talhao
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Cultura { get; set; } = string.Empty; // Ex: Soja, Milho
        public int PropriedadeId { get; set; }
        public Propriedade Propriedade { get; set; } = null!;
    }
}
