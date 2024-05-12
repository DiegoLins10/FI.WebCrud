using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            cli.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta o beneficiario pelo id
        /// </summary>
        /// <param name="idCliente">id do beneficiario</param>
        /// <returns></returns>
        public List<DML.Beneficiario> Consultar(long idCliente)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.Consultar(idCliente);
        }

        /// <summary>
        /// Excluir o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<DML.Beneficiario> Listar()
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os beneficiarios
        /// </summary>
        public List<DML.Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoBeneficiario cli = new DAL.DaoBeneficiario();
            return cli.VerificarExistencia(CPF);
        }

        public bool CpfValido(string CPF)
        {
            // Verifica se o CPF é nulo, vazio ou consiste apenas de espaços em branco
            if (string.IsNullOrWhiteSpace(CPF))
                return false;

            // Remove caracteres especiais do CPF e preenche com zeros à esquerda, se necessário
            CPF = new string(CPF.Where(char.IsDigit).ToArray()).PadLeft(11, '0');

            // Verifica se o CPF tem 11 dígitos após a formatação
            if (CPF.Length != 11)
                return false;

            // Verifica se todos os dígitos do CPF são iguais
            if (CPF.Distinct().Count() == 1)
                return false;

            // Calcula os dígitos verificadores
            int[] peso1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] peso2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCPF = CPF.Substring(0, 9);
            int d1 = CalcularDigitoVerificador(tempCPF, peso1);
            int d2 = CalcularDigitoVerificador(tempCPF + d1, peso2);

            // Verifica se os dígitos verificadores calculados correspondem aos dígitos reais
            return CPF.EndsWith(d1.ToString() + d2.ToString());
        }

        private int CalcularDigitoVerificador(string cpfParcial, int[] pesos)
        {
            int soma = cpfParcial.Select((c, i) => int.Parse(c.ToString()) * pesos[i]).Sum();
            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        public List<string> ProcessarValidacaoCPF(string cpf)
        {
            List<string> erros = new List<string>();

            if (!CpfValido(cpf))
            {
                erros.Add("CPF Invalido!");
            }
            if (VerificarExistencia(cpf))
            {
                erros.Add("CPF já cadastrado!");
            }

            return erros;
        }

    }
}
