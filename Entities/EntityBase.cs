namespace ApiEstagioBicicletaria.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }= Guid.NewGuid();

        public DateTime DataCriacao {  get; private set; }= DateTime.Now;

        public bool Ativo { get; set; } = false;

        protected EntityBase() { }
    }
}
