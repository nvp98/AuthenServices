namespace AuthenServices.DTOs
{
    public class EmployeeInfoDto
    {
        public string manv { get; set; }
        public string hoten { get; set; }
        public string ngaysinh { get; set; }
        public string diachi { get; set; }
        public string email { get; set; }
        public string ngayvaolam { get; set; }

        public string tinhtranglamviec { get; set; } 

        public string ngaynghiviec { get; set; }
        public int maphongban { get; set; }
        public string phongban { get; set; }
        public string mavitri { get; set; }
        public string vitri { get; set; }
        public string makip { get; set; }
        public string tenkip { get; set; }
        public string tolamviec { get; set; }
        public string phanxuong { get; set; }
    }
    public class EmployeeApiResponse
    {
        public string Result { get; set; }
        public object Content { get; set; }
        public List<EmployeeInfoDto> Data { get; set; }
    }
}
