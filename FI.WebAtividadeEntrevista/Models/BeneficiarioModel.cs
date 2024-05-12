using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public class Beneficiario
        {

            public long Id { get; set; }

            public string CPF { get; set; }

            public string Nome { get; set; }

            public long IdCliente { get; set; }
        }
    }
}