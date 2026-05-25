using Microsoft.EntityFrameworkCore;

namespace VisageSmart.Infrastructure.Persistence;

public static class ApplicationDbContextSchemaBootstrap
{
    public static async Task EnsureSchemaAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS "Services" (
                "Id" uuid NOT NULL,
                "Name" character varying(150) NOT NULL DEFAULT '',
                "Category" character varying(80) NOT NULL DEFAULT '',
                CONSTRAINT "PK_Services" PRIMARY KEY ("Id")
            );
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS customers (
                id uuid NOT NULL,
                name character varying(150) NOT NULL DEFAULT '',
                phone character varying(30) NOT NULL DEFAULT '',
                email character varying(150) NOT NULL DEFAULT '',
                CONSTRAINT "PK_customers" PRIMARY KEY (id)
            );
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS products (
                id uuid NOT NULL,
                name character varying(150) NOT NULL DEFAULT '',
                quantity_in_stock integer NOT NULL DEFAULT 0,
                purchase_price numeric(18,2) NOT NULL DEFAULT 0,
                selling_price numeric(18,2) NOT NULL DEFAULT 0,
                CONSTRAINT "PK_products" PRIMARY KEY (id)
            );
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE customers
            ADD COLUMN IF NOT EXISTS name character varying(150) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE customers
            ADD COLUMN IF NOT EXISTS phone character varying(30) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE customers
            ADD COLUMN IF NOT EXISTS email character varying(150) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE products
            ADD COLUMN IF NOT EXISTS name character varying(150) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE products
            ADD COLUMN IF NOT EXISTS quantity_in_stock integer NOT NULL DEFAULT 0;
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE products
            ADD COLUMN IF NOT EXISTS purchase_price numeric(18,2) NOT NULL DEFAULT 0;
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE products
            ADD COLUMN IF NOT EXISTS selling_price numeric(18,2) NOT NULL DEFAULT 0;
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Services"
            ADD COLUMN IF NOT EXISTS "Name" character varying(150) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Services"
            ADD COLUMN IF NOT EXISTS "Category" character varying(80) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE UNIQUE INDEX IF NOT EXISTS "IX_Services_Name" ON "Services" ("Name");
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS "Professionals" (
                "Id" uuid NOT NULL,
                "Name" character varying(150) NOT NULL DEFAULT '',
                "Phone" character varying(30) NOT NULL DEFAULT '',
                "Email" character varying(150) NOT NULL DEFAULT '',
                "Cpf" character varying(20) NOT NULL DEFAULT '',
                "Rg" character varying(20) NOT NULL DEFAULT '',
                "Services" jsonb NOT NULL DEFAULT '[]'::jsonb,
                "Availability" jsonb NOT NULL DEFAULT '[]'::jsonb,
                CONSTRAINT "PK_Professionals" PRIMARY KEY ("Id")
            );
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Professionals"
            ADD COLUMN IF NOT EXISTS "Name" character varying(150) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Professionals"
            ADD COLUMN IF NOT EXISTS "Phone" character varying(30) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Professionals"
            ADD COLUMN IF NOT EXISTS "Email" character varying(150) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Professionals"
            ADD COLUMN IF NOT EXISTS "Cpf" character varying(20) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Professionals"
            ADD COLUMN IF NOT EXISTS "Rg" character varying(20) NOT NULL DEFAULT '';
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Professionals"
            ADD COLUMN IF NOT EXISTS "Services" jsonb NOT NULL DEFAULT '[]'::jsonb;
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            ALTER TABLE "Professionals"
            ADD COLUMN IF NOT EXISTS "Availability" jsonb NOT NULL DEFAULT '[]'::jsonb;
            """, cancellationToken);

        await dbContext.Database.ExecuteSqlRawAsync("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_name = 'Professionals'
                      AND column_name = 'ServiceNames'
                ) THEN
                    EXECUTE 'ALTER TABLE "Professionals" ALTER COLUMN "ServiceNames" SET DEFAULT ''[]''::jsonb';
                END IF;
            END
            $$;
            """, cancellationToken);
    }
}
