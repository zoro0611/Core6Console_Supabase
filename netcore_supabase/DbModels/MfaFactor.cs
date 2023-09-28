using System;
using System.Collections.Generic;

namespace netcore_supabase.DbModels
{
    /// <summary>
    /// auth: stores metadata about factors
    /// </summary>
    public partial class MfaFactor
    {
        public MfaFactor()
        {
            MfaChallenges = new HashSet<MfaChallenge>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? FriendlyName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Secret { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<MfaChallenge> MfaChallenges { get; set; }
    }
}
