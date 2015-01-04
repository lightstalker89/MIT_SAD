namespace VirtualMachineClient.Models
{
    public class SuccessResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public VmInfo[] Data { get; set; }
    }
}
