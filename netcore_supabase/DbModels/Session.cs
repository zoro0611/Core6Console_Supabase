﻿using System;
using System.Collections.Generic;

namespace netcore_supabase.DbModels
{
    /// <summary>
    /// Auth: Stores session data associated to a user.
    /// </summary>
    public partial class Session
    {
        public Session()
        {
            MfaAmrClaims = new HashSet<MfaAmrClaim>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? FactorId { get; set; }
        /// <summary>
        /// Auth: Not after is a nullable column that contains a timestamp after which the session should be regarded as expired.
        /// </summary>
        public DateTime? NotAfter { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<MfaAmrClaim> MfaAmrClaims { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
