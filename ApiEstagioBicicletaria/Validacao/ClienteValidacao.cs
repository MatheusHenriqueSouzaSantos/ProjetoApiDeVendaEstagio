using System.Text.RegularExpressions;

namespace ApiEstagioBicicletaria.Validacao
{
    public static class ClienteValidacao
    {
        public static string RemoverNaoNumericos(String informacao)
        {
            return Regex.Replace(informacao, @"\D", "");
        }
        public static bool ValidarCpf(String cpfInformado)
        {

            if (cpfInformado.Length != 11)
            {
                return false;
            }
            if (cpfInformado.Distinct().Count() == 1)
            {
                return false;
            }
            int[] multiplicador1 = {10,9,8,7,6,5,4,3,2 };
            int[] multiplicador2 = {11,10,9,8,7,6,5,4,3,2 };

            string modeloDoCpfValido = cpfInformado.Substring(0, 9);
            int soma=0;

            for(int i=0;i<9; i++)
            {
                soma += (int)char.GetNumericValue(modeloDoCpfValido[i]) * multiplicador1[i];
            }
            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;
            modeloDoCpfValido+= primeiroDigitoVerificador.ToString();
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += (int)char.GetNumericValue(modeloDoCpfValido[i]) * multiplicador2[i];
            }

            resto= soma % 11;
            int segundoDigitoVerificador= resto < 2 ? 0 : 11 - resto;
            modeloDoCpfValido += segundoDigitoVerificador.ToString();

            if (modeloDoCpfValido != cpfInformado)
            {
                return false;
            }
                return true;
        }
        public static bool ValidarCnpj(string cnpjInformado) 
        {
         
            if (cnpjInformado.Length != 14)
            {
                return false;
            }
            if (cnpjInformado.Distinct().Count() == 1)
            {
                return false;
            }
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string modeloDoCnpjValido = cnpjInformado.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i< 12; i++){
                soma += (int)char.GetNumericValue(modeloDoCnpjValido[i]) * multiplicador1[i];
            }

            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;
            modeloDoCnpjValido+= primeiroDigitoVerificador.ToString();
            soma = 0;

            for (int i = 0; i < 13; i++)
            {
                soma += (int)char.GetNumericValue(modeloDoCnpjValido[i]) * multiplicador2[i];
            }
            resto = soma % 11;
            int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;
            modeloDoCnpjValido += segundoDigitoVerificador.ToString();

            if (modeloDoCnpjValido != cnpjInformado)
            {
                return false;
            }

            return true;
        }
        public static bool validarInscricaoEstadual(string inscricaoEstadual)
        {
            if (inscricaoEstadual == "")
            {
                return true;
            }
            string inscricaoEstadualSomenteNumeros = Regex.Replace(inscricaoEstadual, @"\D", "");
            if (inscricaoEstadualSomenteNumeros.Length != 10){
                return false;
            }
            return true;
        }
    }
}
