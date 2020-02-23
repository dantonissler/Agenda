using System;
using System.Collections.Generic;
using System.Data;
using Agenda.Models;
using Repository;
using Util;

namespace Agenda.Business
{
    public class AtividadeBusiness
    {
        private readonly ConnectionDB _connectionDB;

        public AtividadeBusiness()
        {
            _connectionDB = new ConnectionDB();
        }

        public List<Atividades> Obter()
        {
            string script = @"SELECT * FROM atividades";
            DataSet atividadesDoBanco = _connectionDB.SelectDataFromMySql(script);
            List<Atividades> atividades = new List<Atividades>();

            foreach (DataRow atividade in atividadesDoBanco.Tables[0].Rows)
            {
                atividades.Add(new Atividades
                {
                    Id = Convert.ToInt32(atividade["id"]),
                    Nome = atividade["nome"].ToString(),
                    Descricao = atividade["descricao"].ToString(),
                    DataInicio = string.IsNullOrEmpty(atividade["datainicio"].ToString()) ? (DateTime?)null : Convert.ToDateTime(atividade["datainicio"]),
                    DataFim = string.IsNullOrEmpty(atividade["datafim"].ToString()) ? (DateTime?)null : Convert.ToDateTime(atividade["datafim"]),
                    Status = DefinirStatus(atividade)
                });
            }

            return atividades;
        }

        public Atividades Obter(int id)
        {
            try
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>
                {
                    { "id", id }
                };
                string script = @"SELECT id, 
                                     nome,
                                     descricao,
                                     datainicio,
                                     datafim
                              FROM atividades
                              WHERE id = @id";
                DataSet atividade = _connectionDB.SelectDataFromMySql(script, parametros);
                return new Atividades
                {
                    Id = Convert.ToInt32(atividade.Get("id")),
                    Nome = atividade.Get("nome"),
                    Descricao = atividade.Get("descricao"),
                    DataInicio = string.IsNullOrEmpty(atividade.Get("datainicio").ToString()) ? (DateTime?)null : Convert.ToDateTime(atividade.Get("datainicio")),
                    DataFim = string.IsNullOrEmpty(atividade.Get("datafim").ToString()) ? (DateTime?)null : Convert.ToDateTime(atividade.Get("datafim"))
                };
            }
            catch 
            {
                return null;
            }
        }

        public void Salvar(Atividades atividade)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "Nome", atividade.Nome },
                { "Descricao", atividade.Descricao },
                { "DataInicio", atividade.DataInicio },
                { "DataFim", atividade.DataFim },
            };

            string script = "INSERT INTO atividades (nome, descricao, datainicio, datafim) VALUES (@Nome, @Descricao, @DataInicio, @DataFim)";
            _connectionDB.Execute(script, parametros);
        }

        public void Alterar(Atividades atividade)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "id" , atividade.Id },
                { "Nome", atividade.Nome },
                { "Descricao", atividade.Descricao },
                { "DataInicio", atividade.DataInicio },
                { "DataFim", atividade.DataFim },
            };

            string script = "UPDATE atividades SET nome = @Nome, descricao = @Descricao, datainicio = @DataInicio, datafim = @DataFim where id = @id";
            _connectionDB.Execute(script, parametros);
        }

        public void Excluir(int id)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "id", id }
            };

            string script = "DELETE FROM atividades WHERE Id = @id";
            _connectionDB.Execute(script, parametros);
        }

        public bool ValidarSatusPraFazer(DateTime? dataInicial, DateTime? dataFim)
        {
            return  (string.IsNullOrEmpty(dataFim.ToString()) && (dataInicial > DateTime.Now)) ? true : false ;
        }
        public bool ValidarStatusPendente(DateTime? dataInicial, DateTime? dataFim)
        {
            return ((dataInicial >= DateTime.Now) && (dataFim > DateTime.Now)) ? true : false;
        }
        public bool ValidarConcluida(DateTime? dataInicial, DateTime? dataFim)
        {
            return ((dataInicial < DateTime.Now) && (dataFim < DateTime.Now)) ? true : false;
        }
        public bool ValidarSatusNaoFazer(DateTime? dataInicial, DateTime? dataFim)
        {
            return (string.IsNullOrEmpty(dataInicial.ToString()) && string.IsNullOrEmpty(dataFim.ToString())) ? true : false;
        }
        public string DefinirStatus(DataRow dados)
        {
            string descricaoStatus = string.Empty;
            DateTime? dataInicio = string.IsNullOrEmpty(dados["datainicio"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dados["datainicio"]);
            DateTime? dataFim = string.IsNullOrEmpty(dados["datafim"].ToString()) ? (DateTime?)null : Convert.ToDateTime(dados["datafim"]);

            bool validaStatusPraFazer = ValidarSatusPraFazer(dataInicio, dataFim);
            bool validaStatusPendente = ValidarStatusPendente(dataInicio, dataFim);
            bool validaStatusConcluida = ValidarConcluida(dataInicio, dataFim);
            bool validaStatusNaoFazer = ValidarSatusNaoFazer(dataInicio, dataFim);

            if (validaStatusPraFazer) descricaoStatus = "Pra fazer";
            if (validaStatusPendente) descricaoStatus = "Pendente";
            if (validaStatusConcluida) descricaoStatus = "Concluída";
            if (validaStatusNaoFazer) descricaoStatus = "Não Fazer";

            return descricaoStatus;
        }
    }
}
