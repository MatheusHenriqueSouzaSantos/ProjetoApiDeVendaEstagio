using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;

namespace ApiEstagioBicicletaria.Utils
{
    public class GeradorCodigoIndentificadorMovimentacao<T>
    {
        private static  Random _random = new Random();

        private readonly ContextoDb _contextoDb;

        private const int _tamanhoDoCodigoDaVenda = 6;
        private const string _caracteresParaACombinacao = "abcdefghjkmnpqrstuvwxyz23456789";

        public GeradorCodigoIndentificadorMovimentacao(ContextoDb contextoDb)
        {
            _contextoDb= contextoDb;
        }

        public string GerarCodigo()
        {
           
            string codigoGerado;

            do
            {
                char[] codigo = new char[_tamanhoDoCodigoDaVenda];
                for (int i = 0; i < _tamanhoDoCodigoDaVenda; i++)
                {
                    int indexAleatorio = _random.Next(_caracteresParaACombinacao.Length);
                    codigo[i] = _caracteresParaACombinacao[indexAleatorio];
                }
                codigoGerado = new string(codigo);

            }
            while (VerificarSeOCodigoGeradoJaExisteNoBanco(codigoGerado));

            return codigoGerado;

        }

        public bool VerificarSeOCodigoGeradoJaExisteNoBanco(string codigoGerado)
        {
            if (typeof(T).Equals(typeof(Venda))){
                return _contextoDb.Vendas.Any(v => v.CodigoVenda == codigoGerado);
            }
            if (typeof(T).Equals(typeof(EntradaEstoque))){
                return _contextoDb.EntradasEstoque.Any(e => e.CodigoEntrada == codigoGerado);
            }
            throw new ExcecaoDeRegraDeNegocio(500,"Tipo de entidade inválida");
        }
    }
}
