namespace DeskFlow.API.Models
{
    public class ErrorDTO
    {
        public string Error { get; set; }

        public ErrorDTO(string error)
        {
            Error = error;
        }
    }
}