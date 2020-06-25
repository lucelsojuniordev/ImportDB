using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyncDB.Models.Enums;

namespace SyncDB.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string FotoPath { get; set; }
        public string TokenCARTAO { get; set; }
        public string ClienteIdCARTAO { get; set; }
        public string Role { get; set; } = "Visitante";
        public string SiglaPedido { get; set; }
        public TipoAcesso TipoAcesso { get; set; } = TipoAcesso.Visitante;
        public TipoVenda TipoVenda { get; set; } = TipoVenda.PedidoVenda;
        public StatusCadastro StatusCadastro { get; set; } = StatusCadastro.Ativo;
        public PermitirDesconto PermitirDesconto { get; set; } = PermitirDesconto.Bloqueado;
        public FotoNaLista produtoFotoLista { get; set; } = FotoNaLista.Exibir;
        public FotoNaLista clienteFotoLista { get; set; } = FotoNaLista.Exibir;
        public Thema thema { get; set; } = Thema.Dark;
        public int ultimopedido { get; set; } = 0;
        public int mododebug { get; set; } = 0;
        public int EmpresaId { get; set; }
        public string TokenAPI { get; set; }
        public string ConfirmaPassWord { get; set; }
        public string FotoBase64 { get; set; }

    }
}
