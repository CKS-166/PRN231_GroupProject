using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.CommentDTO
{
    public class CommentViewResponse
    {
        public string? Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }   
    }
}
