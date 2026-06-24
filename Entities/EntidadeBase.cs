namespace ApiEstagioBicicletaria.Entities
{
    public abstract class EntidadeBase
    {
        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public Guid Id { get; private set; }= Guid.NewGuid();

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public DateTime DataCriacao {  get; private set; }= DateTime.Now;

        [AtributoASerIgnoradoLogCriacao]
        [AtributoASerIgnoradoLogAtualizacao]
        public bool Ativo { get; set; } = true;

        protected EntidadeBase() { }
    }
}
