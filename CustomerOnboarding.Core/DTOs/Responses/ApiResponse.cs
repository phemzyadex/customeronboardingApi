using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Core.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null)
            => new ApiResponse<T> { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message, string v)
            => new ApiResponse<T> { Success = false, Message = message };
    }
}
