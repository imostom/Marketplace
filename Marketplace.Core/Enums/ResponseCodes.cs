using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Core.Enums
{
    public enum ResponseCodes
    {
        [Description("Success")]
        Success = 00,

        [Description("invalid Account")]
        invalid_Account = 07,

        [Description("Invalid Amount")]
        Invalid_Amount = 13,

        [Description("Not Found")]
        Not_Found = 13,

        [Description("Insufficient Funds")]
        Insufficient_Funds = 51,

        [Description("Limit Exceeded")]
        Limit_Exceeded = 61,

        [Description("Security Violation")]
        Security_Violation = 63,

        [Description("Failed")]
        Failed = 90,

        [Description("Duplicate Transaction")]
        Duplicate_Transaction = 94,

        [Description("System Malfunction")]
        System_Malfunction = 96
    }
}