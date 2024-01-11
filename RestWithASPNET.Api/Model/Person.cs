using RestWithASPNET.Api.Model.Base;

namespace RestWithASPNET.Api.Model
{
    public class Person : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public bool Enabled { get; set; }
    }
}