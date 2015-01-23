using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudMarketPlaceService
{
    public interface ICommentVirtualMachineBehavior
    {
        DataLayer.MarketPlaceServiceResponse CommentVirtualMachine(DataLayer.VirtualMachine virtualMachineComment, string comment);
    }
}
