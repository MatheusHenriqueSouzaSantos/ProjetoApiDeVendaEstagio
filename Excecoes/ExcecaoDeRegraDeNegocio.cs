using ApiEstagioBicicletaria.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiEstagioBicicletaria.Excecoes
{
    public class ExcecaoDeRegraDeNegocio : Exception
    {
        public int StatusCode { get; private set; }

        public ExcecaoDeRegraDeNegocio(int statusCode,string message) : base(message)
        {
            StatusCode = statusCode;
        }

    }
}
