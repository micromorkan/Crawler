namespace Crawler.Model.PortalExtratoClube
{
    public class PortalExtratoClube
    {
        public long id { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string sexo { get; set; }
        public string rg { get; set; }
        public string orgaoEmissor { get; set; }
        public string expedicao { get; set; }
        public string naturalidade { get; set; }
        public string ufNascimento { get; set; }
        public string ufDocumento { get; set; }
        public string nacionalidade { get; set; }
        public string escolaridade { get; set; }
        public string estadoCivil { get; set; }
        public int idade { get; set; }
        public string nasc { get; set; }
        public string status { get; set; }
        public string telefone1 { get; set; }
        public string telefone2 { get; set; }
        public string telefoneComercial { get; set; }
        public string celularLead { get; set; }
        public string celular1 { get; set; }
        public string celular2 { get; set; }
        public string parentesco { get; set; }
        public string pai { get; set; }
        public string mae { get; set; }
        public string email { get; set; }
        public string cnh { get; set; }
        public string preferencial { get; set; }
        public string identidade { get; set; }
        public string error { get; set; }
        public List<Beneficio> beneficios { get; set; }
    }
}
