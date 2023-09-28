using System;
using System.Collections.Generic;

namespace netcore_supabase.DbModels
{
    /// <summary>
    /// Auth: Manages SSO identity provider information; see saml_providers for SAML.
    /// </summary>
    public partial class SsoProvider
    {
        public SsoProvider()
        {
            SamlProviders = new HashSet<SamlProvider>();
            SamlRelayStates = new HashSet<SamlRelayState>();
            SsoDomains = new HashSet<SsoDomain>();
        }

        public Guid Id { get; set; }
        /// <summary>
        /// Auth: Uniquely identifies a SSO provider according to a user-chosen resource ID (case insensitive), useful in infrastructure as code.
        /// </summary>
        public string? ResourceId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<SamlProvider> SamlProviders { get; set; }
        public virtual ICollection<SamlRelayState> SamlRelayStates { get; set; }
        public virtual ICollection<SsoDomain> SsoDomains { get; set; }
    }
}
