using System;
using System.Collections.Generic;
using System.Text;

namespace Ucl.PontoNet.Application.Dto
{
    public class UserInfoDto
    {
        public string sub { get; set; }
        public string mail { get; set; }
        public string locationCountry { get; set; }
        public string FirstName { get; set; }
        public string ValeVALENETID { get; set; }
        public string ValeCompanyCode { get; set; }
        public string ValeCPF { get; set; }
        public string cn { get; set; }
        public string sn { get; set; }
        public string EmployeeID { get; set; }
        public List<string> groupMembership { get; set; }
        public string UserFullName { get; set; }
    }
}
