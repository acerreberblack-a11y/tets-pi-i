using System;

namespace IPWhiteListManager.Models
{
    public class IPAddressInfo
    {
        public int Id { get; set; }
        public int SystemId { get; set; }
        public string IPAddress { get; set; }
        public EnvironmentType Environment { get; set; }
        public bool IsRegisteredInNamen { get; set; }
        public string NamenRequestNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string SystemName { get; set; }
    }
}