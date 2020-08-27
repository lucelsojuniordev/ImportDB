using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncDB.SQL
{
    class GerarSQL
    {
        public GerarSQL() { }

        public string Produtos()
        {       
            return @"select
            substring(i.codigo from 1 for 6) Codigo,
            substring(i.descricao from 1 for 250) Descricao,
            substring(coalesce(i.descricaotecnica,
            substring(i.descricao from 1 for 250)) from 1 for 250) DescricaoTecnica,
            substring(coalesce(i.referencia,'') from 1 for 250) Referencia,
            '' FotoPath, cast(p.preco as decimal(10,2)) PrecoVenda,
            cast(coalesce(i.precocompra,0.00) as decimal(10,2)) Precocompra,
            cast(coalesce(i.customedio,0.00) as decimal(10,2)) valorCusto,
            cast(coalesce(i.custocontabil,0.00) as decimal(10,2)) valorCustoContabil,
            cast(coalesce(i.descontomaximo,0.00) as decimal(10,2)) DescontoMax,
            cast(coalesce(i.estoque,0) as decimal(10,4)) EstoqueDisponivel,
            cast(coalesce(i.estoqueconsigentrada,0) as decimal(10,4)) EstoqueReservado,
            0 StatusCadastro ,
            coalesce(g.codigo,0) GrupoCodigo,
            coalesce(g.descricaogrupo,'Nao Informado') GrupoDescricao,
            coalesce(f.codigo,0) FabricanteCodigo,
            coalesce(f.descricao,'Nao Informado') FabricanteDescricao,
            coalesce(u.unidade,0) UnidadeSigla,
            coalesce(u.descricao,'Nao Informado') unidadeDescricao
            from itens i inner join produtospreco p on i.item = p.item and p.tabelapreco = 0
            inner join unidades u on u.unidade = i.unidade
            inner join grupos g on g.grupo = i.grupo
            inner join fabricantes f on f.fabricante = i.fabricante ";
        }

        public string Favorecidos()
        {
            return @"select
            substring(i.codigo from 1 for 250) Codigo,
            substring(i.razao from 1 for 250) Razao,
            substring(coalesce(i.nome,'Nome') from 1 for 250) Fantasia,
            substring(coalesce(i.cpf_cnpj,'') from 1 for 14) CPF_CNPJ,
            substring(coalesce(i.rg,'') from 1 for 14) RG,
            substring(coalesce(i.email,'') from 1 for 150) Email,
            substring(coalesce(i.inscricao_est,'') from 1 for 14) Inscricao_Est,
            substring(coalesce(i.inscricao_mun,'') from 1 for 14) Inscricao_Mun,
            '' Inscricao_Sub,
            '' CNAE,
            case when (coalesce(i.pessoa_fj,'') = 'F' or i.pessoa_fj is null) then 1 else 0 end  Pessoa_FJ,
            1 TipoTributcao,
            cast(coalesce(i.datacadastro,'1999-01-01') as varchar (10)) DataCadastro,
            cast(coalesce(i.datanasc,'1999-01-01') as varchar (10)) DataNascimento,
            '1999-01-01' DataAlteracao,
            substring(coalesce(i.fone1,'') from 1 for 25) Telefone,
            substring(coalesce(i.celular,'') from 1 for 25) WhatsApp,
            substring(coalesce(i.celular,'') from 1 for 25) Celular,
            substring(coalesce(i.contato,'') from 1 for 100) Contato,
            1 TipoFavorecido,
            cast(coalesce(i.limitecredito,0) as decimal (10,2)) LimiteCredito,
            substring(coalesce(i.site,'') from 1 for 250) Site,
            substring(coalesce(i.obs,'') from 1 for 250) Obs,
            substring(coalesce(i.sexo,'N') from 1 for 1) Sexo,
            substring(coalesce(i.substitutotrib,'N') from 1 for 1) SubstitutoTrib,
            substring(coalesce(i.permitircheque,'N') from 1 for 1) PermitirCheque,
            substring(coalesce(i.permitirfiado,'N') from 1 for 1) PermitirFiado,
            substring(coalesce(i.consumidorfinal,'N') from 1 for 1) ConsumidorFinal,
            '' FotoPath,
            substring(coalesce(i.endereco,'Logradouro') from 1 for 250) Logradouro,
            substring(coalesce(i.bairro,'Bairro') from 1 for 250) Bairro,
            coalesce(i.municipio,(select municipio from favorecidos where favorecido = -1)) Municipio,
            coalesce(i.uf,(select uf from favorecidos where favorecido = -1)) UF,
            replace(replace(substring(coalesce(i.cep,(select cep from favorecidos where favorecido = -1)) from 1 for 10),' ',''),'-','') CEP,
            substring(coalesce(i.nro,'') from 1 for 6) Numero,
            '' Complemento,
            coalesce(i.pais,1058) Pais,
            case when desativado = 'S' then 1 else 0 end StatusCadastro
            from favorecidos i where tipofavorecido = 1
            /* Regras */
            and i.cpf_cnpj is not null
            and replace(i.cpf_cnpj,' ','') <> ''
            and i.razao is not null";
        }

        public string Empresa()
        {
            return @"select
            substring(f.razao from 1 for 250) Razao,
            substring(coalesce(f.nome, f.razao) from 1 for 250) Fantasia,
            substring(coalesce(f.cpf_cnpj,'') from 1 for 14) CPF_CNPJ,
            substring(coalesce(f.email,'') from 1 for 150) Email,
            substring(coalesce(f.inscricao_est,'') from 1 for 14) Inscricao_Est,
            substring(coalesce(f.inscricao_mun,'') from 1 for 14) Inscricao_Mun,
            substring(coalesce(e.inscricao_est_sub,'') from 1 for 14) Inscricao_Sub,
            substring(coalesce(e.cnae,'') from 1 for 10) CNAE,
            case when f.pessoa_fj = 'J' then 1 else 0 end Pessoa_FJ,
            case when f.tipoempresa in ('S','N') then 2 when f.tipoempresa = 'P' then 3 when f.tipoempresa in ('R', 'A') then 4 else 1 end TipoTributcao,
            0 Status,
            0 Situacao,
            cast(coalesce(f.datacadastro,'1999-01-01') as varchar (10)) DataCadastro,
            cast(coalesce(f.datacadastro,'1999-01-01') as varchar (10)) DataAlteracao,
            '1999-01-01' ValidadeContrato,
            0.00 ValorContrato,
            '' FotoPath,
            case when (e.cae is null or e.cae = '') then 'N' else 'S' end Industria,
            coalesce(e.cae,'') Industria_CAE,
            coalesce(e.contribipi,'N') ContribIPI,
            substring(coalesce(f.substitutotrib,'N') from 1 for 1) SubstitutoTRIB,
            coalesce(e.aliqcreditosn,0) AliqCreditoSN,
            coalesce(e.aliqdifal,0) AliqDIFAL,
            substring(coalesce(e.csc,'') from 1 for 50) CSC,
            substring(coalesce(e.flexdocs,'') from 1 for 150) FlexDocs,
            substring(coalesce(e.idtoken,'') from 1 for 250) TokenCSC,
            '' Certificado,
            '' TokenCARTAO,
            '' ClienteIdCARTAO,
            substring(coalesce(f.endereco,'Logradouro') from 1 for 250) Logradouro,
            substring(coalesce(f.bairro,'Bairro') from 1 for 250) Bairro,
            coalesce(f.municipio,0) Municipio,
            coalesce(f.uf,'') UF,
            replace(replace(substring(coalesce(f.cep,(select cep from favorecidos where favorecido = -1)) from 1 for 10),' ',''),'-','') CEP,
            substring(coalesce(f.nro,'') from 1 for 6) Numero,
            coalesce(e.complemento,'') Complemento,
            coalesce(f.pais,1058) Pais,
            -1 ContadorId
            from favorecidos f inner join empresas e on f.favorecido = e.favorecido where f.favorecido = -1
            /* Regras */
            and f.cpf_cnpj is not null
            and f.razao is not null
            and f.email is not null
            and replace(f.email,' ','') <> ''";
        }

    }
}
