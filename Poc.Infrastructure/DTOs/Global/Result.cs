using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Poc.Infrastructure.DTOs.Global
{
    public class Result<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "success";
        public T? Data { get; set; }
    }
}
