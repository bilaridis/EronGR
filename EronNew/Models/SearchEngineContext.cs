using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EronNew.Models
{
    public partial class SearchEngineContext : DbContext, ISearchEngineContext
    {
        public SearchEngineContext()
        {
        }

        public SearchEngineContext(DbContextOptions<SearchEngineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Idxentries> Idxentries { get; set; }
        public virtual DbSet<SearchRawData> SearchRawData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=SearchEngine;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Idxentries>(entity =>
            {
                entity.ToTable("idxentries");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DocGuid)
                    .HasColumnName("doc_guid")
                    .HasMaxLength(64);

                entity.Property(e => e.RefCount).HasColumnName("ref_count");

                entity.Property(e => e.Term)
                    .HasColumnName("term")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<SearchRawData>(entity =>
            {
                entity.ToTable("Search_RawData");

                entity.Property(e => e.AddedUser).HasMaxLength(32);

                entity.Property(e => e.Description).HasMaxLength(1024);

                entity.Property(e => e.Handle).HasMaxLength(256);

                entity.Property(e => e.RawKey)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.RawText)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Source).HasMaxLength(32);

                entity.Property(e => e.Title).HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
