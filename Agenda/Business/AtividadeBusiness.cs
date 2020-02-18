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
                    Id = Convert.ToInt32(atividade["Id"]),
                    Nome = atividade["Nome"].ToString(),
                    Descricao = atividade["Descricao"].ToString(),
                    DataInicio = Convert.ToDateTime(atividade["DataInicio"]),
                    DataFim = Convert.ToDateTime(atividade["DataFim"]),
                    Status = DefinirStatus(atividade)
                });
            }

            return atividades;
        }

        public Atividades Obter(int id)
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
                DataInicio = Convert.ToDateTime(atividade.Get("datainicio")),
                DataFim = Convert.ToDateTime(atividade.Get("datafim"))
            };
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

        public bool ValidarSatusPraFazer(DateTime dataInicial)
        {
            bool praFazer = false;

            if (dataInicial <= DateTime.Now)
            {
                praFazer = true;
            }

            return praFazer;
        }

        public bool ValidarStatusPendente(DateTime dataInicial)
        {
            bool pendente = false;

            if (dataInicial >= DateTime.Now)
            {
                pendente = true;
            }

            return pendente;
        }
        public bool ValidarSatusNaoFazer(DateTime dataInicial, DateTime dataFim)
        {
            return string.IsNullOrEmpty(dataInicial.ToString()) && string.IsNullOrEmpty(dataFim.ToString()) ? true : false;
        }

        public bool ValidarConcluida(DateTime dataFim)
        {
            return string.IsNullOrEmpty(dataFim.ToString()) ? true : false;
        }

        //public string ValidarDatas(DateTime dataInicio, DateTime dataFim, string fail, string success)
        //{
        //    try
        //    {
        //        ValidarData(dataFim);
        //        ValidarData(dataInicio);
        //        ValidarData(dataInicio, dataFim);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(fail, ex); ;
        //    }
        //    return success;
        //}

        public string DefinirStatus(DataRow dados)
        {
            string descricaoStatus = string.Empty;

            DateTime dataInicio = Convert.ToDateTime(dados["DataInicio"].ToString());
            DateTime dataFim = Convert.ToDateTime(dados["DataFim"].ToString());

            bool validaStatusPraFazer = ValidarSatusPraFazer(dataInicio);
            bool validaStatusPendente = ValidarStatusPendente(dataInicio);
            bool validaStatusConcluida = ValidarConcluida(dataFim);
            bool validaStatusNaoFazer = ValidarSatusNaoFazer(dataInicio, dataFim);

            if (validaStatusPraFazer) descricaoStatus = "Pra fazer";
            if(validaStatusPendente) descricaoStatus = "Pendente";
            if(validaStatusConcluida) descricaoStatus = "Concluída";
            if (validaStatusConcluida) descricaoStatus = "Concluída";
            if (validaStatusNaoFazer) descricaoStatus = "Não Fazer";
            return descricaoStatus;
        }
    }
}
