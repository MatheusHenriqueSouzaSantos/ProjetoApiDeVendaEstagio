namespace ApiEstagioBicicletaria.Entities
{
    public abstract class EntidadeBase
    {
        public Guid Id { get; private set; }= Guid.NewGuid();

        public DateTime DataCriacao {  get; private set; }= DateTime.Now;

        public bool Ativo { get; set; } = true;

        protected EntidadeBase() { }
    }
}
