namespace fitapp.DTOs
{
        public class ExternalLoginDTO
        {
            public required string Provider { get; set; }  // "Google", "Facebook", "Apple"
            public required string Email { get; set; }  // "Google", "Facebook", "Apple"
            public required string Token { get; set; }
        }
    

}
