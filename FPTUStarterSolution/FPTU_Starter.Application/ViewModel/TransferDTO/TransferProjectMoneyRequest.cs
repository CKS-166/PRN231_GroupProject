using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel.TransferDTO
{
    public class TransferProjectMoneyRequest
    {
        public Guid ProjectID { get; set; }
        public Guid DestinationWalletID { get; set; }      
    }
}
