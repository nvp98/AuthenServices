using System.ComponentModel.DataAnnotations;

namespace AuthenServices.Models
{
    public class Users
    {
        [Key]
        public int ID { get; set; }
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string MatKhau { get; set; }
        public int TinhTrangLV { get; set; }
        public string? Email { get; set; }
    }
}
