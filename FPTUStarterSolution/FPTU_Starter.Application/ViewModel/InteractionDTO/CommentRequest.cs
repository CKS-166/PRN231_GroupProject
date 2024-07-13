using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.InteractionDTO
{
    public class CommentRequest
    {
        public string? Content { get; set; }
        public Guid ProjectId { get; set; }
    }
}
