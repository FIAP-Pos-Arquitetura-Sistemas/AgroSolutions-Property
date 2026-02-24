namespace AgroSolutions_FarmService.Models
{
    public class Propriedade
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;

        // O ID que vem do IdentityService via JWT
        public string ProdutorId { get; set; } = string.Empty;

        public List<Talhao> Talhoes { get; set; } = new List<Talhao>();
    }
}
