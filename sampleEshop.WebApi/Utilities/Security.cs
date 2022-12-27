using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampleEshop.WebApi
{
    public static class Security
    {
        public static string secretKeyStr = "ProjectJwtBearer";//!!!!!!!!!!!!!!!!!!!! must be unique
        public static string issuer = "https://localhost:44300/"; //!!!!!!!!!!!!!!!!!!!!!!!!

        public static string corsPolicyName = "EnableCors"; //!!!!!!!!!!!!!!!!!!!!!!!!

        
    }
}
