Entity Framework 6 For PostgreSQL
Install-Package Npgsql

Install-Package EntityFramework6.Npgsql
1. Code First For PostgreSQL
Web.config
<connectionStrings>
    <add name="SampleDbContext" connectionString="Server=localhost;Port=5432;Database=SampleDB;User Id=postgres;Password=123456;" providerName="Npgsql" />
</connectionStrings>
&

<entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
        <parameters>
            <parameter value="mssqllocaldb" />
        </parameters>
    </defaultConnectionFactory>
    <providers>
        <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
        <provider invariantName="Npgsql" type="Npgsql.NpgsqlServices, EntityFramework6.Npgsql" />
    </providers>
</entityFramework>
&

<system.data>
    <DbProviderFactories>
        <remove invariant="Npgsql" />
        <add name="Npgsql Data Provider" invariant="Npgsql" support="FF" description=".Net Framework Data Provider for Postgresql" type="Npgsql.NpgsqlFactory, Npgsql" />
    </DbProviderFactories>
</system.data>
Entity Framework Code First For PostgreSQL
