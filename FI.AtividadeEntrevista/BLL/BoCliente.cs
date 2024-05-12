using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
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
                erros.Add("CPF Invalido para o cliente!");
            }
            if (VerificarExistencia(cpf))
            {
                erros.Add("CPF já cadastrado para outro cliente!");
            }

            return erros;
        }

    }
}
