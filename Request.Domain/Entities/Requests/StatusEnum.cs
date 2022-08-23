using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Domain.Entities.Requests
{
    public static class StatusEnum
    {
        public const string Waiting = "Đang chờ";
        public const string Accept = "Đồng ý";
        public const string Refuse = "Từ chối";
        public const string Cancel = "Đã hủy";
    }
}