using System;

namespace IPWhiteListManager.Models
{
    public class SystemInfo
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public bool IsTestProductionCombined { get; set; }
        public string CuratorName { get; set; }
        public string CuratorEmail { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}