using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.InteractionDTO
{
    public class LikeResponse
    {
        public Guid ProjectId { get; set; }
        public Guid UserID { get; set; }
        //public bool IsLike { get; set; }
    }
}
