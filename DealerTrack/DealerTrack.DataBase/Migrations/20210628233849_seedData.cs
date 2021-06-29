using Microsoft.EntityFrameworkCore.Migrations;

namespace DealerTrack.DataBase.Migrations
{
    public partial class seedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into [Customers](Name)
            VALUES('Milli Fulton'),
            ('Rahima Skinner'),
            ('Aroush Knapp')"
            );

            migrationBuilder.Sql(@"insert into [Vehicles](Name)
            VALUES('2009 Lamborghini Gallardo Carbon Fiber LP-560'),
            ('2016 Porsche 911 2dr Cpe GT3 RS'),
            ('2017 Ferrari 488 Spider')"
            );

            migrationBuilder.Sql(@"insert into [Dealerships](Name)
            VALUES('Sun of Saskatoon'),
            ('Seven Star Dealership'),
            ('Maxwell & Junior')"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete from [Customers]"
            );

            migrationBuilder.Sql(@"delete from [Vehicles]"
            );

            migrationBuilder.Sql(@"delete from [Dealerships]"
            );
        }
    }
}
