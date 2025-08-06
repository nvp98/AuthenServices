namespace AuthenServices.Models
{
    public class Domains
    {
        public int ID { get; set; }
        public string DomainName { get; set; }
        public string DomainUrl { get; set; }
        public string DomainSub { get; set; }
        public bool IsActive { get; set; }
    }
}
