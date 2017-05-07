using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSG.Deezer.Models
{
    class ErrorResponse
    {
        public Error Error { get; set; }
    }

    class Error
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
