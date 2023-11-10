using FluentMigrator;

namespace BusinessCalendar.SqlMigrations;

[Migration(2023110900000)]
public class InitialMigration : Migration
{
    private const string CalendarIdentifierTableName = "CalendarIdentifier";
    private const string CalendarTableName = "Calendar";
    public override void Up()
    {
        Create.Table(CalendarIdentifierTableName)
            .WithColumn("Id").AsString(64).NotNullable().PrimaryKey() //Type(15)+_(1)+Key(48)
            .WithColumn("Type").AsString(15).NotNullable()
            .WithColumn("Key").AsString(48).NotNullable();

        Create.Index("Idx_Type_Key")
            .OnTable(CalendarIdentifierTableName)
            .OnColumn("Type").Ascending()
            .OnColumn("Key").Ascending()
            .WithOptions()
            .NonClustered();

        Create.Table(CalendarTableName)
            .WithColumn("Type").AsString(15).NotNullable().PrimaryKey()
            .WithColumn("Key").AsString(48).NotNullable().PrimaryKey()
            .WithColumn("Year").AsInt16().NotNullable()
            .WithColumn("Document").AsString();
    }

    public override void Down()
    {
        Delete.Table(CalendarIdentifierTableName);
        Delete.Table(CalendarTableName);
    }
}