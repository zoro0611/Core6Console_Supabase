using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace netcore_supabase.DbModels
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLogEntry> AuditLogEntries { get; set; } = null!;
        public virtual DbSet<Bucket> Buckets { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<DecryptedSecret> DecryptedSecrets { get; set; } = null!;
        public virtual DbSet<FlowState> FlowStates { get; set; } = null!;
        public virtual DbSet<Identity> Identities { get; set; } = null!;
        public virtual DbSet<Instance> Instances { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<MfaAmrClaim> MfaAmrClaims { get; set; } = null!;
        public virtual DbSet<MfaChallenge> MfaChallenges { get; set; } = null!;
        public virtual DbSet<MfaFactor> MfaFactors { get; set; } = null!;
        public virtual DbSet<Migration> Migrations { get; set; } = null!;
        public virtual DbSet<Object> Objects { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<SamlProvider> SamlProviders { get; set; } = null!;
        public virtual DbSet<SamlRelayState> SamlRelayStates { get; set; } = null!;
        public virtual DbSet<SchemaMigration> SchemaMigrations { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        public virtual DbSet<SsoDomain> SsoDomains { get; set; } = null!;
        public virtual DbSet<SsoProvider> SsoProviders { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("User Id=postgres;Password=A127753029x;Server=db.nnjbdmgmqqhlffxyxrrq.supabase.co;Port=5432;Database=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
                .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
                .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
                .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn" })
                .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
                .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
                .HasPostgresExtension("extensions", "pg_stat_statements")
                .HasPostgresExtension("extensions", "pgcrypto")
                .HasPostgresExtension("extensions", "pgjwt")
                .HasPostgresExtension("extensions", "uuid-ossp")
                .HasPostgresExtension("graphql", "pg_graphql")
                .HasPostgresExtension("pgsodium", "pgsodium")
                .HasPostgresExtension("vault", "supabase_vault");

            modelBuilder.Entity<AuditLogEntry>(entity =>
            {
                entity.ToTable("audit_log_entries", "auth");

                entity.HasComment("Auth: Audit trail for user actions.");

                entity.HasIndex(e => e.InstanceId, "audit_logs_instance_id_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.InstanceId).HasColumnName("instance_id");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(64)
                    .HasColumnName("ip_address")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.Payload)
                    .HasColumnType("json")
                    .HasColumnName("payload");
            });

            modelBuilder.Entity<Bucket>(entity =>
            {
                entity.ToTable("buckets", "storage");

                entity.HasIndex(e => e.Name, "bname")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AllowedMimeTypes).HasColumnName("allowed_mime_types");

                entity.Property(e => e.AvifAutodetection)
                    .HasColumnName("avif_autodetection")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.FileSizeLimit).HasColumnName("file_size_limit");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.Property(e => e.Public)
                    .HasColumnName("public")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Buckets)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("buckets_owner_fkey");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Categoryname).HasColumnName("categoryname");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");
            });

            modelBuilder.Entity<DecryptedSecret>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("decrypted_secrets", "vault");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.DecryptedSecret1)
                    .HasColumnName("decrypted_secret")
                    .UseCollation("C");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.KeyId).HasColumnName("key_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Nonce).HasColumnName("nonce");

                entity.Property(e => e.Secret).HasColumnName("secret");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<FlowState>(entity =>
            {
                entity.ToTable("flow_state", "auth");

                entity.HasComment("stores metadata for pkce logins");

                entity.HasIndex(e => e.CreatedAt, "flow_state_created_at_idx")
                    .HasSortOrder(new[] { SortOrder.Descending });

                entity.HasIndex(e => e.AuthCode, "idx_auth_code");

                entity.HasIndex(e => new { e.UserId, e.AuthenticationMethod }, "idx_user_id_auth_method");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AuthCode).HasColumnName("auth_code");

                entity.Property(e => e.AuthenticationMethod).HasColumnName("authentication_method");

                entity.Property(e => e.CodeChallenge).HasColumnName("code_challenge");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.ProviderAccessToken).HasColumnName("provider_access_token");

                entity.Property(e => e.ProviderRefreshToken).HasColumnName("provider_refresh_token");

                entity.Property(e => e.ProviderType).HasColumnName("provider_type");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Identity>(entity =>
            {
                entity.HasKey(e => new { e.Provider, e.Id })
                    .HasName("identities_pkey");

                entity.ToTable("identities", "auth");

                entity.HasComment("Auth: Stores identities associated to a user.");

                entity.HasIndex(e => e.Email, "identities_email_idx")
                    .HasOperators(new[] { "text_pattern_ops" });

                entity.HasIndex(e => e.UserId, "identities_user_id_idx");

                entity.Property(e => e.Provider).HasColumnName("provider");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasComputedColumnSql("lower((identity_data ->> 'email'::text))", true)
                    .HasComment("Auth: Email is a generated column that references the optional email property in the identity_data");

                entity.Property(e => e.IdentityData)
                    .HasColumnType("jsonb")
                    .HasColumnName("identity_data");

                entity.Property(e => e.LastSignInAt).HasColumnName("last_sign_in_at");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Identities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("identities_user_id_fkey");
            });

            modelBuilder.Entity<Instance>(entity =>
            {
                entity.ToTable("instances", "auth");

                entity.HasComment("Auth: Manages users across multiple sites.");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.RawBaseConfig).HasColumnName("raw_base_config");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.Uuid).HasColumnName("uuid");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Stock).HasColumnName("stock");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.Categoryid)
                    .HasConstraintName("Menu_categoryid_fkey");
            });

            modelBuilder.Entity<MfaAmrClaim>(entity =>
            {
                entity.ToTable("mfa_amr_claims", "auth");

                entity.HasComment("auth: stores authenticator method reference claims for multi factor authentication");

                entity.HasIndex(e => new { e.SessionId, e.AuthenticationMethod }, "mfa_amr_claims_session_id_authentication_method_pkey")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AuthenticationMethod).HasColumnName("authentication_method");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.MfaAmrClaims)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("mfa_amr_claims_session_id_fkey");
            });

            modelBuilder.Entity<MfaChallenge>(entity =>
            {
                entity.ToTable("mfa_challenges", "auth");

                entity.HasComment("auth: stores metadata about challenge requests made");

                entity.HasIndex(e => e.CreatedAt, "mfa_challenge_created_at_idx")
                    .HasSortOrder(new[] { SortOrder.Descending });

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.FactorId).HasColumnName("factor_id");

                entity.Property(e => e.IpAddress).HasColumnName("ip_address");

                entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

                entity.HasOne(d => d.Factor)
                    .WithMany(p => p.MfaChallenges)
                    .HasForeignKey(d => d.FactorId)
                    .HasConstraintName("mfa_challenges_auth_factor_id_fkey");
            });

            modelBuilder.Entity<MfaFactor>(entity =>
            {
                entity.ToTable("mfa_factors", "auth");

                entity.HasComment("auth: stores metadata about factors");

                entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "factor_id_created_at_idx");

                entity.HasIndex(e => new { e.FriendlyName, e.UserId }, "mfa_factors_user_friendly_name_unique")
                    .IsUnique()
                    .HasFilter("(TRIM(BOTH FROM friendly_name) <> ''::text)");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.FriendlyName).HasColumnName("friendly_name");

                entity.Property(e => e.Secret).HasColumnName("secret");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MfaFactors)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("mfa_factors_user_id_fkey");
            });

            modelBuilder.Entity<Migration>(entity =>
            {
                entity.ToTable("migrations", "storage");

                entity.HasIndex(e => e.Name, "migrations_name_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ExecutedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("executed_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Hash)
                    .HasMaxLength(40)
                    .HasColumnName("hash");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Object>(entity =>
            {
                entity.ToTable("objects", "storage");

                entity.HasIndex(e => new { e.BucketId, e.Name }, "bucketid_objname")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "name_prefix_search")
                    .HasOperators(new[] { "text_pattern_ops" });

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.BucketId).HasColumnName("bucket_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.LastAccessedAt)
                    .HasColumnName("last_accessed_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Metadata)
                    .HasColumnType("jsonb")
                    .HasColumnName("metadata");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.Property(e => e.PathTokens)
                    .HasColumnName("path_tokens")
                    .HasComputedColumnSql("string_to_array(name, '/'::text)", true);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Version).HasColumnName("version");

                entity.HasOne(d => d.Bucket)
                    .WithMany(p => p.Objects)
                    .HasForeignKey(d => d.BucketId)
                    .HasConstraintName("objects_bucketId_fkey");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Objects)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("objects_owner_fkey");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens", "auth");

                entity.HasComment("Auth: Store of tokens used to refresh JWT tokens once they expire.");

                entity.HasIndex(e => e.InstanceId, "refresh_tokens_instance_id_idx");

                entity.HasIndex(e => new { e.InstanceId, e.UserId }, "refresh_tokens_instance_id_user_id_idx");

                entity.HasIndex(e => e.Parent, "refresh_tokens_parent_idx");

                entity.HasIndex(e => new { e.SessionId, e.Revoked }, "refresh_tokens_session_id_revoked_idx");

                entity.HasIndex(e => e.Token, "refresh_tokens_token_unique")
                    .IsUnique();

                entity.HasIndex(e => e.UpdatedAt, "refresh_tokens_updated_at_idx")
                    .HasSortOrder(new[] { SortOrder.Descending });

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.InstanceId).HasColumnName("instance_id");

                entity.Property(e => e.Parent)
                    .HasMaxLength(255)
                    .HasColumnName("parent");

                entity.Property(e => e.Revoked).HasColumnName("revoked");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .HasColumnName("token");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("refresh_tokens_session_id_fkey");
            });

            modelBuilder.Entity<SamlProvider>(entity =>
            {
                entity.ToTable("saml_providers", "auth");

                entity.HasComment("Auth: Manages SAML Identity Provider connections.");

                entity.HasIndex(e => e.EntityId, "saml_providers_entity_id_key")
                    .IsUnique();

                entity.HasIndex(e => e.SsoProviderId, "saml_providers_sso_provider_id_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AttributeMapping)
                    .HasColumnType("jsonb")
                    .HasColumnName("attribute_mapping");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.EntityId).HasColumnName("entity_id");

                entity.Property(e => e.MetadataUrl).HasColumnName("metadata_url");

                entity.Property(e => e.MetadataXml).HasColumnName("metadata_xml");

                entity.Property(e => e.SsoProviderId).HasColumnName("sso_provider_id");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.SsoProvider)
                    .WithMany(p => p.SamlProviders)
                    .HasForeignKey(d => d.SsoProviderId)
                    .HasConstraintName("saml_providers_sso_provider_id_fkey");
            });

            modelBuilder.Entity<SamlRelayState>(entity =>
            {
                entity.ToTable("saml_relay_states", "auth");

                entity.HasComment("Auth: Contains SAML Relay State information for each Service Provider initiated login.");

                entity.HasIndex(e => e.CreatedAt, "saml_relay_states_created_at_idx")
                    .HasSortOrder(new[] { SortOrder.Descending });

                entity.HasIndex(e => e.ForEmail, "saml_relay_states_for_email_idx");

                entity.HasIndex(e => e.SsoProviderId, "saml_relay_states_sso_provider_id_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.ForEmail).HasColumnName("for_email");

                entity.Property(e => e.FromIpAddress).HasColumnName("from_ip_address");

                entity.Property(e => e.RedirectTo).HasColumnName("redirect_to");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.SsoProviderId).HasColumnName("sso_provider_id");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.SsoProvider)
                    .WithMany(p => p.SamlRelayStates)
                    .HasForeignKey(d => d.SsoProviderId)
                    .HasConstraintName("saml_relay_states_sso_provider_id_fkey");
            });

            modelBuilder.Entity<SchemaMigration>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("schema_migrations_pkey");

                entity.ToTable("schema_migrations", "auth");

                entity.HasComment("Auth: Manages updates to the auth system.");

                entity.Property(e => e.Version)
                    .HasMaxLength(255)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("sessions", "auth");

                entity.HasComment("Auth: Stores session data associated to a user.");

                entity.HasIndex(e => e.NotAfter, "sessions_not_after_idx")
                    .HasSortOrder(new[] { SortOrder.Descending });

                entity.HasIndex(e => e.UserId, "sessions_user_id_idx");

                entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "user_id_created_at_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.FactorId).HasColumnName("factor_id");

                entity.Property(e => e.NotAfter)
                    .HasColumnName("not_after")
                    .HasComment("Auth: Not after is a nullable column that contains a timestamp after which the session should be regarded as expired.");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("sessions_user_id_fkey");
            });

            modelBuilder.Entity<SsoDomain>(entity =>
            {
                entity.ToTable("sso_domains", "auth");

                entity.HasComment("Auth: Manages SSO email address domain mapping to an SSO Identity Provider.");

                entity.HasIndex(e => e.SsoProviderId, "sso_domains_sso_provider_id_idx");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Domain).HasColumnName("domain");

                entity.Property(e => e.SsoProviderId).HasColumnName("sso_provider_id");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.SsoProvider)
                    .WithMany(p => p.SsoDomains)
                    .HasForeignKey(d => d.SsoProviderId)
                    .HasConstraintName("sso_domains_sso_provider_id_fkey");
            });

            modelBuilder.Entity<SsoProvider>(entity =>
            {
                entity.ToTable("sso_providers", "auth");

                entity.HasComment("Auth: Manages SSO identity provider information; see saml_providers for SAML.");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.ResourceId)
                    .HasColumnName("resource_id")
                    .HasComment("Auth: Uniquely identifies a SSO provider according to a user-chosen resource ID (case insensitive), useful in infrastructure as code.");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "auth");

                entity.HasComment("Auth: Stores user login data within a secure schema.");

                entity.HasIndex(e => e.ConfirmationToken, "confirmation_token_idx")
                    .IsUnique()
                    .HasFilter("((confirmation_token)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.EmailChangeTokenCurrent, "email_change_token_current_idx")
                    .IsUnique()
                    .HasFilter("((email_change_token_current)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.EmailChangeTokenNew, "email_change_token_new_idx")
                    .IsUnique()
                    .HasFilter("((email_change_token_new)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.ReauthenticationToken, "reauthentication_token_idx")
                    .IsUnique()
                    .HasFilter("((reauthentication_token)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.RecoveryToken, "recovery_token_idx")
                    .IsUnique()
                    .HasFilter("((recovery_token)::text !~ '^[0-9 ]*$'::text)");

                entity.HasIndex(e => e.Email, "users_email_partial_key")
                    .IsUnique()
                    .HasFilter("(is_sso_user = false)");

                entity.HasIndex(e => e.InstanceId, "users_instance_id_idx");

                entity.HasIndex(e => e.Phone, "users_phone_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Aud)
                    .HasMaxLength(255)
                    .HasColumnName("aud");

                entity.Property(e => e.BannedUntil).HasColumnName("banned_until");

                entity.Property(e => e.ConfirmationSentAt).HasColumnName("confirmation_sent_at");

                entity.Property(e => e.ConfirmationToken)
                    .HasMaxLength(255)
                    .HasColumnName("confirmation_token");

                entity.Property(e => e.ConfirmedAt)
                    .HasColumnName("confirmed_at")
                    .HasComputedColumnSql("LEAST(email_confirmed_at, phone_confirmed_at)", true);

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.EmailChange)
                    .HasMaxLength(255)
                    .HasColumnName("email_change");

                entity.Property(e => e.EmailChangeConfirmStatus)
                    .HasColumnName("email_change_confirm_status")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.EmailChangeSentAt).HasColumnName("email_change_sent_at");

                entity.Property(e => e.EmailChangeTokenCurrent)
                    .HasMaxLength(255)
                    .HasColumnName("email_change_token_current")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.EmailChangeTokenNew)
                    .HasMaxLength(255)
                    .HasColumnName("email_change_token_new");

                entity.Property(e => e.EmailConfirmedAt).HasColumnName("email_confirmed_at");

                entity.Property(e => e.EncryptedPassword)
                    .HasMaxLength(255)
                    .HasColumnName("encrypted_password");

                entity.Property(e => e.InstanceId).HasColumnName("instance_id");

                entity.Property(e => e.InvitedAt).HasColumnName("invited_at");

                entity.Property(e => e.IsSsoUser)
                    .HasColumnName("is_sso_user")
                    .HasComment("Auth: Set this column to true when the account comes from SSO. These accounts can have duplicate emails.");

                entity.Property(e => e.IsSuperAdmin).HasColumnName("is_super_admin");

                entity.Property(e => e.LastSignInAt).HasColumnName("last_sign_in_at");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.PhoneChange)
                    .HasColumnName("phone_change")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.PhoneChangeSentAt).HasColumnName("phone_change_sent_at");

                entity.Property(e => e.PhoneChangeToken)
                    .HasMaxLength(255)
                    .HasColumnName("phone_change_token")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.PhoneConfirmedAt).HasColumnName("phone_confirmed_at");

                entity.Property(e => e.RawAppMetaData)
                    .HasColumnType("jsonb")
                    .HasColumnName("raw_app_meta_data");

                entity.Property(e => e.RawUserMetaData)
                    .HasColumnType("jsonb")
                    .HasColumnName("raw_user_meta_data");

                entity.Property(e => e.ReauthenticationSentAt).HasColumnName("reauthentication_sent_at");

                entity.Property(e => e.ReauthenticationToken)
                    .HasMaxLength(255)
                    .HasColumnName("reauthentication_token")
                    .HasDefaultValueSql("''::character varying");

                entity.Property(e => e.RecoverySentAt).HasColumnName("recovery_sent_at");

                entity.Property(e => e.RecoveryToken)
                    .HasMaxLength(255)
                    .HasColumnName("recovery_token");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .HasColumnName("role");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
