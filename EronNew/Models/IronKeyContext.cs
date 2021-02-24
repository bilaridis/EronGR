using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

#nullable disable

namespace EronNew.Models
{

    public partial class IronKeyContext : DbContext, IIronKeyContext
    {

        public IronKeyContext(DbContextOptions<IronKeyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserInterest> AspNetUserInterests { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserNote> AspNetUserNotes { get; set; }
        public virtual DbSet<AspNetUserNotesDetail> AspNetUserNotesDetails { get; set; }
        public virtual DbSet<AspNetUserProfile> AspNetUserProfiles { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUserSavedSearch> AspNetUserSavedSearches { get; set; }
        public virtual DbSet<Culture> Cultures { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PostsModel> Posts { get; set; }
        public virtual DbSet<ExtraInformation> ExtraInformations { get; set; }
        public virtual DbSet<PostFeature> PostFeatures { get; set; }
        public virtual DbSet<PostsHistory> PostsHistories { get; set; }
        public virtual DbSet<TypesModel> Types { get; set; }
        public virtual DbSet<Areas> Areas { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PostsImages> PostsImages { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=Bilaridis2020\\SqlExpress;Database=IronKey;User Id=IIS;Password=IIS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                //entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                //    .IsUnique()
                //    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                //entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                //entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                //entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                //    .IsUnique()
                //    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                //entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                //entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                //entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<PostsModel>(entity =>
            {
                //entity.Property(e => e.id).HasColumnName("id");
                //entity.Property(e => e.SubTypeInformation).HasColumnName("Type");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Condition)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("condition");

                entity.Property(e => e.ConstructionYear).HasColumnName("constructionYear");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EnergyEfficiency)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("energyEfficiency");

                entity.Property(e => e.Floor).HasColumnName("floor");

                entity.Property(e => e.Furniture).HasColumnName("furniture").HasDefaultValueSql("((0))");
                entity.Property(e => e.View).HasDefaultValueSql("((0))");
                entity.Property(e => e.Sold).HasDefaultValueSql("((0))");
                entity.Property(e => e.Reserved).HasDefaultValueSql("((0))");
                entity.Property(e => e.ParkingArea).HasDefaultValueSql("((0))");

                entity.Property(e => e.ParkingAreaType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PetAllowed).HasColumnName("petAllowed").HasDefaultValueSql("((0))");

                entity.Property(e => e.RenovationYear).HasColumnName("renovationYear");

                entity.Property(e => e.Reserved).HasDefaultValueSql("((0))");

                entity.Property(e => e.SearchRawKey)
                    .HasMaxLength(64)
                    .HasColumnName("searchRawKey");

                entity.Property(e => e.Sold).HasDefaultValueSql("((0))");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_AspNetUsers");

                entity.HasOne(d => d.SubTypeInformation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.SubTypeId)
                    .HasConstraintName("FK_Posts_Types1");
                entity.HasOne(d => d.Areas)
                    .WithMany(p => p.PostAreas)
                    .HasForeignKey(d => d.Area)
                    .HasConstraintName("FK_Posts_Areas");
                entity.HasOne(d => d.SubAreas)
                    .WithMany(p => p.PostSubAreas)
                    .HasForeignKey(d => d.SubAreaId)
                    .HasConstraintName("FK_Posts_SubAreas");
            });

            modelBuilder.Entity<ExtraInformation>(entity =>
            {
                entity.HasKey(e => e.PostId);

                entity.ToTable("PostsExtraInformation");

                entity.Property(e => e.AirCondition).HasColumnName("airCondition");

                entity.Property(e => e.Bbq).HasColumnName("bbq");

                entity.Property(e => e.Elevator).HasColumnName("elevator");

                entity.Property(e => e.FPostId).HasColumnName("F_PostId");

                entity.Property(e => e.Fireplace).HasColumnName("fireplace");

                entity.Property(e => e.Garden).HasColumnName("garden");

                entity.Property(e => e.GardenSpace).HasColumnName("gardenSpace");

                entity.Property(e => e.Gym).HasColumnName("gym");

                entity.Property(e => e.Hall).HasColumnName("hall");

                entity.Property(e => e.Heating).HasColumnName("heating");

                entity.Property(e => e.HeatingSystem)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("heatingSystem");

                entity.Property(e => e.Kitchen).HasColumnName("kitchen");

                entity.Property(e => e.Livingroom).HasColumnName("livingroom");

                entity.Property(e => e.Maidroom).HasColumnName("maidroom");

                entity.Property(e => e.Master).HasColumnName("master");

                entity.Property(e => e.RoofFloor).HasColumnName("roofFloor");

                entity.Property(e => e.SemiOutdoor).HasColumnName("semiOutdoor");

                entity.Property(e => e.SemiOutdoorSquare).HasColumnName("semiOutdoorSquare");

                entity.Property(e => e.Storageroom).HasColumnName("storageroom");

                entity.Property(e => e.StorageroomSquare).HasColumnName("storageroomSquare");

                entity.Property(e => e.Swimmingpool).HasColumnName("swimmingpool");

                entity.Property(e => e.Wc).HasColumnName("wc");

                entity.HasOne(d => d.FPost)
                    .WithMany(p => p.ExtraInformation)
                    .HasForeignKey(d => d.FPostId)
                    .HasConstraintName("FK_PostExtraInformation_Posts");
            });

            modelBuilder.Entity<TypesModel>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Desc)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Areas>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.AreaName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PostsImages>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FpostId).HasColumnName("FPostId");

                entity.Property(e => e.ImageName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UploadedDate).HasColumnType("datetime");

                entity.Property(e => e.UrlImage)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Posts)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.FpostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostsImages_Posts");
            });

            modelBuilder.Entity<AspNetUserInterest>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AspNetUserId)
                    .HasMaxLength(450);

                entity.Property(e => e.ClickDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Fbclid)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("fbclid");

                entity.Property(e => e.Location)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.AspNetUserInterests)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspNetUserInterests_Posts");
            });

            modelBuilder.Entity<AspNetUserProfile>(entity =>
            {
                entity.ToTable("AspNetUserProfile");

                entity.Property(e => e.AspNetUserId).HasMaxLength(450);

                entity.Property(e => e.EmailAccount)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Facebook)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("facebook");

                entity.Property(e => e.Info)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.InfoText)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.LinkedIn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoImage).HasMaxLength(600);

                entity.Property(e => e.SubTitle)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Twitter)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.ToTable("Wallet", "wallet");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AspNetUserId).HasMaxLength(450);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AspNetUser)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.AspNetUserId)
                    .HasConstraintName("FK_Wallet_AspNetUsers");
            });


            modelBuilder.Entity<WishList>(entity =>
            {
                entity.Property(e => e.Added).HasColumnType("datetime");

                entity.Property(e => e.AspNetUserId).HasMaxLength(450);

                entity.Property(e => e.FpostId).HasColumnName("FPostId");

                entity.Property(e => e.Removed).HasColumnType("datetime");

                entity.Property(e => e.WishListName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.AspNetUser)
                    .WithMany(p => p.WishLists)
                    .HasForeignKey(d => d.AspNetUserId)
                    .HasConstraintName("FK_WishLists_AspNetUsers");

                entity.HasOne(d => d.Fpost)
                    .WithMany(p => p.WishLists)
                    .HasForeignKey(d => d.FpostId)
                    .HasConstraintName("FK_WishLists_Posts");
            });

            modelBuilder.Entity<AspNetUserNote>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AspNetUserId).HasMaxLength(450);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.AspNetUser)
                    .WithMany(p => p.AspNetUserNotes)
                    .HasForeignKey(d => d.AspNetUserId)
                    .HasConstraintName("FK_AspNetUserNotes_AspNetUsers");
               
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.AspNetUserNotes)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_AspNetUserNotes_Posts");
            });

            modelBuilder.Entity<AspNetUserNotesDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FnoteId).HasColumnName("FNoteId");

                entity.HasOne(d => d.Fnote)
                    .WithMany(p => p.AspNetUserNotesDetails)
                    .HasForeignKey(d => d.FnoteId)
                    .HasConstraintName("FK_AspNetUserNotesDetails_AspNetUserNotes");
            });

            modelBuilder.Entity<PostsHistory>(entity =>
            {
                entity.ToTable("PostsHistory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostsHistories)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostsHistory_Posts");
            });

            modelBuilder.Entity<PostFeature>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BasicFeatures).HasDefaultValueSql("((1))");

                entity.Property(e => e.FpostId).HasColumnName("FPostId");

                entity.Property(e => e.PremiumLanguage).HasDefaultValueSql("((0))");

                entity.Property(e => e.PremiumPreview360).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Fpost)
                    .WithMany(p => p.PostFeatures)
                    .HasForeignKey(d => d.FpostId)
                    .HasConstraintName("FK_PostFeatures_Posts");
            });

            modelBuilder.Entity<Culture>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.CultureName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FpostId).HasColumnName("FPostId");

                entity.Property(e => e.OwnerId).HasMaxLength(450);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Fpost)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.FpostId)
                    .HasConstraintName("FK_Orders_Posts");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_Orders_AspNetUsers");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Orders_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.ScopeOfProduct)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeOfPayment)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AspNetUserSavedSearch>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AspNetUserId).HasMaxLength(450);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Title).HasMaxLength(1000);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.AspNetUser)
                    .WithMany(p => p.AspNetUserSavedSearches)
                    .HasForeignKey(d => d.AspNetUserId)
                    .HasConstraintName("FK_AspNetUserSavedSearches_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
