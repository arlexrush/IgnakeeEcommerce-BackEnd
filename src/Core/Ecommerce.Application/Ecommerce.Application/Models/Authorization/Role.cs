using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Authorization
{
    public static class Role
    {
        public const string ADMIN = nameof(ADMIN);
        public const string USER = nameof(USER);

        /// <summary>Service-to-service role for the IgnakeeAI.McpServer.Supplier integration.</summary>
        public const string SUPPLIER_INTEGRATION = nameof(SUPPLIER_INTEGRATION);

    }
}
