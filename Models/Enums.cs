using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.Models
{
    public class Enums
    {
        public enum TipoEndereco
        {
            Residencial = 0,
            ResidencialEntrega = 1,
            Empresarial = 2,
            EmpresarialAlmoxarifado = 3,
            EmpresarialEntrega = 4,
            EmpresarialDeposito = 5,
        }
        public enum TipoTributacao
        {
            ConsumidorFinal = 0,
            MicroIndividual = 1,
            SimplesNacional = 2,
            LucloPresumido = 3,
            LucroReal = 4
        }

        public enum Thema { Dark = 0, Light = 1 }

        public enum TipoAcesso { SuperUsuario = 0, Gerente = 1, Financeiro = 2, Vendedor = 3, Visitante = 4 }

        public enum TipoFavorecido { Empresa = 0, Cliente = 1, Fornecedor = 2, Funcionario = 3 }

        public enum TipoVenda { PedidoVenda = 0, VendaDireta = 1, RemessaAVenda = 2 }

        public enum StatusPedido { Aberto = 0, Salvo = 1, Pago = 2, Enviado = 3 }

        public enum StatusCadastro { Ativo = 0, Inativo = 1 }

        public enum StatusEmpresa { Ativo = 0, Bloqueado = 1 }

        public enum StatusSistema { Testando = 0, Ativo = 1 }

        public enum TipoEntidade { Fisica = 0, Juridica = 1 }

        public enum PermitirDesconto { Permitido = 0, Bloqueado = 1 }

        public enum FotoNaLista { Exibir = 0, Ocultar = 1 }
    }
}
