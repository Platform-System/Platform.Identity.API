using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Identity.API.Infrastructure.Data.Migrations
{
    public partial class RemoveAuthorizationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP TABLE IF EXISTS "RolePermissions" CASCADE;
                DROP TABLE IF EXISTS "Permissions" CASCADE;
                DROP TABLE IF EXISTS "Roles" CASCADE;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "Permissions" (
                    "Id" uuid NOT NULL,
                    "Name" text NOT NULL,
                    "CreatedAt" timestamp with time zone NOT NULL,
                    "CreatedBy" text NULL,
                    "UpdatedAt" timestamp with time zone NULL,
                    "UpdatedBy" text NULL,
                    "IsSoftDeleted" boolean NOT NULL,
                    "DeletedAt" timestamp with time zone NULL,
                    "DeletedBy" text NULL,
                    CONSTRAINT "PK_Permissions" PRIMARY KEY ("Id")
                );

                CREATE UNIQUE INDEX IF NOT EXISTS "IX_Permissions_Name" ON "Permissions" ("Name");

                CREATE TABLE IF NOT EXISTS "Roles" (
                    "Id" uuid NOT NULL,
                    "Name" text NOT NULL,
                    "CreatedAt" timestamp with time zone NOT NULL,
                    "CreatedBy" text NULL,
                    "UpdatedAt" timestamp with time zone NULL,
                    "UpdatedBy" text NULL,
                    "IsSoftDeleted" boolean NOT NULL,
                    "DeletedAt" timestamp with time zone NULL,
                    "DeletedBy" text NULL,
                    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
                );

                CREATE UNIQUE INDEX IF NOT EXISTS "IX_Roles_Name" ON "Roles" ("Name");

                CREATE TABLE IF NOT EXISTS "RolePermissions" (
                    "Id" uuid NOT NULL,
                    "RoleId" uuid NOT NULL,
                    "PermissionId" uuid NOT NULL,
                    "CreatedAt" timestamp with time zone NOT NULL,
                    "CreatedBy" text NULL,
                    "UpdatedAt" timestamp with time zone NULL,
                    "UpdatedBy" text NULL,
                    "IsSoftDeleted" boolean NOT NULL,
                    "DeletedAt" timestamp with time zone NULL,
                    "DeletedBy" text NULL,
                    CONSTRAINT "PK_RolePermissions" PRIMARY KEY ("Id"),
                    CONSTRAINT "FK_RolePermissions_Permissions_PermissionId" FOREIGN KEY ("PermissionId") REFERENCES "Permissions" ("Id") ON DELETE CASCADE,
                    CONSTRAINT "FK_RolePermissions_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
                );

                CREATE INDEX IF NOT EXISTS "IX_RolePermissions_PermissionId" ON "RolePermissions" ("PermissionId");
                CREATE UNIQUE INDEX IF NOT EXISTS "IX_RolePermissions_RoleId_PermissionId" ON "RolePermissions" ("RoleId", "PermissionId");
                """);
        }
    }
}
