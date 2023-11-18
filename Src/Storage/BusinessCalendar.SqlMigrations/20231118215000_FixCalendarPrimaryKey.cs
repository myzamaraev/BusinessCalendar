using FluentMigrator;

namespace BusinessCalendar.SqlMigrations;
[Migration(2023111800000)]
public class FixCalendarPrimaryKey : Migration
{
    public override void Up()
    {
        Delete.PrimaryKey("PK_Calendar").FromTable(TableNames.Calendar);
        Create.PrimaryKey("PK_Calendar").OnTable(TableNames.Calendar)
            .Columns("Type", "Key", "Year");
    }

    public override void Down()
    {
        Delete.PrimaryKey("PK_Calendar").FromTable(TableNames.Calendar);
        Create.PrimaryKey("PK_Calendar").OnTable(TableNames.Calendar)
            .Columns("Type", "Key");
    }
}