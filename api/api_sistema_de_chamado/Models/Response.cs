﻿namespace api_sistema_de_chamado.Models
{
    public class Response<T>
    {
        public T? Dados { get; set; }
        public string? Mensagem { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
