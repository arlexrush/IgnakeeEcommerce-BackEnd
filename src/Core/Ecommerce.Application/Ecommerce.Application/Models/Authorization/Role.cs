namespace Ecommerce.Application.Models.Authorization
{
    public static class Role
    {
        public const string ADMIN = nameof(ADMIN);
        public const string INVENTORY_READER = nameof(INVENTORY_READER);
        public const string USER = nameof(USER);

        /// <summary>Service-to-service role for the IgnakeeAI.McpServer.Supplier integration.</summary>
        public const string SUPPLIER_INTEGRATION = nameof(SUPPLIER_INTEGRATION);

    }
}
