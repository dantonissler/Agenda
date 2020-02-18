using System;

namespace Agenda.Models
{
    public class Atividades
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Status { get; set; }
    }
}
