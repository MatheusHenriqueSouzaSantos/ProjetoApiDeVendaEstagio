namespace ApiEstagioBicicletaria.Entities
{
    public abstract class EntidadeBase
    {
        [AnotacaoDeAtributoASerIgnoradoLog]
        public Guid Id { get; private set; }= Guid.NewGuid();

        [AnotacaoDeAtributoASerIgnoradoLog]
        public DateTime DataCriacao {  get; private set; }= DateTime.Now;

        [AnotacaoDeAtributoASerIgnoradoLog]
        public bool Ativo { get; set; } = true;

        protected EntidadeBase() { }
    }
}
