using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace websimulador.Models
{
    [Table("sensor_data")]
    public class SensorData : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("device_id")]
        public Guid DeviceId { get; set; }

        [Column("temperature")]
        public decimal Temperature { get; set; }

        [Column("humidity")]
        public decimal Humidity { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
