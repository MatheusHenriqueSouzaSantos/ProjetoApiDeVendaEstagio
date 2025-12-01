using ApiEstagioBicicletaria.Repositories;

namespace ApiEstagioBicicletaria.Utils
{
    public class GeradorCodigoVenda
    {
        private static  Random _radom = new Random();

        private readonly ContextoDb _contextoDb;

        private const int _tamanhoDoCodigoDaVenda = 6;
        private const string _caracteresParaACombinacao = "abcdefghjkmnpqrstuvwxyz23456789";

        public GeradorCodigoVenda(ContextoDb contextoDb)
        {
            _contextoDb= contextoDb;
        }

        public string GerarCodigoVenda()
        {
           
            string codigoGerado;

            do
            {
                char[] codigo = new char[_tamanhoDoCodigoDaVenda];
                for (int i = 0; i < _tamanhoDoCodigoDaVenda; i++)
                {
                    int indexAleatorio = _radom.Next(_caracteresParaACombinacao.Length);
                    codigo[i] = _caracteresParaACombinacao[indexAleatorio];
                }
                codigoGerado = new string(codigo);

            }
            while (VerificarSeOCodigoDaVendaGeradoJaExisteNoBanco(codigoGerado));

            return codigoGerado;

        }

        public bool VerificarSeOCodigoDaVendaGeradoJaExisteNoBanco(string codigoGerado)
        {
            return _contextoDb.Vendas.Any(v => v.CodigoVenda == codigoGerado);
        }
    }
}
