using System;
using System.Collections.Generic;
using System.Text;

namespace Ucl.PontoNet.Application.Dto
{
    public class TokenDto
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }
}
